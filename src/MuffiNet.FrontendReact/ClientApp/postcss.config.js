//const isDev = require("./mode.js").mode === "development"

/**
 * @type {import('postcss').AcceptedPlugin}
 */
const options = {
  plugins: [
    require("tailwindcss"),
    require("autoprefixer"), // NOTE: this plugin uses the .browserslistrc file
    //...(import.meta.env.DEV ? [] : ["cssnano"]),
  ],
}

module.exports = options
