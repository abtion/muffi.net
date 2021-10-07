import { renderHook, act } from "@testing-library/react-hooks"
import useWindowSizeEffect from "./useWindowSizeEffect"

const callback = jest.fn()

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
