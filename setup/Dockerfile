FROM alpine
RUN apk update && apk add --no-cache bash curl openssl jq openssh-client
ADD ./setup-kong.sh /setup/setup-kong.sh
WORKDIR /setup
CMD ["/setup/setup-kong.sh"]
