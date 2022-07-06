const isDev = require("./mode.js").mode === "development";

/**
 * @type {import('postcss').AcceptedPlugin}
 */
const options = {
  plugins: [
    "tailwindcss",
    "autoprefixer", // NOTE: this plugin uses the .browserslistrc file
    ... isDev ? [] : ["cssnano"],
  ],
};

module.exports = options; 
