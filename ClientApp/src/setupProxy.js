const { createProxyMiddleware } = require('http-proxy-middleware');

//Docker-compose file get the var
const target = process.env.API_URL;

const context =  [
    "/events",
];

module.exports = function(app) {
  const appProxy = createProxyMiddleware(context, {
    target: target,
    secure: false,
    headers: {
      Connection: 'Keep-Alive'
    }
  });

  app.use(appProxy);
};
