import React from "react"
import { render as tlRender } from "@testing-library/react"
import userEvent from "@testing-library/user-event"

import TrackToggleButton from "./"

let disableMock = jest.fn()
let enableMock = jest.fn()

const mockedTrack = {
  isEnabled: null,
  disable: disableMock,
  enable: enableMock,
  on: jest.fn(),
  off: jest.fn(),
}

function render(trackIsEnabled) {
  mockedTrack.isEnabled = trackIsEnabled
  const context = tlRender(
    <TrackToggleButton track={mockedTrack}>
      {(isEnabled) => {
        return isEnabled ? <p>true</p> : <p>false</p>
      }}
    </TrackToggleButton>
  )

  return { ...context }
}
describe(TrackToggleButton, () => {
  it("renders button", () => {
    const { getByRole } = render(false)

    const button = getByRole("button")
    expect(button).toBeInTheDocument()
    expect(button).toHaveClass("Button Button--sm Button--danger")
  })

  it("renders conditional child element", () => {
    const { getByText } = render(false)

    const child = getByText("false")
    expect(child).toBeInTheDocument()
  })

  it("calls track.enable() on toggle click", () => {
    const { getByRole } = render(false)

    userEvent.click(getByRole("button"))

    expect(enableMock).toHaveBeenCalledTimes(1)
    expect(disableMock).toHaveBeenCalledTimes(0)
  })

  it("calls track.disable() on toggle click", () => {
    const { getByRole } = render(true)

    userEvent.click(getByRole("button"))

    expect(enableMock).toHaveBeenCalledTimes(1)
    expect(disableMock).toHaveBeenCalledTimes(1)
  })
})
