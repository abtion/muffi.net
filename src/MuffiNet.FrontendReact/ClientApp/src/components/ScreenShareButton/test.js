import React from "react"
import { act, render as tlRender } from "@testing-library/react"
import userEvent from "@testing-library/user-event"

import ScreenShareButton from "./"

const mediaStreamTrack = {}

jest.mock("twilio-video", () => {
  return {
    LocalVideoTrack: jest.fn(() => {
      return {
        mediaStreamTrack,
        stop: jest.fn(),
      }
    }),
  }
})

beforeEach(() => {
  const stream = { getTracks: jest.fn().mockImplementationOnce(() => ["", ""]) }

  const mediaDevicesMock = {
    getDisplayMedia: jest
      .fn()
      .mockImplementationOnce(() => Promise.resolve(stream)),
  }
  global.navigator.mediaDevices = mediaDevicesMock
})

const room = {
  localParticipant: {
    publishTrack: jest.fn(),
    unpublishTrack: jest.fn(),
  },
}

function render() {
  const context = tlRender(<ScreenShareButton room={room} />)
  return { ...context }
}

describe(ScreenShareButton, () => {
  it("renders button", () => {
    //Arrange
    const { getByRole } = render()
    const button = getByRole("button")

    //Assert
    expect(button).toBeInTheDocument()
  })

  it("renders button with text 'Share screen' ", () => {
    //Arrange
    const { getByRole } = render()
    const button = getByRole("button")

    //Assert
    expect(button).toHaveTextContent("Share screen")
  })

  describe("when the 'Share screen' button is clicked", () => {
    it("changes text to 'Stop sharing' (screen share is activated)", async () => {
      //Arrange
      const { getByRole, findByText } = render()
      const button = getByRole("button")

      //Act
      userEvent.click(button)

      //Assert
      expect(await findByText("Stop sharing")).toBeInTheDocument()
    })
  })

  describe("when the 'Stop sharing' button is clicked", () => {
    it("changes text to 'Share screen' (screen share is deactivated)", async () => {
      //Arrange
      const { getByRole, findByText } = render()
      const button = getByRole("button")

      //Act
      userEvent.click(button)
      userEvent.click(await findByText("Stop sharing"))

      //Assert
      expect(await findByText("Share screen")).toBeInTheDocument()
    })
  })

  describe("when mediaStreamTrack.onended is triggered", () => {
    it("stops the screen share and show the 'Share screen' button", async () => {
      //Arrange
      const { getByRole, findByText } = render()
      const button = getByRole("button")

      //Act
      userEvent.click(button)

      await findByText("Stop sharing")

      act(() => {
        mediaStreamTrack.onended()
      })

      //Assert
      expect(await findByText("Share screen")).toBeInTheDocument()
    })
  })
})
