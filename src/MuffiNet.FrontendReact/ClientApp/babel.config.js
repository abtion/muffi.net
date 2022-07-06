const isDev = require("./mode.js").mode === "development";

/**
 * @link https://babeljs.io/docs/en/options
 * @type {import('@babel/core').TransformOptions}
 */
const options = {
  babelrc: false,
  presets: [
    [
      "@babel/preset-env",
      {
        debug: false,
        useBuiltIns: false,
        shippedProposals: true,
        // NOTE: this preset uses the .browserslistrc file
      }
    ],
    [
      "@babel/preset-react",
      {
        development: isDev,
        useSpread: true
      }
    ],
    [
      "@babel/preset-typescript",
      {
        allExtensions: true,
        isTSX: true
      }
    ]
  ],
  plugins: [
    "@babel/plugin-syntax-dynamic-import",
    ... isDev ? [
      "react-hot-loader/babel",
    ] : [],
  ]
};

module.exports = options;
