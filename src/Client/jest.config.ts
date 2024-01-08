const rootDir = __dirname

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
  moduleFileExtensions: [
    "web.jsx",
    "web.js",
    "wasm",
    "jsx",
    "js",
    "json",
    "tsx",
    "ts",
    "scss",
  ],
  moduleNameMapper: {
    "~/(.*)$": "<rootDir>/src/$1",
    "\\.svg": "<rootDir>/__mocks__/svgr-webpack.js", // TODO fix conflict with file mock below? not sure which one is being used. is the string in the mocked file important? if not, we can probably just delete this line
    "\\.(jpg|jpeg|png|gif|eot|otf|webp|svg|ttf|woff|woff2|mp4|webm|wav|mp3|m4a|aac|oga)$":
      "<rootDir>/__mocks__/file.js",
    "\\.(css|less|sass|scss)$": "<rootDir>/__mocks__/style.js",
    "^~$": "<rootDir>/src",
  },
  bail: true,
  verbose: false,
  setupFiles: ["./src/setupTests.ts"],
  setupFilesAfterEnv: ["./src/setupTestsAfterEnv.ts"],
}

module.exports = config
