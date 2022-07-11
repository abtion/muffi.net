/**
 * @type {import('postcss').AcceptedPlugin}
 */
const options = {
  modules: true,
  plugins: {
    tailwindcss: {},
    autoprefixer: {}, // NOTE: this plugin uses the .browserslistrc file
    // TODO enable nanocss in prod mode
  },
}

module.exports = options
