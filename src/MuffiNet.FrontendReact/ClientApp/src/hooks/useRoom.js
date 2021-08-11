import { useState, useEffect, useCallback } from "react"
import { connect, LocalDataTrack } from "twilio-video"

import useMappedRecords from "./useMappedRecords"

const upsertEvents = [
  "trackEnabled",
  // "trackMessage",
  // "trackPublished",
  "trackStarted",
  "trackSubscribed",
  "trackSwitchedOn",
  "trackDisabled",
]

const deleteEvents = [
  "trackSwitchedOff",
  "trackUnsubscribed",
  // "trackUnpublished",
]

export default function useRoom(accessToken, participantName) {
  const [room, setRoom] = useState(null)
  const [connectionError, setConnectionError] = useState(false)
  const [tracksByName, upsertTrack, deleteTrack] = useMappedRecords(
    (track) => track.name
  )

  useEffect(() => {
    connect(accessToken, {
      audio: { name: `${participantName}Audio` },
      video: { name: `${participantName}Video` },
    })
      .then((room) => {
        setRoom(room)

        // Add local tracks
        const localTrackPublications = room.localParticipant.tracks.values()
        const localTracks = [...localTrackPublications].map(
          (publication) => publication.track
        )
        upsertTrack(localTracks)

        // Catch local screen share
        room.localParticipant.on("trackPublished", ({ track }) =>
          upsertTrack(track)
        )
        room.localParticipant.on("trackStopped", deleteTrack)

        // Publish local data track (we could not get this to directly in the connect call)
        room.localParticipant.publishTrack(
          new LocalDataTrack({ name: `${participantName}Data` })
        )

        upsertEvents.forEach((eventName) => {
          room.on(eventName, (track) => {
            upsertTrack(track)
          })
        })

        deleteEvents.forEach((eventName) => {
          room.on(eventName, (track) => {
            deleteTrack(track)
          })
        })
      })
      .catch((error) => {
        console.log("Not able to connect to room", error.message)
        setConnectionError(true)
      })
  }, [accessToken, participantName, upsertTrack, deleteTrack])

  const leaveRoom = useCallback(() => {
    room && room.disconnect()
  }, [room])
  const handleBeforeUnload = useCallback((event) => {
    event.preventDefault()
    event.returnValue = ""
  }, [])
  useEffect(() => {
    window.addEventListener("beforeunload", handleBeforeUnload)

    window.addEventListener("unload", leaveRoom)

    return () => {
      window.removeEventListener("beforeunload", handleBeforeUnload)
      window.removeEventListener("unload", leaveRoom)
      leaveRoom()
    }
  }, [room, leaveRoom, handleBeforeUnload])

  return [room, connectionError, tracksByName, upsertTrack, deleteTrack]
}
