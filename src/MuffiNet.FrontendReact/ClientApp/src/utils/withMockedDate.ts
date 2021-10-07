export default (date: string | number | Date, callback: Function) => {
  // Mock Date
  const realDate = Date

  new Date()

  // eslint-disable-next-line no-native-reassign
  class MockedDate extends Date {
    constructor(...args: [(string | number | Date)?]) {
      if (args.length || !date) {
        super(...args)
      } else {
        super(date)
      }
    }

    static now() {
      return new realDate(date).getTime()
    }
  }

  // @ts-expect-error: We are overriding Date class on purpose
  Date = MockedDate

  // Run callback
  const updateDate = (newDate: string | number) => (date = newDate)
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
