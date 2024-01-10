import useInterval from "./useInterval"
import { render } from "~/utils/test-utils"

describe(useInterval, () => {
  it("calls the callback immediately", () => {
    const callback = vi.fn()

    const Component = () => {
      useInterval(callback, 1000)
      return null
    }

    render(<Component />)

    expect(callback).toHaveBeenCalled()
  })

  it("calls the callback every nth ms specified by delay", () => {
    vi.useFakeTimers()

    const callback = vi.fn()
    const delay = 1000

    const Component = () => {
      useInterval(callback, delay)
      return null
    }

    render(<Component />)
    callback.mockClear() // Don't count immediate call

    vi.advanceTimersByTime(delay * 10)

    expect(callback).toHaveBeenCalledTimes(10)

    vi.clearAllTimers()
    vi.useRealTimers()
  })

  it("cleans up the timer when the component is unmounted", () => {
    vi.useFakeTimers()

    const callback = vi.fn()
    const delay = 1000

    const Component = () => {
      useInterval(callback, delay)
      return null
    }

    const { unmount } = render(<Component />)
    callback.mockClear() // Don't count immediate call

    unmount()

    vi.advanceTimersByTime(delay * 10)

    expect(callback).not.toHaveBeenCalled()

    vi.clearAllTimers()
    vi.useRealTimers()
  })

  describe("dependencies", () => {
    it("keeps running when a non-dependent prop is changed", () => {
      vi.useFakeTimers()

      const callback = vi.fn()
      const delay = 1000

      const Component = ({
        dependency,
      }: {
        dependency: string
        nonDependency: string
      }) => {
        useInterval(callback, delay, [dependency])
        return null
      }

      // Initial render
      const { rerender } = render(
        <Component dependency="untouched" nonDependency="before" />,
      )

      expect(callback).toHaveBeenCalledTimes(1)
      callback.mockClear()

      vi.advanceTimersByTime(delay * 0.5)

      // Non-dependency prop change
      rerender(<Component dependency="untouched" nonDependency="after" />)
      expect(callback).not.toHaveBeenCalled()

      vi.advanceTimersByTime(delay * 0.5)

      expect(callback).toHaveBeenCalledTimes(1)

      // Cleanup
      vi.clearAllTimers()
      vi.useRealTimers()
    })

    it("cleans up and reevaluates when dependencies are changed", () => {
      vi.useFakeTimers()

      const callback = vi.fn()
      const delay = 1000

      const Component = (props: { dependency: string }) => {
        const { dependency } = props

        useInterval(
          () => callback(dependency), // Using an arrow function lets us check the callback argument
          delay,
          [dependency],
        )
        return null
      }

      // Initial render
      const { rerender } = render(<Component dependency="before" />)

      expect(callback).toHaveBeenCalledWith("before")
      expect(callback).toHaveBeenCalledTimes(1)
      callback.mockClear()

      vi.advanceTimersByTime(delay)

      expect(callback).toHaveBeenCalledWith("before")
      expect(callback).toHaveBeenCalledTimes(1)
      callback.mockClear()

      // Dependency change
      rerender(<Component dependency="after" />)

      expect(callback).toHaveBeenCalledWith("after")
      expect(callback).toHaveBeenCalledTimes(1)
      callback.mockClear()

      vi.advanceTimersByTime(delay)

      expect(callback).toHaveBeenCalledWith("after")
      expect(callback).toHaveBeenCalledTimes(1)

      // Cleanup
      vi.clearAllTimers()
      vi.useRealTimers()
    })
  })
})
