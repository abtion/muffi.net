import { act, renderHook } from "@testing-library/react"
import useWindowSizeEffect from "./useWindowSizeEffect"

const callback = vi.fn()

describe(useWindowSizeEffect, () => {
  it("calls callback method on resize", () => {
    renderHook(() => useWindowSizeEffect(callback))

    expect(callback).toHaveBeenCalledTimes(1)
    act(() => {
      global.dispatchEvent(new Event("resize"))
    })
    expect(callback).toHaveBeenCalledTimes(2)
  })
})
