import { useState, useEffect, useCallback } from "react"

export default function useTrackIsEnabled(track) {
  const [isEnabled, setIsEnabled] = useState(null)

  const handleToggled = useCallback((track) => {
    setIsEnabled(track.isEnabled)
  }, [])

  useEffect(() => {
    if (track) {
      track.on("disabled", handleToggled)
      track.on("enabled", handleToggled)
      handleToggled(track)
    }

    return () => {
      if (track) {
        track.off("disabled", handleToggled)
        track.off("enabled", handleToggled)
      }
    }
  }, [track, handleToggled])

  return isEnabled
}
