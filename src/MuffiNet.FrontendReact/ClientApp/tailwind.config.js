import colors from "../../../colors.json"

import prepareColorVariables from "./src/utils/prepareColorVariables"

const tailwindConfig = prepareColorVariables(colors).tailwindConfig

/**
 * @link https://tailwindcss.com/docs/configuration
 * @type {import('@types/tailwindcss/tailwind-config').TailwindConfig}
 */
const config = {
  purge: {
    enabled: true, // TODO disable in dev mode
    content: ["./src/**/*.{js,jsx,ts,tsx,scss,css}", "./index.html"],
  },
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

export default config
