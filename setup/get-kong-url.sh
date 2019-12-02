#!/bin/bash

PORT=$1

if [[ "$(uname -s)" == "Darwin" ]] ; then
	export DOCKER0=0.0.0.0
else
	export DOCKER0=$(printf "import netifaces\nprint(netifaces.gateways()['default'][netifaces.AF_INET][0])\n" | python)
fi
KONG_ADMIN_URL=http://$DOCKER0:$(docker port coin-sdk-dotnet-${CI_JOB_ID:-local}_kong_1 ${PORT} | cut -d: -f2)
echo "${KONG_ADMIN_URL}"
