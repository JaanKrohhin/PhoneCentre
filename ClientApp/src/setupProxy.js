const { createProxyMiddleware } = require('http-proxy-middleware');

const target = 'https://192.168.3.68:44451';

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
