const process = require("process");

// NOTE: because webpack (or some of it's components) fork the node.js process, environment
//       variables are literally the only way to share state between configuration files.
//       since the webpack --mode flag is only made available to the webpack configuration
//       file, environment vars are the only way to communicate this setting.

module.exports = {
  get mode() {
    return process.env.MODE || "production";
  },
  set mode(value) {
    process.env.MODE = value;
  }
}
