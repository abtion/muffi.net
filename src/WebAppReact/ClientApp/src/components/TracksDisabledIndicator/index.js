import React from "react"
import { ReactComponent as IconMicMute } from "bootstrap-icons/icons/mic-mute.svg"
import { ReactComponent as IconCameraVideoOff } from "bootstrap-icons/icons/camera-video-off.svg"
import useTrackIsEnabled from "~/hooks/useTrackIsEnabled"

import "./style.scss"

export default function TracksDisabledIndicator({ audioTrack, videoTrack }) {
  const audioTrackIsEnabled = useTrackIsEnabled(audioTrack)
  const videoTrackIsEnabled = useTrackIsEnabled(videoTrack)

  return (
    <>
      {!audioTrackIsEnabled ? (
        <div className="TracksDisabledIndicator__mic-muted-indicator">
          <IconMicMute />
        </div>
      ) : null}
      {!videoTrackIsEnabled ? (
        <div className="TracksDisabledIndicator__camera-off-indicator">
          <IconCameraVideoOff />
        </div>
      ) : null}
    </>
  )
}
