const { configurePostCSS} = require("./config.js");

module.exports = configurePostCSS({ isDev: false }); // TODO vary with dev/prod
