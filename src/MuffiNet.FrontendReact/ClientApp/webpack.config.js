const { configureWebpack} = require("./config.js");

module.exports = (env, { mode }) => configureWebpack({
  isDev: mode === "development",
});
