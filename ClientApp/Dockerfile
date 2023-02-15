FROM node:lts-alpine3.16

WORKDIR /ClientApp

Copy package.json .

RUN npm install

COPY . .

EXPOSE 44450

CMD ["npm", "start"]