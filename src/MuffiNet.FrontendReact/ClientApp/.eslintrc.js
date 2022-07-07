/**
 * @link https://eslint.org/docs/latest/user-guide/configuring/configuration-files
 * @type {import('eslint').ESLint.ConfigData}
 */
const options = {
  root: true,
  env: {
    browser: true,
    es6: true,
    node: true,
    jest: true,
  },
  settings: {
    react: {
      version: "detect",
    },
  },
  plugins: ["@typescript-eslint"],
  extends: [
    "eslint:recommended",
    "plugin:react/recommended",
    "plugin:jest-dom/recommended",
    "plugin:@typescript-eslint/recommended",
    "prettier",
  ],
  parser: "@typescript-eslint/parser",
  rules: {
    // enable additional rules
    "linebreak-style": ["error", "unix"],
    camelcase: ["error"],

    // Don't allow console.log
    "no-console": ["error"],
    "@typescript-eslint/no-unused-vars": [
      "error",
      { argsIgnorePattern: "^_", varsIgnorePattern: "^_" },
    ],

    "react/prop-types": "off",
  },
  ignorePatterns: ["coverage/**/*", "build/**/*"],
}

module.exports = options
