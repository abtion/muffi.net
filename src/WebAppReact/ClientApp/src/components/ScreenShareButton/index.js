import React, { useState, useCallback, useRef } from "react"
import Video from "twilio-video"
import Button from "~/components/Button"

export default function ScreenShareButton({ room }) {
  const [isScreenSharing, setIsScreenSharing] = useState(false)
  const screenTrack = useRef()

  const stopScreenShareHandler = useCallback(() => {
    // It is important that `stop` is called before `unpublishTrack`,
    // since there is no "unpublishTrack" event that we can listen for
    screenTrack.current.stop()
    room.localParticipant.unpublishTrack(screenTrack.current)
    screenTrack.current = null
    setIsScreenSharing(false)
  }, [room])

  const shareScreenHandler = useCallback(() => {
    navigator.mediaDevices.getDisplayMedia().then((stream) => {
      screenTrack.current = new Video.LocalVideoTrack(stream.getTracks()[0], {
        name: "sharedScreen",
      })
      room.localParticipant.publishTrack(screenTrack.current)

      screenTrack.current.mediaStreamTrack.onended = () => {
        stopScreenShareHandler()
      }
      setIsScreenSharing(true)
    })
  }, [room, stopScreenShareHandler])

  return isScreenSharing ? (
    <Button size="sm" outline color="primary" onClick={stopScreenShareHandler}>
      Stop sharing
    </Button>
  ) : (
    <Button size="sm" color="primary" onClick={shareScreenHandler}>
      Share screen
    </Button>
  )
}
