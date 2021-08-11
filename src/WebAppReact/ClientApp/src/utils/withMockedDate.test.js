import withMockedDate from "./withMockedDate"

describe(withMockedDate, () => {
  it("takes a date argument and executes a callback", () => {
    const callback = jest.fn()

    withMockedDate(new Date(), callback)
    expect(callback).toHaveBeenCalled()
  })

  describe("inside the callback", () => {
    it("stubs `new Date()` to the provided date", () => {
      const fakeDate = new Date("1999-01-03:12:30:00")

      expect(new Date()).not.toEqual(fakeDate)

      withMockedDate(fakeDate, () => {
        expect(new Date()).toEqual(fakeDate)
      })
    })

    it("stubs `Date.now()` to the provided date's milliseconds", () => {
      const fakeDate = new Date("1999-01-03:12:30:00")

      expect(new Date()).not.toEqual(fakeDate)

      withMockedDate(fakeDate, () => {
        expect(Date.now()).toEqual(fakeDate.getTime())
      })
    })

    it('passes the "date" as a named parameter', () => {
      const fakeDate = new Date("1999-01-03:12:30:00")

      withMockedDate(fakeDate, ({ date }) => {
        expect(date).toEqual(fakeDate)
      })
    })

    it('passes a "updateDate" method for replacing the faked date', () => {
      withMockedDate(new Date(), ({ updateDate }) => {
        const newFakeDate = new Date("1999-01-02:12:30:00")

        expect(new Date()).not.toEqual(newFakeDate)

        updateDate(newFakeDate)

        expect(new Date()).toEqual(newFakeDate)
      })
    })

    it("works with async callbacks", async () => {
      await withMockedDate(new Date(), async ({ date }) => {
        await new Promise((resolve) => setTimeout(resolve))
        expect(new Date()).toEqual(date)
      })
    })
  })
})
