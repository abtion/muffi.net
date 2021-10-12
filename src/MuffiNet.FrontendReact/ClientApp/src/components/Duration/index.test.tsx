import React from "react"
import { DateTime } from "luxon"
import { render, act } from "@testing-library/react"

import Duration from "."
import withMockedDate from "~/utils/withMockedDate"

describe(Duration, () => {
  describe("since-prop is in the future", () => {
    it("displays 00:00", () => {
      const { getByText } = render(<Duration since={new Date("2222-01-21")} />)
      expect(getByText("00:00")).toBeInTheDocument()
    })
  })

  describe("since-prop is in the past", () => {
    it("displays and updates a countdown", async () => {
      await withMockedDate(new Date(), async ({ updateDate }) => {
        jest.useFakeTimers()

        const since = DateTime.now().minus({ hours: 48 }).toJSDate()

        const { getByText } = render(<Duration since={since} />)

        expect(getByText("48:00")).toBeInTheDocument()

        act(() => {
          const newTime = DateTime.now().minus({ hours: 2 }).toJSDate()
          updateDate(newTime)
          jest.runOnlyPendingTimers()
        })

        expect(getByText("46:00")).toBeInTheDocument()

        jest.clearAllTimers()
        jest.useRealTimers()
      })
    })

    describe("format", () => {
      it("allows specifying a luxon duration format", () => {
        const since = DateTime.now().minus({ hours: 2 }).toJSDate()

        const { getByText } = render(<Duration since={since} format="mmm:ss" />)
        expect(getByText("120:00")).toBeInTheDocument()
      })
    })

    describe("upperLimit", () => {
      it("sets an upper limit in ms", () => {
        const since = DateTime.now().minus({ hours: 2 }).toJSDate()
        const upperLimit = 60 * 60 * 1000

        const { getByText } = render(
          <Duration since={since} format="mm:ss" upperLimit={upperLimit} />
        )
        expect(getByText("> 60:00")).toBeInTheDocument()
      })
    })
  })
})
