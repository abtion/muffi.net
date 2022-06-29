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
        targets: {
          browsers: [
            'last 2 Chrome versions',
            'last 2 Firefox versions',
            'last 2 Edge versions',
            'last 2 Opera versions',
            'last 2 Safari versions',
            'last 2 iOS versions'
          ]
        }
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
