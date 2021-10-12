type MockedDateDate = string | number | Date

interface UpdateDate {
  (date: MockedDateDate): void
}

interface WithMockedDateCallbackOptions {
  date: MockedDateDate
  updateDate: UpdateDate
}
interface WithMockedDateCallback {
  (options: WithMockedDateCallbackOptions): void | Promise<void>
}

export default (
  date: MockedDateDate,
  callback: WithMockedDateCallback
): void | Promise<void> => {
  // Mock Date
  const realDate = Date

  new Date()

  // eslint-disable-next-line no-native-reassign
  class MockedDate extends Date {
    constructor(...args: Parameters<typeof Date>) {
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
  const updateDate = (newDate: MockedDateDate) => {
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
