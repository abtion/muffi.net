/**
 * @link https://eslint.org/docs/latest/user-guide/configuring/configuration-files
 * @type {import('eslint').ESLint.ConfigData}
 */
const options = {
  root: true,
  env: {
    browser: true,
    es2020: true,
    node: true,
    jest: true,
  },
  settings: {
    react: {
      version: "detect",
    },
  },
  plugins: ["@typescript-eslint", "react-refresh"],
  extends: [
    "eslint:recommended",
    "plugin:react-hooks/recommended",
    "plugin:react/recommended",
    "plugin:jest-dom/recommended",
    "plugin:@typescript-eslint/recommended",
    "prettier",
    "plugin:storybook/recommended"
  ],
  parser: "@typescript-eslint/parser",
  rules: {
    "react/jsx-uses-react": "off",
    "react/react-in-jsx-scope": "off",
    "react-refresh/only-export-components": "warn",
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
  ignorePatterns: [
    "coverage/**/*",
    "build/**/*",
    "src/components/AuthOidcProvider/index.tsx",
  ],
}

module.exports = options
