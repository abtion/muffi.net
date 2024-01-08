// eslint-disable-next-line @typescript-eslint/no-var-requires
const process = require("process")

module.exports = {
  get mode() {
    return process.env.MODE || "production"
  },
  set mode(value) {
    process.env.MODE = value
  },
}
