import React, { useCallback } from "react"

import Button from "~/components/Button"
import useTrackIsEnabled from "~/hooks/useTrackIsEnabled"

export default function TrackToggleButton({ track, children }) {
  const isEnabled = useTrackIsEnabled(track)

  const clickHandler = useCallback(() => {
    if (isEnabled) {
      track.disable()
    } else {
      track.enable()
    }
  }, [isEnabled, track])

  return (
    <Button
      size="sm"
      color={isEnabled ? "primary" : "danger"}
      onClick={clickHandler}
    >
      {children(isEnabled)}
    </Button>
  )
}
