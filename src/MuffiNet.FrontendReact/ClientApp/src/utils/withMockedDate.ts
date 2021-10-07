interface WithMockedDateCallbackOptions {
  date: string | number | Date
  updateDate: (date: string | number | Date) => void
}
interface WithMockedDateCallback {
  (options: WithMockedDateCallbackOptions): void | Promise<void>
}

export default (
  date: string | number | Date,
  callback: WithMockedDateCallback
): void | Promise<void> => {
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
  // eslint-disable-next-line no-global-assign
  Date = MockedDate

  // Run callback
  const updateDate = (newDate: string | number) => {
    date = newDate
  }
  const callbackResult = callback({ date, updateDate })

  // Reset differently depending on whether or not the callback was async
  const resetDateClass = () => {
    // eslint-disable-next-line no-global-assign
    Date = realDate
  }

  const callbackIsPromise =
    callbackResult && typeof callbackResult.then === "function"

  if (callbackIsPromise) {
    return callbackResult.then(resetDateClass)
  } else {
    resetDateClass()
  }
}
