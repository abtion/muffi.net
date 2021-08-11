import React from "react"
import { render as tlRender } from "@testing-library/react"
import VideoTrack from "."

function getTrack() {
  return {
    on: jest.fn(),
    attach: jest
      .fn()
      .mockImplementationOnce(() => document.createElement("video")),
    off: jest.fn(),
    kind: "video",
    name: "Name",
  }
}

function render(className, contain, mirror, children) {
  const track = getTrack()
  const context = tlRender(
    <VideoTrack
      className={className}
      track={track}
      contain={contain}
      mirror={mirror}
      children={children}
    />
  )
  return { ...context }
}

describe(VideoTrack, () => {
  it("renders a videotrack div with the given className", async () => {
    //Arrange
    const className = "GivenClassName"
    const {} = render(className, false, false, null)
    //Act
    //Assert
    expect(document.querySelector("div.VideoTrack")).toHaveClass(className)
  })

  it("renders a mirrored videotrack when mirrored = true", async () => {
    //Arrange
    const {} = render("", false, true, null)
    //Act
    //Assert
    expect(document.querySelector("video")).toHaveClass(
      "VideoTrack__video--mirror"
    )
  })

  it("renders a non mirrored videotrack when mirrored = false", async () => {
    //Arrange
    const {} = render("", false, false, null)
    //Act
    //Assert
    expect(document.querySelector("video")).not.toHaveClass(
      "VideoTrack__video--mirror"
    )
  })

  it("renders a videotrack with 'VideoTrack__video--contain' when contain = true", async () => {
    //Arrange
    const {} = render("", true, false, null)
    //Act
    //Assert
    expect(document.querySelector("video")).toHaveClass(
      "VideoTrack__video--contain"
    )
  })
  it("renders a videotrack without 'VideoTrack__video--contain' when contain = false", async () => {
    //Arrange
    const {} = render("", false, false, null)
    //Act
    //Assert
    expect(document.querySelector("video")).not.toHaveClass(
      "VideoTrack__video--contain"
    )
  })

  it("renders a videotrack without contain or mirrored by default if not set", async () => {
    //Arrange
    const { rerender } = render("", false, false, null)
    //Act
    rerender(
      <VideoTrack className="rerendered" track={getTrack()} children={null} />
    )
    //Assert
    expect(document.querySelector("video")).not.toHaveClass(
      "VideoTrack__video--contain"
    )
    expect(document.querySelector("video")).not.toHaveClass(
      "VideoTrack__video--mirror"
    )
  })

  it("renders children", async () => {
    //Arrange
    const child = <div>child element</div>
    const { getByText } = render("", false, false, child)
    //Act
    //Assert
    expect(getByText("child element")).toBeInTheDocument()
  })

  describe("when component rerenders", () => {
    it("renders only 1 video element", async () => {
      //Arrange
      const { rerender } = render("", false, false, null)

      //Assert
      expect(document.querySelectorAll("video")).toHaveLength(1)

      //Act
      const track = getTrack()
      rerender(
        <VideoTrack
          className="rerendered"
          track={track}
          contain={false}
          mirror={false}
          children={null}
        />
      )

      //Assert
      expect(document.querySelectorAll("video")).toHaveLength(1)
    })
    it("renders 0 video element if no track is added", async () => {
      //Arrange
      const { rerender } = render("", false, false, null)

      //Assert
      expect(document.querySelectorAll("video")).toHaveLength(1)

      //Act
      const track = null
      rerender(
        <VideoTrack
          className="rerendered"
          track={track}
          contain={false}
          mirror={false}
          children={null}
        />
      )

      //Assert
      expect(document.querySelectorAll("video")).toHaveLength(0)
    })
  })
})
