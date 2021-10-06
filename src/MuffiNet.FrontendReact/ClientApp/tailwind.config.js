/* eslint-disable @typescript-eslint/no-var-requires */
const colors = require("../../../colors.json")
const prepareColorVariables = require("./src/utils/prepareColorVariables")
/* eslint-enable @typescript-eslint/no-var-requires */

const tailwindConfig = prepareColorVariables(colors).tailwindConfig

module.exports = {
  purge: ["./src/**/*.{js,jsx,ts,tsx,scss,css}", "./public/index.ejs"],
  darkMode: false, // or 'media' or 'class'
  theme: {
    container: {
      center: true,
      padding: "1rem",
    },
    colors: {
      transparent: "transparent",
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
