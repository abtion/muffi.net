import type { Config } from "tailwindcss"
import colors from "./colors.json"
import prepareColorVariables from "./src/utils/prepareColorVariables"

const tailwindConfig = prepareColorVariables(colors).tailwindConfig

export default {
  content: ["./src/**/*.{js,jsx,ts,tsx,scss,css}", "./public/index.ejs"],
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
} satisfies Config
