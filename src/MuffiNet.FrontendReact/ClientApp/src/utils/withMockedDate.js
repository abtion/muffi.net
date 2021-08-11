export default (date, callback) => {
  // Mock Date
  const realDate = Date

  // eslint-disable-next-line no-native-reassign
  Date = class extends Date {
    constructor(...args) {
      if (args.length || !date) return super(...args)

      return new Date(date)
    }

    static now() {
      return date * 1
    }
  }

  // Run callback
  const updateDate = (newDate) => (date = newDate)
  const callbackResult = callback({ date, updateDate })

  // Reset differently depending on whether or not the callback was async
  // eslint-disable-next-line no-native-reassign
  const resetDateClass = () => (Date = realDate)

  const callbackIsPromise =
    callbackResult && typeof callbackResult.then === "function"
  if (callbackIsPromise) {
    return callbackResult.then(resetDateClass)
  } else {
    resetDateClass()
  }
}
