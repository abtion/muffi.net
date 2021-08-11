import React from "react"
import { render as tlRender } from "@testing-library/react"
import AudioTrack from "."

function getAudioTrack() {
  return {
    on: jest.fn(),
    attach: jest
      .fn()
      .mockImplementationOnce(() => document.createElement("audio")),
    off: jest.fn(),
    kind: "Audio",
    name: "Name",
  }
}

function render(track) {
  const context = tlRender(<AudioTrack track={track} />)
  return { ...context }
}

describe(AudioTrack, () => {
  it("renders an AudioTrack", () => {
    //Arrange
    const track = getAudioTrack()
    const {} = render(track)
    //Act
    //Assert
    expect(document.querySelector("audio")).toBeInTheDocument()
  })
  describe("when track is null", () => {
    it("renders NO AudioTrack", () => {
      //Arrange
      const track = null
      const {} = render(track)
      //Act
      //Assert
      expect(document.querySelector("audio")).not.toBeInTheDocument()
    })
  })
})
