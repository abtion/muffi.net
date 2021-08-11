import { useEffect } from "react"

export default (callback, delay, dependencies) => {
  if (!delay || !Number.isInteger(delay))
    throw Error("Delay must be an integer")

  useEffect(() => {
    callback()
    const interval = setInterval(callback, delay)

    return () => clearInterval(interval)
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, dependencies)
}
