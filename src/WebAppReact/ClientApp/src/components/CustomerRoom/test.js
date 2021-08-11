import React from "react"
import { render as tlRender } from "@testing-library/react"
import CustomerRoom from "./"
import useRoom from "~/hooks/useRoom"
import VideoTrack from "~/components/VideoTrack"
import AudioTrack from "~/components/AudioTrack"
import TrackToggleButton from "~/components/TrackToggleButton"
import Button from "~/components/Button"

jest.mock("~/hooks/useRoom")
jest.mock("~/components/VideoTrack", () => jest.fn(() => <div>VideoTrack</div>))
jest.mock("~/components/AudioTrack", () => jest.fn(() => <div>AudioTrack</div>))
jest.mock("~/components/ChatHistory", () => () => <div>ChatHistory</div>)
jest.mock("~/components/Button", () => jest.fn(() => <div>Button</div>))
jest.mock("~/components/TracksDisabledIndicator", () => () => (
  <div>TracksDisabledIndicator</div>
))
jest.mock("~/components/TrackToggleButton", () =>
  jest.fn(() => <div>TrackToggleButton</div>)
)
jest.mock("~/components/Spinner", () => jest.fn(() => <div>Spinner</div>))
jest.mock("~/components/ChatArea", () => jest.fn(() => <div>ChatArea</div>))

const technicianVideoTrack = {
  kind: "video",
  name: "technicianVideo",
}
const CustomerVideoTrack = {
  kind: "video",
  name: "customerVideo",
}
const sharedScreenTrack = {
  kind: "video",
  name: "sharedScreen",
}

const technicianAudioTrack = {
  kind: "audio",
  name: "technicianAudio",
}
const CustomerAudioTrack = {
  kind: "audio",
  name: "customerAudio",
}

const tracksByName = {
  sharedScreen: { sharedScreenTrack },
  technicianVideo: { technicianVideoTrack },
  customerVideo: { CustomerVideoTrack },
  customerAudio: { CustomerAudioTrack },
  technicianAudio: { technicianAudioTrack },
}

function render() {
  const context = tlRender(<CustomerRoom />)

  return { ...context }
}

afterEach(() => {
  useRoom.mockReturnValue([])
  VideoTrack.mockClear()
  AudioTrack.mockClear()
  TrackToggleButton.mockClear()
  Button.mockClear()
})

describe(CustomerRoom, () => {
  describe("when room couldn't be found and we were not able to connect ", () => {
    it("renders an error message", async () => {
      const connectionError = true
      useRoom.mockReturnValue([null, connectionError, null])
      const { findByText } = render()
      expect(
        await findByText("Vi kunne ikke forbinde til dette kald")
      ).toBeInTheDocument()
    })
  })
  describe("when room is not yet found but no error has happened", () => {
    it("renders a spinner", async () => {
      const connectionError = false
      useRoom.mockReturnValue([null, connectionError, null])
      const { findByText } = render()
      expect(await findByText("Spinner")).toBeInTheDocument()
    })
  })
  describe("when room is found", () => {
    beforeEach(() => {
      const room = {}
      const connectionError = false

      useRoom.mockReturnValue([room, connectionError, tracksByName])
    })
    it("renders 3 videoTracks with correct parameters", async () => {
      const {} = render()

      expect(VideoTrack).toHaveBeenCalledTimes(3)
      expect(VideoTrack).toHaveBeenCalledWith(
        expect.objectContaining({
          track: tracksByName.customerVideo,
          mirror: true,
          className: "CustomerRoom__video-track",
        }),
        expect.anything()
      )
      expect(VideoTrack).toHaveBeenCalledWith(
        expect.objectContaining({
          track: tracksByName.technicianVideo,
          className: "CustomerRoom__video-track",
        }),
        expect.anything()
      )
      expect(VideoTrack).toHaveBeenCalledWith(
        expect.objectContaining({
          track: tracksByName.sharedScreen,
          contain: true,
          className: "CustomerRoom__video-track",
        }),
        expect.anything()
      )
    })

    it("renders 1 audioTrack with correct parameters", async () => {
      const {} = render()
      expect(AudioTrack).toHaveBeenCalledTimes(1)
      expect(AudioTrack).toHaveBeenCalledWith(
        expect.objectContaining({ track: tracksByName.technicianAudio }),
        expect.anything()
      )
    })

    it("renders 2 TrackToggleButtons with correct parameters", async () => {
      const {} = render()

      expect(TrackToggleButton).toHaveBeenCalledTimes(2)
      expect(TrackToggleButton).toHaveBeenCalledWith(
        expect.objectContaining({
          room: {},
          track: tracksByName.customerAudio,
        }),
        expect.anything()
      )
      expect(TrackToggleButton).toHaveBeenCalledWith(
        expect.objectContaining({
          room: {},
          track: tracksByName.customerVideo,
        }),
        expect.anything()
      )
    })

    it("renders chat button", async () => {
      const {} = render()

      expect(Button).toHaveBeenCalledWith(
        expect.objectContaining({
          size: "sm",
          color: "success",
          className: "CustomerRoom__chat-enable",
        }),
        expect.anything()
      )
    })

    // Test chatArea correct params
    // Test text
    // Test isEnabled?
  })
})
