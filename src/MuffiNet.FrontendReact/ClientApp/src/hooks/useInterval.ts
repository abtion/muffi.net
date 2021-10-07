import { useEffect } from "react"

export default (
  callback: () => void,
  delay: number,
  dependencies?: React.DependencyList
) => {
  useEffect(() => {
    callback()
    const interval = setInterval(callback, delay)

    return () => clearInterval(interval)
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, dependencies)
}
