const process = require("process");
const { resolve } = require("path");
const { readFileSync } = require("fs");

const rootDir = __dirname;

const certificateDir = process.env.APPDATA
  ? `${process.env.APPDATA}/ASP.NET/https`
  : `${process.env.HOME}/.aspnet/https`;

const host = "localhost";
const port = 44437;

module.exports = (env, { mode }) => {
  require("./mode.js").mode = mode; // made available to other configuration files

  const isDev = mode === "development";

  /**
   * @link https://webpack.js.org/configuration/#options
   * @type {import('webpack').Configuration}
   */
  const config = {
    mode,
    devtool: isDev
      ? "eval-cheap-module-source-map"
      : undefined, // TODO use "source-map" in production build? the Ruby vesion has it, but it didn't immediately work, so more configuration and testing needed here
    target: 'web',
    context: rootDir,
    stats: {
      children: false,
      entrypoints: false,
      modules: false
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
      extensions: ['.web.jsx', '.web.js', '.wasm', '.mjs', '.jsx', '.js', '.json', '.tsx', '.ts'],
      fallback: {
        fs: false,
        tls: false,
      }
    },
    devServer: {
      host,
      port,
      hot: true,
      historyApiFallback: true,
      client: {
        overlay: true,
      },
      devMiddleware: {
        stats: {
          all: false,
          errors: true,
          timings: true,
          warnings: true,
        },
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
      allowedHosts: "all",
    },
    module: {
      rules: [
        {
          test: /\.svg$/,
          issuer: /\.[tj]sx?$/, // apply only when svg is imported from jsx/tsx files,
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
                importLoaders: 2,
                sourceMap: isDev, // not enabled by default (despite the devtool setting) - it's slower, but CSS sourcemap doesn't work without it
              }
            },
            'postcss-loader',
            'sass-loader',
          ]
        },
        {
          test: /\.(eot|ttf|woff|woff2)(\?v=\d+\.\d+\.\d+)?$/,
          type: "asset/resource",
          generator: {
            filename: isDev
              ? 'assets/[name].[ext]'
              : 'assets/[name].[hash:8].[ext]'
          }
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
      ... isDev ? [] : [
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
