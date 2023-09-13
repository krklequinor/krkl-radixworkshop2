FROM docker.io/node:alpine3.18

WORKDIR /app
COPY ./server.js .
COPY ./package.json .
COPY ./package-lock.json .

RUN npm install

EXPOSE 8030
USER 1000
CMD ["node", "server.js"]
