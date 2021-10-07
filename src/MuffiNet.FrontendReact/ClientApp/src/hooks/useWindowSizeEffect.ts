import { useEffect } from "react"

export default function useWindowSizeEffect(callback: () => void) {
  useEffect(() => {
    callback()
    window.addEventListener("resize", callback)

    return () => window.removeEventListener("resize", callback)
  }, [callback])
}
