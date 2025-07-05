FROM debian:bullseye-slim

RUN apt-get update \
    && apt-get install -y curl gnupg2 lsb-release \
    && curl -s https://packages.asterisk.org/keys/asterisk-signing-key.pub | apt-key add - \
    && echo "deb http://packages.asterisk.org/deb $(lsb_release -sc) main" > /etc/apt/sources.list.d/asterisk.list \
    && apt-get update \
    && apt-get install -y asterisk asterisk-voicemail

COPY configs/ /etc/asterisk/

EXPOSE 5060/udp 8088 8089

CMD ["asterisk", "-f", "-U", "root"]
