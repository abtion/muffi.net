const mode = require("./mode").mode;

const colors = require("../../../colors.json");

const prepareColorVariables = require("./src/utils/prepareColorVariables");

const tailwindConfig = prepareColorVariables(colors).tailwindConfig;

/**
 * @link https://tailwindcss.com/docs/configuration
 * @type {import('@types/tailwindcss/tailwind-config').TailwindConfig}
 */
const config = {
  purge: {
    enabled: mode === "production",
    content: [
      "./src/**/*.{js,jsx,ts,tsx,scss,css}",
      "./public/index.ejs"
    ],
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
};

module.exports = config;
