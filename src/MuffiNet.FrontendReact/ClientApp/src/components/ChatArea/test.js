import React from "react"
import { render as tlRender } from "@testing-library/react"
// import userEvent from "@testing-library/user-event"

import ChatArea from "./"

let remoteTrackOnMock = jest.fn((arg1, cb) => {
  cb(JSON.stringify({ content: "messageToPost" }))
})
let remoteTrackOffMock = jest.fn()

function render(chatHistory) {
  const context = tlRender(
    <ChatArea
      remoteTrack={{
        on: remoteTrackOnMock,
        off: remoteTrackOffMock,
      }}
    >
      <div>some child element</div>
    </ChatArea>
  )

  return { ...context }
}
describe(ChatArea, () => {
  it("renders child elements and subscribes to remoteTrack", () => {
    const { getByText } = render()

    const child = getByText("some child element")
    expect(child).toBeInTheDocument()

    expect(remoteTrackOnMock).toHaveBeenCalledTimes(1)
  })
})
