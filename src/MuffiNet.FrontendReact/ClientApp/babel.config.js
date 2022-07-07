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
      "react-hot-loader/babel", // TODO react-hot-loader is deprecated: switch to react-fast-refresh? the webpack plugin is an unstable beta at this time
    ] : [],
  ]
};

module.exports = options;
