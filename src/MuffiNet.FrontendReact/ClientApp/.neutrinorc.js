const react = require("@neutrinojs/react")
const jest = require("@neutrinojs/jest")
const path = require("path")

module.exports = {
  options: {
    root: __dirname,
  },
  use: [
    (neutrino) =>
      neutrino.config.module
        .rule("svgr")
        .test(/\.svg$/) // you can pick whatever file extension you like here
        .issuer({ test: /\.[tj]sx?$/ }) // optional, but lets you continue importing SVGs from CSS files etc. without it breaking
        .use("@svgr/webpack")
        .loader(require.resolve("@svgr/webpack"))
        .options({ svgoConfig: { plugins: { removeViewBox: false } } }), // allow resizing SVGs (default options strip viewBox for some strange reason)
    // .end()      // these next bits are not needed if your files are named '.svg' since the built-in rules do this for you
    // .use('url') // but if you pick a different extension, this is needed
    // .loader(require.resolve('url-loader')),

    react({
      html: {
        title: "MuffiNet",
        template: require.resolve("./public/index.ejs"),
      },
      style: {
        test: /\.s?css$/,
        loaders: [
          {
            loader: "postcss-loader",
            options: {
              postcssOptions: {
                plugins: [require("tailwindcss"), require("autoprefixer")],
              },
            },
          },
          "sass-loader",
        ],
      },
    }),
    jest({
      testRegex: "src/.*test.js$",
      collectCoverageFrom: ["**/src/**/*.{js,jsx}"],
      coveragePathIgnorePatterns: [
        "src/authorization",
        "src/registerServiceWorker.js",
        "src/index.js",
        "src/App.js",
        "src/contexts",
        "src/components/Nav",
      ],
      setupFiles: ["./src/setupTests.js"],
      setupFilesAfterEnv: ["./src/setupTestsAfterEnv.js"],
      moduleNameMapper: {
        "~/(.*)$": "<rootDir>/src/$1",
        "\\.svg": "<rootDir>/__mocks__/svgr-webpack.js",
      },
    }),
    (neutrino) => {
      neutrino.config.resolve.alias.set("~", path.resolve(__dirname, "src"))
      neutrino.config.devServer.port(process.env.PORT || 3000)
      neutrino.config.devServer.host(process.env.PORT ? "0.0.0.0" : "localhost")
      neutrino.config.devServer.set("sockPort", "location")
      neutrino.config.devServer.set("disableHostCheck", true)
      neutrino.config.devServer.set("onListening", () => {
        // aspnetcore's React SPA boilerplate waits for this non-configurable "magic string" before concluding that the development server is ready
        console.log("Starting the development server")
      })
    },
  ],
}
