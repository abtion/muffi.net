import React, { useCallback } from "react"
import axios from "axios"
import { useHistory } from "react-router-dom"

import useRoom from "~/hooks/useRoom"
import VideoTrack from "~/components/VideoTrack"
import AudioTrack from "~/components/AudioTrack"
import Button from "~/components/Button"
import TracksDisabledIndicator from "~/components/TracksDisabledIndicator"
import CustomerInfo from "../CustomerInfo"
import ScreenShareButton from "../ScreenShareButton"
import TrackToggleButton from "../TrackToggleButton"
import ChatArea from "../ChatArea"
import ChatHeader from "../ChatHeader"
import ChatHistory from "../ChatHistory"
import ChatInput from "../ChatInput"
import getFirstName from "~/utils/getFirstName"

import { ReactComponent as IconMicMute } from "bootstrap-icons/icons/mic-mute.svg"
import { ReactComponent as IconMic } from "bootstrap-icons/icons/mic.svg"
import { ReactComponent as IconCameraVideo } from "bootstrap-icons/icons/camera-video.svg"
import { ReactComponent as IconCameraVideoOff } from "bootstrap-icons/icons/camera-video-off.svg"

import "./style.scss"

export default function TechRoom({
  accessToken,
  twilioAccessToken,
  supportTicket,
}) {
  const [room, connectionError, tracksByName] = useRoom(
    twilioAccessToken,
    "technician"
  )

  const history = useHistory()
  const leaveRoomHandler = useCallback(() => {
    if (window.confirm("Er du sikker på at du vil afslutte kaldet?")) {
      axios
        .post(
          "/api/technician/completeroom",
          { SupportTicketId: supportTicket.supportTicketId },
          {
            headers: {
              authorization: `Bearer ${accessToken}`,
            },
          }
        )
        .then(() => {
          history.push(`/technician`)
        })
        .catch((error) => {
          //This error handling asumes the error commes from closing the twilio room.
          //Should probably be handled differently when better error messages is created in the BE.
          console.log("Leave Room", error)
          alert(
            "Vi havde problemer med at finde Twilio rummet. \n\nSupport ticket lukkes."
          )
          history.push(`/technician`)
        })
    }
  }, [accessToken, history, supportTicket])

  if (connectionError)
    return (
      <>
        <div>Vi kunne ikke forbinde til dette kald</div>
        <Button id="leaveRoom" onClick={leaveRoomHandler} color="danger">
          Luk support ticket
        </Button>
      </>
    )
  if (!room) return <div>Loading...</div>

  return (
    <div className="TechRoom">
      <div className="TechRoom__video-tracks">
        <AudioTrack track={tracksByName.customerAudio} />

        <div className="TechRoom__technician">
          <VideoTrack
            className="TechRoom__video-track"
            mirror
            track={tracksByName.technicianVideo}
          >
            <TracksDisabledIndicator
              audioTrack={tracksByName.technicianAudio}
              videoTrack={tracksByName.technicianVideo}
            />
          </VideoTrack>
          <div className="TechRoom__video-track-actions">
            <TrackToggleButton room={room} track={tracksByName.technicianAudio}>
              {(isEnabled) => {
                return isEnabled ? <IconMic /> : <IconMicMute />
              }}
            </TrackToggleButton>
            <TrackToggleButton room={room} track={tracksByName.technicianVideo}>
              {(isEnabled) => {
                return isEnabled ? <IconCameraVideo /> : <IconCameraVideoOff />
              }}
            </TrackToggleButton>
          </div>
        </div>
        <div className="TechRoom__shared-screen">
          <VideoTrack
            className="TechRoom__video-track"
            contain
            track={tracksByName.sharedScreen}
          />
          <div className="TechRoom__video-track-actions">
            <ScreenShareButton room={room} />
            <Button
              size="sm"
              id="leaveRoom"
              onClick={leaveRoomHandler}
              color="danger"
            >
              Leave room
            </Button>
          </div>
        </div>

        <div className="TechRoom__customer">
          <VideoTrack
            className="TechRoom__video-track"
            track={tracksByName.customerVideo}
          >
            <TracksDisabledIndicator
              audioTrack={tracksByName.customerAudio}
              videoTrack={tracksByName.customerVideo}
            />
          </VideoTrack>
        </div>
      </div>

      <div className="TechRoom__chat-area">
        <ChatArea remoteTrack={tracksByName.customerData}>
          <ChatHeader>
            <CustomerInfo
              supportTicket={supportTicket}
              accessToken={accessToken}
            />
          </ChatHeader>
          <ChatHistory
            localTrack={tracksByName.technicianData}
            remoteName={getFirstName(supportTicket.customerName)}
          />
          <ChatInput track={tracksByName.technicianData} />
        </ChatArea>
      </div>
    </div>
  )
}
