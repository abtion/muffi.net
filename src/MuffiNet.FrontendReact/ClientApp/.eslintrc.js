module.exports = {
  env: {
    browser: true,
    es6: true,
    node: true,
    jest: true,
  },
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
  },
  overrides: [
    {
      // enable the rule specifically for TypeScript files
      files: ["*.js"],
      rules: {
        "@typescript-eslint/explicit-module-boundary-types": 0,
      },
    },
    {
      files: ["./*.js"],
      rules: {
        "@typescript-eslint/no-var-requires": 0,
      },
    },
  ],
  ignorePatterns: ["coverage/**/*"],
}
