#!/bin/bash

KONG_ADMIN_URL=${KONG_ADMIN_URL:-$($(dirname $0)/get-kong-url.sh 8001)}
KONG_APP_URL=${KONG_APP_URL:-$($(dirname $0)/get-kong-url.sh 8000)}

secret=secret123
username=loadtest-loada
KEYS_LOCATION=${KEYS_LOCATION:-${1:-.}}

tries=0

echo "Waiting for Kong to be ready..."
while [[ $tries -lt 5 ]]; do
	curl -o /dev/null -I -s -L ${KONG_APP_URL}
	result=$?
	if [[ $result -eq 0 ]]; then
		break
	fi
	echo "Still waiting"
	sleep 5
	tries=$[$tries+1]
done

if [[ $tries -eq 5 ]]; then
	echo "Quitting, Kong still not available"
	exit 1
fi
echo "Kong up and running"

echo -n "Creating services..."
service_id_np=$(curl -s -X POST ${KONG_ADMIN_URL}/services/ \
	--data 'name=np' \
	--data 'host=np-api-stub' \
	--data 'port=8443' \
	--data 'protocol=https' | jq -r '.id')
service_id_bs=$(curl -s -X POST ${KONG_ADMIN_URL}/services/ \
	--data 'name=bs' \
	--data 'host=bs-api-stub' \
	--data 'port=8443' \
	--data 'protocol=https' | jq -r '.id')
echo " done"

echo -n "Creating routes..."
route_id_np=$(curl -s -X POST ${KONG_ADMIN_URL}/routes/ \
	--data 'methods[]=GET' \
	--data 'methods[]=PUT' \
	--data 'methods[]=POST' \
	--data 'paths[]=/number-portability/v3' \
	--data 'strip_path=false' \
	--data "service.id=$service_id_np" | jq -r '.id')
route_id_bs=$(curl -s -X POST ${KONG_ADMIN_URL}/routes/ \
	--data 'methods[]=GET' \
	--data 'methods[]=PUT' \
	--data 'methods[]=POST' \
	--data 'paths[]=/bundle-switching/v5' \
	--data 'strip_path=false' \
	--data "service.id=$service_id_bs" | jq -r '.id')
echo " done"

jwt_plugin='{
      "name": "jwt",
      "config": {
	"claims_to_verify": [
	  "exp",
	  "nbf"
	],
	"key_claim_name": "iss",
	"cookie_names": [
	  "jwt"
	],
	"maximum_expiration": 0,
	"secret_is_base64": false
      },
      "protocols": ["http", "https"],
      "run_on": "first"
    }'

echo -n "Setting jwt and hmac plugin..."
curl -o /dev/null -sS -X POST ${KONG_ADMIN_URL}/services/${service_id_np}/plugins -H 'Content-Type: application/json' -d "${jwt_plugin}"
curl -o /dev/null -sS -X POST ${KONG_ADMIN_URL}/services/${service_id_np}/plugins/ --data "name=hmac-auth"
curl -o /dev/null -sS -X POST ${KONG_ADMIN_URL}/services/${service_id_bs}/plugins -H 'Content-Type: application/json' -d "${jwt_plugin}"
curl -o /dev/null -sS -X POST ${KONG_ADMIN_URL}/services/${service_id_bs}/plugins/ --data "name=hmac-auth"
echo " done"

echo -n "Creating user..."
curl -o /dev/null -sS -X POST ${KONG_ADMIN_URL}/consumers/ --data "username=${username}"
curl -o /dev/null -sS -X POST ${KONG_ADMIN_URL}/consumers/${username}/hmac-auth/ \
	--data "username=${username}" \
	--data "secret=${secret}"
echo " done"

echo -n "Creating keys..."
rm -f "${KEYS_LOCATION}"/{private-key.pem,private-key.pem.pub,public-key.pem,sharedkey.encrypted}
ssh-keygen -m PEM -t rsa -b 4096 -f "${KEYS_LOCATION}/private-key.pem" -N '' > /dev/null
ssh-keygen -e -m PKCS8 -f "${KEYS_LOCATION}/private-key.pem" > "${KEYS_LOCATION}/public-key.pem"
echo -n ${secret} | openssl rsautl -encrypt -inkey ${KEYS_LOCATION}/public-key.pem -pubin -pkcs | base64 | tr -d \\n > "${KEYS_LOCATION}/sharedkey.encrypted"
chmod 0644 "${KEYS_LOCATION}/private-key.pem"
echo " done"

echo -n "Submitting keys to Kong..."
curl -o /dev/null -sS -X POST ${KONG_ADMIN_URL}/consumers/${username}/jwt/ \
	--data-urlencode "key=${username}" \
	--data-urlencode "rsa_public_key=$(<"${KEYS_LOCATION}/public-key.pem")" \
	--data-urlencode 'algorithm=RS256'
echo " done"
