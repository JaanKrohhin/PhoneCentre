const { createProxyMiddleware } = require('http-proxy-middleware');

const target = process.env.API_URL;

const context =  [
    "/events",
];

module.exports = function(app) {
    const appProxy = createProxyMiddleware(context, {
        target: target,
        changeOrigin: true,
        secure: false,
        headers: {
            Connection: 'Keep-Alive'
        }
    });

  app.use(appProxy);
};
