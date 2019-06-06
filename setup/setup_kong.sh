#!/bin/bash -xv

KONG_ADMIN_URL=$($(dirname $0)/get-kong-url.sh 8001)
KONG_APP_URL=$($(dirname $0)/get-kong-url.sh 8000)

secret=secret123
username=loadtest-loada
KEYS_LOCATION=${1:-.}

tries=0

while [[ $tries -lt 5 ]]; do
	curl -I -s -L ${KONG_APP_URL}
	result=$?
	if [[ $result -eq 0 ]]; then
		break
	fi
	echo "Waiting for Kong..."
	sleep 5
	tries=$[$tries+1]
done

if [[ $tries -eq 5 ]]; then
	exit 1
fi

service_id=$(curl -s -X POST ${KONG_ADMIN_URL}/services/ \
	--data 'name=test' \
	--data 'host=api-stub' \
	--data 'port=8443' \
	--data 'protocol=https' | jq -r '.id')

route_id=$(curl -s -X POST ${KONG_ADMIN_URL}/routes/ \
	--data 'methods[]=GET' \
	--data 'methods[]=PUT' \
	--data 'methods[]=POST' \
	--data 'paths[]=/number-portability/v1' \
	--data 'strip_path=false' \
	--data "service.id=$service_id" | jq -r '.id')

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


curl -o /dev/null -sS -X POST ${KONG_ADMIN_URL}/services/${service_id}/plugins -H 'Content-Type: application/json' -d "${jwt_plugin}"
curl -o /dev/null -sS -X POST ${KONG_ADMIN_URL}/services/${service_id}/plugins/ --data "name=hmac-auth"

curl -o /dev/null -sS -X POST ${KONG_ADMIN_URL}/consumers/ --data "username=${username}"
curl -o /dev/null -sS -X POST ${KONG_ADMIN_URL}/consumers/${username}/hmac-auth/ \
	--data "username=${username}" \
	--data "secret=${secret}"

rm -f "${KEYS_LOCATION}"/{private-key.pem,private-key.pem.pub,public-key.pem,sharedkey.encrypted}
ssh-keygen -m PEM -t rsa -b 4096 -f "${KEYS_LOCATION}/private-key.pem" -N ''
ssh-keygen -e -m PKCS8 -f "${KEYS_LOCATION}/private-key.pem" > "${KEYS_LOCATION}/public-key.pem"
echo -n ${secret} | openssl rsautl -encrypt -inkey ${KEYS_LOCATION}/public-key.pem -pubin -pkcs | base64 | tr -d \\n > "${KEYS_LOCATION}/sharedkey.encrypted"
chmod 0644 "${KEYS_LOCATION}/private-key.pem"

curl -o /dev/null -sS -X POST ${KONG_ADMIN_URL}/consumers/${username}/jwt/ \
	--data-urlencode "key=${username}" \
	--data-urlencode "rsa_public_key=$(<"${KEYS_LOCATION}/public-key.pem")" \
	--data-urlencode 'algorithm=RS256'
