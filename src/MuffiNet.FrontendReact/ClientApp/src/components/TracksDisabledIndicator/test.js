import React from "react"
import { render as tlRender } from "@testing-library/react"

import TracksDisabledIndicator from "./"

const mockedAudioTrack = {
  isEnabled: null,
  on: jest.fn(),
  off: jest.fn(),
}
const mockedVideoTrack = {
  isEnabled: null,
  on: jest.fn(),
  off: jest.fn(),
}

function render(audioEnabled, videoEnabled) {
  mockedAudioTrack.isEnabled = audioEnabled
  mockedVideoTrack.isEnabled = videoEnabled

  const context = tlRender(
    <TracksDisabledIndicator
      audioTrack={mockedAudioTrack}
      videoTrack={mockedVideoTrack}
    ></TracksDisabledIndicator>
  )

  return { ...context }
}
describe(TracksDisabledIndicator, () => {
  it("renders both icons", () => {
    const { getAllByText } = render(false, false)

    const disabledIcons = getAllByText("Mocked SVG Icon")
    expect(disabledIcons).toHaveLength(2)
    expect(disabledIcons[0]).toHaveClass(
      "TracksDisabledIndicator__mic-muted-indicator"
    )
    expect(disabledIcons[1]).toHaveClass(
      "TracksDisabledIndicator__camera-off-indicator"
    )
  })

  it("renders no icons", () => {
    const { queryByText } = render(true, true)

    const missingIcons = queryByText("Mocked SVG Icon")
    expect(missingIcons).not.toBeInTheDocument()
  })

  it("renders mute icon", () => {
    const { getByText } = render(false, true)

    const muteIcon = getByText("Mocked SVG Icon")
    expect(muteIcon).toHaveClass("TracksDisabledIndicator__mic-muted-indicator")
  })

  it("renders videoOff icon", () => {
    const { getByText } = render(true, false)

    const videoOffIcon = getByText("Mocked SVG Icon")
    expect(videoOffIcon).toHaveClass(
      "TracksDisabledIndicator__camera-off-indicator"
    )
  })
})
