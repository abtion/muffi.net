import React, { useCallback, useState } from "react"
import classNames from "classnames"

import { ReactComponent as IconMicMute } from "bootstrap-icons/icons/mic-mute.svg"
import { ReactComponent as IconMic } from "bootstrap-icons/icons/mic.svg"
import { ReactComponent as IconCameraVideo } from "bootstrap-icons/icons/camera-video.svg"
import { ReactComponent as IconCameraVideoOff } from "bootstrap-icons/icons/camera-video-off.svg"
import { ReactComponent as ChatDots } from "bootstrap-icons/icons/chat-dots.svg"

import useRoom from "~/hooks/useRoom"
import Button from "~/components/Button"
import VideoTrack from "~/components/VideoTrack"
import AudioTrack from "~/components/AudioTrack"
import TracksDisabledIndicator from "~/components/TracksDisabledIndicator"
import TrackToggleButton from "~/components/TrackToggleButton"
import ChatArea from "~/components/ChatArea"
import ChatHistory from "~/components/ChatHistory"
import ChatInput from "~/components/ChatInput"

import "./style.scss"
import ChatHeader from "../ChatHeader"
import Spinner from "../Spinner"
import useWindowSizeEffect from "~/hooks/useWindowSizeEffect"

export default function CustomerRoom({ accessToken, ossId, technicianName }) {
  const [room, connectionError, tracksByName] = useRoom(accessToken, "customer")
  const [chatShown, setChatShown] = useState(false)
  const [maxInputHeight, setMaxInputHeight] = useState(Infinity)

  const updateMaxInputHeight = useCallback(() => {
    setMaxInputHeight(window.innerHeight <= 200 ? 36 : Infinity)
  }, [setMaxInputHeight])

  useWindowSizeEffect(updateMaxInputHeight)

  if (connectionError) return <div>Vi kunne ikke forbinde til dette kald</div>
  if (!room) return <Spinner className="mx-auto mt-10" />

  const screenIsShared = Boolean(tracksByName.sharedScreen)
  const videoTracksClassNames = classNames("CustomerRoom__video-tracks", {
    "CustomerRoom__video-tracks--screen-not-shared": !screenIsShared,
    "CustomerRoom__video-tracks--screen-shared": screenIsShared,
  })
  const chatAreaClassNames = classNames("CustomerRoom__chat-area", {
    "CustomerRoom__chat-area--visible": chatShown,
  })

  return (
    <div className="CustomerRoom">
      <div className={videoTracksClassNames}>
        <AudioTrack track={tracksByName.technicianAudio} />
        <div className="CustomerRoom__technician">
          <h1 className="CustomerRoom__video-track-title">{technicianName}</h1>
          <VideoTrack
            className="CustomerRoom__video-track"
            track={tracksByName.technicianVideo}
          >
            <TracksDisabledIndicator
              audioTrack={tracksByName.technicianAudio}
              videoTrack={tracksByName.technicianVideo}
            />
          </VideoTrack>
        </div>
        <div className="CustomerRoom__customer">
          <h1 className="CustomerRoom__video-track-title">Dig</h1>
          <VideoTrack
            className="CustomerRoom__video-track"
            mirror
            track={tracksByName.customerVideo}
          >
            <TracksDisabledIndicator
              audioTrack={tracksByName.customerAudio}
              videoTrack={tracksByName.customerVideo}
            />
          </VideoTrack>
          <div className="CustomerRoom__video-track-actions">
            <TrackToggleButton room={room} track={tracksByName.customerAudio}>
              {(isEnabled) => {
                return isEnabled ? <IconMic /> : <IconMicMute />
              }}
            </TrackToggleButton>
            <TrackToggleButton room={room} track={tracksByName.customerVideo}>
              {(isEnabled) => {
                return isEnabled ? <IconCameraVideo /> : <IconCameraVideoOff />
              }}
            </TrackToggleButton>
            <Button
              size="sm"
              color="success"
              className="CustomerRoom__chat-enable"
              onClick={() => setChatShown(true)}
            >
              <ChatDots />
            </Button>
          </div>
        </div>
        <div className="CustomerRoom__shared-screen">
          <h1 className="CustomerRoom__video-track-title">Skærmdeling</h1>
          <VideoTrack
            className="CustomerRoom__video-track"
            contain
            track={tracksByName.sharedScreen}
          />
        </div>
      </div>
      <div className={chatAreaClassNames}>
        <ChatArea remoteTrack={tracksByName.technicianData}>
          <ChatHeader className="CustomerRoom__chat-actions">
            <Button color="gray" onClick={() => setChatShown(false)}>
              Skjul chat
            </Button>
          </ChatHeader>

          <div className="CustomerRoom__chat-title">
            <h1>Chat med {technicianName}</h1>
            {ossId && (
              <>
                <p>Servicenummer på din sag:</p>
                <p>{ossId}</p>
              </>
            )}
          </div>

          <ChatHistory
            localTrack={tracksByName.customerData}
            remoteName={technicianName}
          />
          <ChatInput
            track={tracksByName.customerData}
            maxHeight={maxInputHeight}
          />
        </ChatArea>
      </div>
    </div>
  )
}
