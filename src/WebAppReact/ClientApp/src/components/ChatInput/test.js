import React from "react"
import { render as tlRender } from "@testing-library/react"
import userEvent from "@testing-library/user-event"

import ChatInput from "./"
import ChatContext from "~/contexts/ChatContext"

let postMessageMock = jest.fn()
let sendMock = jest.fn()

function render(chatHistory) {
  const context = tlRender(
    <ChatContext.Provider
      value={{
        history: chatHistory,
        postMessage: postMessageMock,
      }}
    >
      <ChatInput
        track={{
          name: "technicianData",
          send: sendMock,
        }}
      />
    </ChatContext.Provider>
  )

  return { ...context }
}
describe(ChatInput, () => {
  it("renders Send button", () => {
    const { getByRole } = render([])

    const button = getByRole("button")
    expect(button).toBeInTheDocument()
    expect(button).toHaveTextContent("Send")
  })

  it("renders textarea", () => {
    const { getByRole } = render()

    const textarea = getByRole("textbox")
    expect(textarea).toBeInTheDocument()
    expect(textarea).toHaveClass("ChatInput__InputTextField")
  })

  it("allows typing and submitting text to handleSubmit()", () => {
    const { getByRole } = render()

    const textarea = getByRole("textbox")
    userEvent.type(textarea, "myMessage")
    expect(textarea).toHaveTextContent("myMessage")

    const button = getByRole("button")
    userEvent.click(button)

    expect(sendMock).toHaveBeenCalledTimes(1)
    expect(postMessageMock).toHaveBeenCalledTimes(1)
    expect(textarea).toHaveTextContent("")
  })

  it("allows using enter key to submit", () => {
    const { getByRole } = render()

    const textarea = getByRole("textbox")
    userEvent.type(textarea, "myMessage{enter}")

    expect(sendMock).toHaveBeenCalledTimes(2)
    expect(postMessageMock).toHaveBeenCalledTimes(2)
    expect(textarea).toHaveTextContent("")
  })
})
