module.exports = {
  purge: ["./src/**/*.{js,jsx,ts,tsx,scss,css}", "./public/index.ejs"],
  darkMode: false, // or 'media' or 'class'
  theme: {
    container: {
      center: true,
      padding: "1rem",
    },
    extend: {
      colors: {
        "brand-primary": "var(--color-brand-primary)",
        "brand-primary-dark": "var(--color-brand-primary-dark)",
        "brand-blue": "var(--color-brand-blue)",
        "brand-green": "var(--color-brand-green)",
        "brand-dark-green": "var(--color-brand-dark-green)",
        "brand-red": "var(--color-brand-red)",
        "brand-gray": "var(--color-brand-gray)",

        // Color uses
        "button-primary": "var(--color-brand-blue)",
        "button-primary-text": "white",

        "button-secondary": "var(--color-brand-primary-dark)",
        "button-secondary-text": "var(--color-brand-primary)",

        "button-gray": "var(--color-brand-gray)",
        "button-gray-text": "var(--color-brand-primary)",

        "button-success": "var(--color-brand-green)",
        "button-success-text": "white",

        "button-danger": "var(--color-brand-red)",
        "button-danger-text": "white",
      },
    },
  },
  variants: {
    extend: {
      backgroundColor: ["odd"],
    },
  },
  plugins: [require("@tailwindcss/aspect-ratio")],
}
