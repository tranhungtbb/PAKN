FROM node:8.11.3

# Create app directory
RUN mkdir -p /app
WORKDIR /app

# Install app dependencies
COPY package.json /app
COPY yarn.lock /app
RUN yarn

# Bundle app source
COPY . /app
RUN yarn build

VOLUME /app
EXPOSE 5000
CMD [ "yarn", "start" ]