import { useEffect } from "react"

export default (
  callback: () => void,
  delay: number,
  dependencies?: React.DependencyList
): void => {
  useEffect(() => {
    callback()
    const interval = setInterval(callback, delay)

    return () => clearInterval(interval)
  }, dependencies)
}
