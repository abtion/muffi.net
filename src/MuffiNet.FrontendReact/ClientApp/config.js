const process = require("process");
const { resolve } = require("path");
const { readFileSync } = require("fs");

const rootDir = __dirname;

const certificateDir = process.env.APPDATA
  ? `${process.env.APPDATA}/ASP.NET/https`
  : `${process.env.HOME}/.aspnet/https`;

const host = "localhost";
const port = 44437;

function configureWebpack({ isDev }) {
  /**
   * @link https://webpack.js.org/configuration/#options
   * @type {import('webpack').Configuration}
   */
  const config = {
    mode: isDev
      ? "development"
      : "production",
    devtool: isDev
      ? "cheap-module-eval-source-map"
      : undefined, // TODO use "source-map" in production build? the Ruby vesion has it, but it didn't immediately work, so more configuration and testing needed here
    target: 'web',
    context: rootDir,
    stats: {
      children: false,
      entrypoints: false,
      modules: false
    },
    node: {
      Buffer: false,
      fs: 'empty',
      tls: 'empty'
    },
    output: {
      path: resolve(rootDir, "build"),
      publicPath: "/",
      filename: isDev
        ? "assets/[name].js"
        : "assets/[name].[contenthash:8].js"
    },
    resolve: {
      alias: {
        '~': resolve(rootDir, "src"),
      },
      extensions: ['.web.jsx', '.web.js', '.wasm', '.mjs', '.jsx', '.js', '.json', '.tsx', '.ts']
    },
    devServer: {
      host,
      port,
      hot: true,
      historyApiFallback: true,
      overlay: true,
      stats: {
        all: false,
        errors: true,
        timings: true,
        warnings: true
      },
      proxy: {
        '/api': {
          target: 'https://localhost:5001',
          secure: false
        },
        '/swagger': {
          target: 'https://localhost:5001',
          secure: false
        },
        '/hubs': {
          target: 'https://localhost:5001',
          secure: false,
          ws: true
        }
      },
      https: {
        key: readFileSync(resolve(certificateDir, "muffinet.frontendreact.key")),
        cert: readFileSync(resolve(certificateDir, "muffinet.frontendreact.pem")),
      },
      sockPort: 'location',
      disableHostCheck: true
    },
    module: {
      rules: [
        {
          test: /\.svg$/,
          issuer: {
            test: /\.[tj]sx?$/ // apply only when svg is imported from jsx/tsx files
          },
          loader: "@svgr\\webpack",
          options: {
            // https://react-svgr.com/docs/options/
            svgoConfig: {
              plugins: {
                removeViewBox: false // allow resizing SVGs (default options strip viewBox for some strange reason)
              }
            }
          }
        },
        {
          test: /\.(wasm|mjs|jsx|js|tsx|ts)$/,
          include: [
            resolve(rootDir, "src"),
            resolve(rootDir, "test"),
          ],
          loader: "babel-loader",
          options: {
            /**
             * These are additional options for `babel-loader` - the configuration
             * for Babel itself is automatically merged from `babel-config.js`
             * 
             * https://github.com/babel/babel-loader#options
             */
            cacheDirectory: false
          }
        },
        {
          test: /\.s?css$/,
          use: [
            ... isDev ? [
              "style-loader",
            ]: [
              require("mini-css-extract-plugin").loader,
            ],
            {
              loader: "css-loader",
              options: {
                importLoaders: 2
              }
            },
            'postcss-loader',
            'sass-loader',
          ]
        },
        {
          test: /\.(eot|ttf|woff|woff2)(\?v=\d+\.\d+\.\d+)?$/,
          use: [
            {
              loader: "file-loader",
              options: {
                name: isDev
                  ? 'assets/[name].[ext]'
                  : 'assets/[name].[hash:8].[ext]'
              }
            }
          ]
        },
        {
          test: /\.(ico|png|jpg|jpeg|gif|svg|webp)(\?v=\d+\.\d+\.\d+)?$/,
          use: [
            {
              loader: "url-loader",
              options: {
                limit: 8192,
                name: isDev
                  ? 'assets/[name].[ext]'
                  : 'assets/[name].[hash:8].[ext]'
              }
            }
          ]
        }
      ]
    },
    optimization: {
      minimize: ! isDev,
      splitChunks: {
        chunks: 'all',
        maxInitialRequests: isDev ? Infinity : 5,
        name: isDev
      },
      runtimeChunk: 'single',
      ... isDev ? {} : {
        minimizer: [
          new (require("terser-webpack-plugin"))({
            test: /\.[cm]?js(\?.*)?$/i,
            extractComments: true,
          })
        ]
      }
    },
    plugins: [
      new (require('html-webpack-plugin'))({
        template: resolve(rootDir, 'public/index.ejs'),
        appMountId: 'root',
        lang: 'en',
        meta: {
          viewport: 'width=device-width, initial-scale=1'
        },
        filename: 'index.html',
        chunks: [
          'index'
        ],
        title: 'MuffiNet'
      }),
      ... isDev ? [
        new (require("webpack").HotModuleReplacementPlugin)()
      ] : [
        new (require("mini-css-extract-plugin"))({
          filename: isDev
            ? 'assets/[name].css'
            : 'assets/[name].[contenthash:8].css'
        }),
        new (require("clean-webpack-plugin").CleanWebpackPlugin)({
          verbose: false
        }),
      ],
    ],
    entry: {
      index: [
        resolve(rootDir, 'src/index')
      ]
    }
  };

  return config;
}

function configureBabel({ isDev }) {
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

  return options;
}

function configurePostCSS({ isDev }) {
  /**
   * @type {import('postcss').AcceptedPlugin}
   */
  const options = {
    plugins: [
      "tailwindcss",
      "autoprefixer",
      ... isDev ? [] : ["cssnano"],
    ],
  };

  return options;
}

function configureJest() {
  /**
   * @link https://jestjs.io/docs/configuration
   * @type {import('@jest/types').Config.InitialOptions}
   */
  const config = {
    rootDir,
    testRegex: ".*test.(t|j)sx?$",
    testEnvironment: "jsdom",
    collectCoverageFrom: [
      "src\\**\\*.{mjs,jsx,js}",
      "./**/*.{js,ts,tsx}",
      "!./**/*.d.ts",
    ],
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
      "\\.(js|jsx|mjs|ts|tsx)$": "babel-jest", // Compile ts and tsx files with babel (regular js files)
    },
    moduleDirectories: ["node_modules"],
    moduleFileExtensions: ["web.jsx", "web.js","wasm", "jsx", "js", "json", "tsx", "ts", "scss"],
    moduleNameMapper: {
      "~/(.*)$": "<rootDir>/src/$1",
      "\\.svg": "<rootDir>/__mocks__/svgr-webpack.js", // TODO fix conflict with file mock below? not sure which one is being used. is the string in the mocked file important? if not, we can probably just delete this line
      "\\.(jpg|jpeg|png|gif|eot|otf|webp|svg|ttf|woff|woff2|mp4|webm|wav|mp3|m4a|aac|oga)$": "<rootDir>/__mocks__/file.js",
      "\\.(css|less|sass|scss)$": "<rootDir>/__mocks__/style.js",
      "^~$": "<rootDir>/src",
    },
    bail: true,
    verbose: false,
    setupFiles: ["./src/setupTests.ts"],
    setupFilesAfterEnv: ["./src/setupTestsAfterEnv.ts"],
  };

  return config;
}

module.exports = { configureWebpack, configureBabel, configurePostCSS, configureJest };
