const react = require("@neutrinojs/react")
const jest = require("@neutrinojs/jest")
const path = require("path")
const fs = require("fs")

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
      babel: {
        presets: [
          ["@babel/preset-typescript", { allExtensions: true, isTSX: true }],
        ],
      },
    }),

    jest({
      testRegex: ".*test.(t|j)sx?$",
      testEnvironment: "jsdom",
      collectCoverageFrom: ["./**/*.{js,ts,tsx}", "!./**/*.d.ts"],
      coveragePathIgnorePatterns: [
        "src/authorization",
        "src/index.tsx",
        "src/App.tsx",
        "src/contexts",
        "src/types",
        "src/const",
        "./*.js",
      ],
      transform: {
        "\\.(ts|tsx)$": "@neutrinojs/jest/src/transformer.js", // Compile ts and tsx files with babel (regular js files)
      },
      setupFiles: ["./src/setupTests.ts"],
      setupFilesAfterEnv: ["./src/setupTestsAfterEnv.ts"],
      moduleFileExtensions: ["js", "ts", "json", "tsx", "scss"],
      moduleNameMapper: {
        "~/(.*)$": "<rootDir>/src/$1",
        "\\.svg": "<rootDir>/__mocks__/svgr-webpack.js",
      },
    }),

   
    (neutrino) => {
      // Add typescript extensions
      neutrino.config.resolve.extensions.add(".tsx")
      neutrino.config.resolve.extensions.add(".ts")
      neutrino.config.resolve.alias.set("~", path.resolve(__dirname, "src"))

      neutrino.config.module.rule("compile").test(/\.(wasm|mjs|jsx|js|tsx|ts)$/)

      neutrino.config.devServer.port(process.env.PORT || 44437)
      neutrino.config.devServer.host(process.env.PORT ? "0.0.0.0" : "localhost")
      neutrino.config.devServer.set("proxy", {
        "/api": {
          target: "https://localhost:5001",
          secure: false,
        },
      })

      const baseFolder =
        process.env.APPDATA !== undefined && process.env.APPDATA !== ""
          ? `${process.env.APPDATA}/ASP.NET/https`
          : `${process.env.HOME}/.aspnet/https`

      neutrino.config.devServer.set("https", {
        key: fs.readFileSync(path.resolve(baseFolder,"muffinet.frontendreact.key")),
        cert: fs.readFileSync(path.resolve(baseFolder,"muffinet.frontendreact.pem")),
      })
      neutrino.config.devServer.set("sockPort", "location")
      neutrino.config.devServer.set("disableHostCheck", true)
      neutrino.config.devServer.set("onListening", () => {
        // aspnetcore's React SPA boilerplate waits for this non-configurable "magic string" before concluding that the development server is ready
        console.log("Starting the development server")
      })
    },
  ],
}
