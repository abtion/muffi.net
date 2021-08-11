import React from "react"
import { render as tlRender } from "@testing-library/react"
import CallPreview from "."

jest.mock("~/components/VideoTrack", () => () => <div>Video Track</div>)

function render() {
  const context = tlRender(<CallPreview />)
  return { ...context }
}

jest.mock("twilio-video", () => {
  const track = {
    // on: jest.fn(),
    // attach: jest.fn(),
    // off: jest.fn()
  }
  return {
    createLocalVideoTrack: jest
      .fn()
      .mockImplementation(() => Promise.resolve(track)),
    createLocalAudioTrack: jest
      .fn()
      .mockImplementation(() => Promise.resolve(track)),
  }
})

describe(CallPreview, () => {
  it("renders the CallPreview", async () => {
    //Arrange
    const { findByText } = render()
    //Assert
    expect(await findByText("Video Track")).toBeInTheDocument()
  })
})
