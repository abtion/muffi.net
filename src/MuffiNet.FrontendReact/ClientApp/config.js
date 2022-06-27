const process = require("process");
const { resolve } = require("path");
const { readFileSync } = require("fs");

const rootDir = __dirname;

const certificateDir = process.env.APPDATA
  ? `${process.env.APPDATA}/ASP.NET/https`
  : `${process.env.HOME}/.aspnet/https`;

// TODO remove these? no longer necessary
const host = process.env.PORT ? "0.0.0.0" : "localhost";
const port = process.env.PORT || 44437;

// TODO avoid inline require() - use package names where possible (for easier diagnostics)
// TODO add dependencies to package.json

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
      : undefined,
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
        'react-native': 'react-native-web',
        '~': resolve(rootDir, "src"),
      },
      extensions: [
        '.web.jsx',
        '.web.js',
        '.wasm',
        '.mjs',
        '.jsx',
        '.js',
        '.json',
        '.tsx',
        '.ts'
      ]
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
    // TODO refactor: remove duplicate code, clean up Neutrino's comments
    module: {
      rules: [
        /* config.module.rule('svgr') */
        {
          test: /\.svg$/, // you can pick whatever file extension you like here (TODO moved this comment from .neutrinorc.js - what does it mean? might have been just a boilerplate explainer copy/pasted from somewhere?)
          issuer: {
            test: /\.[tj]sx?$/ // optional, but lets you continue importing SVGs from CSS files etc. without it breaking
          },
          use: [
            /* config.module.rule('svgr').use('@svgr/webpack') */
            {
              loader: "@svgr\\webpack",
              options: {
                svgoConfig: {
                  plugins: {
                    removeViewBox: false // allow resizing SVGs (default options strip viewBox for some strange reason)
                  }
                }
              }
            }
          ]
        },
        /* config.module.rule('html') */
        {
          test: /\.html$/,
          use: [
            /* config.module.rule('html').use('html') */
            {
              loader: "html-loader",
              options: {
                attrs: [
                  'img:src',
                  'link:href'
                ]
              }
            }
          ]
        },
        /* config.module.rule('compile') */
        {
          test: /\.(wasm|mjs|jsx|js|tsx|ts)$/,
          include: [
            resolve(rootDir, "src"),
            resolve(rootDir, "test"),
          ],
          use: [
            /* config.module.rule('compile').use('babel') */
            {
              loader: "babel-loader",
              options: configureBabel({ isDev })
            }
          ]
        },
        /* config.module.rule('style') */
        {
          oneOf: [
            /* config.module.rule('style').oneOf('modules') */
            {
              test: /\.module\.css$/,
              use: [
                /* config.module.rule('style').oneOf('modules').use('style') */
                {
                  loader: "style-loader",
                  options: {
                    esModule: true // TODO check this: this was enabled in production mode only - not sure if that was intentional? it's enabled by default, so we can probably just delete this section. make sure!
                  }
                },
                /* config.module.rule('style').oneOf('modules').use('css') */
                {
                  loader: "css-loader",
                  options: {
                    importLoaders: 2,
                    modules: true
                  }
                },
                /* config.module.rule('style').oneOf('modules').use('css-2') */
                {
                  loader: 'postcss-loader',
                  options: {
                    postcssOptions: {
                      plugins: [require("tailwindcss"), require("autoprefixer")]
                    }
                  }
                },
                /* config.module.rule('style').oneOf('modules').use('css-3') */
                {
                  loader: 'sass-loader'
                }
              ]
            },
            /* config.module.rule('style').oneOf('normal') */
            {
              test: /\.s?css$/,
              use: [
                /* config.module.rule('style').oneOf('normal').use('style') */
                {
                  loader: "style-loader",
                  options: {
                    esModule: true // TODO check this: this was enabled in production mode only - not sure if that was intentional? it's enabled by default, so we can probably just delete this section. make sure!
                  }
                },
                /* config.module.rule('style').oneOf('normal').use('css') */
                {
                  loader: "css-loader",
                  options: {
                    importLoaders: 2
                  }
                },
                /* config.module.rule('style').oneOf('normal').use('css-2') */
                {
                  loader: 'postcss-loader',
                  options: {
                    postcssOptions: {
                      plugins: [require("tailwindcss"), require("autoprefixer")]
                    }
                  }
                },
                /* config.module.rule('style').oneOf('normal').use('css-3') */
                {
                  loader: 'sass-loader'
                }
              ]
            }
          ]
        },
        /* config.module.rule('font') */
        {
          test: /\.(eot|ttf|woff|woff2)(\?v=\d+\.\d+\.\d+)?$/,
          use: [
            /* config.module.rule('font').use('file') */
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
        /* config.module.rule('image') */
        {
          test: /\.(ico|png|jpg|jpeg|gif|svg|webp)(\?v=\d+\.\d+\.\d+)?$/,
          use: [
            /* config.module.rule('image').use('url') */
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
      runtimeChunk: 'single'
    },
    plugins: [
      /* config.plugin('html-index') */
      new (require('html-webpack-plugin'))(
        {
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
        }
      ),
      ... isDev ? [
        /* config.plugin('hot') */
        new (require("webpack").HotModuleReplacementPlugin)()
      ] : [
        /* config.plugin('extract') */
        new (require("mini-css-extract-plugin"))({
          filename: 'assets/[name].[contenthash:8].css'
        }),
        /* config.plugin('clean') */
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
    // cacheDirectory: isTest, // TODO the neutrino jest plugin specified this this, but it's not a documented option??
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
          development: true,
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
      "react-hot-loader/babel",
    ]
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
      "\\.svg": "<rootDir>/__mocks__/svgr-webpack.js",
      "\\.(jpg|jpeg|png|gif|eot|otf|webp|svg|ttf|woff|woff2|mp4|webm|wav|mp3|m4a|aac|oga)$": "<rootDir>/__mocks__/file.js",
      "\\.(css|less|sass|scss)$": "<rootDir>/__mocks__/style.js",
      "^react-native$": "<rootDir>\\node_modules\\react-native-web",
      "^~$": "<rootDir>/src",
    },
    bail: true,
    verbose: false,
    setupFiles: ["./src/setupTests.ts"],
    setupFilesAfterEnv: ["./src/setupTestsAfterEnv.ts"],
  };

  return config;
}

module.exports = { configureWebpack, configureBabel, configureJest };
