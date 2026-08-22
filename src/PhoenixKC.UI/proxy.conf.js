module.exports = {
  "/api": {
    target: process.env["services__phoenixkc-webapi__https__0"] || process.env["services__phoenixkc-webapi_http__0"],
    secure: process.env["NODE_ENV"] !== "development",
    pathRewrite: {
      "^/api": "api"
    }
  }
}
