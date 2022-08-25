const colors = require("../../../colors.json")

const prepareColorVariables = require("./src/utils/prepareColorVariables")

const tailwindConfig = prepareColorVariables(colors).tailwindConfig

/**
 * @link https://tailwindcss.com/docs/configuration
 * @type {import('@types/tailwindcss/tailwind-config').TailwindConfig}
 */
const config = {
  content: [
    "./src/**/*.{js,jsx,ts,tsx,scss,css}",
    "./public/index.ejs",
  ],
  theme: {
    container: {
      center: true,
      padding: "1rem",
    },
    colors: {
      transparent: "transparent",
      white: "white",
      black: "black",
      current: "currentColor",
      ...tailwindConfig,
    },
  },
  variants: {
    extend: {
      backgroundColor: ["odd"],
    },
  },
  plugins: [require("@tailwindcss/aspect-ratio")],
}

module.exports = config
