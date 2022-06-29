const isDev = false; // TODO vary with dev/prod

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
