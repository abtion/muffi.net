import React, { useEffect, useState } from "react"
import { createLocalAudioTrack, createLocalVideoTrack } from "twilio-video"
import VideoTrack from "~/components/VideoTrack"
import "./style.css"

export default function CallPreview() {
  const [cameraPreviewTrack, setCameraPreviewTrack] = useState()
  useEffect(() => {
    createLocalVideoTrack().then((track) => {
      setCameraPreviewTrack(track)
    })

    // Force microphone prompt
    createLocalAudioTrack()
  }, [])

  return (
    <div>
      <VideoTrack
        className="CallPreview__video-track"
        mirror
        track={cameraPreviewTrack}
      />
    </div>
  )
}
