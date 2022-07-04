const isDev = require("./mode.js").mode === "development";

/**
 * @type {import('postcss').AcceptedPlugin}
 */
const options = {
  plugins: [
    "tailwindcss",
    "autoprefixer",
    ... isDev ? [] : ["cssnano"],
  ],
};

module.exports = options; 
