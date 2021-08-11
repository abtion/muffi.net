import React from "react"
import { render as tlRender } from "@testing-library/react"
// import userEvent from "@testing-library/user-event"
// import { createMemoryHistory } from "history"

import ChatHistory from "./"
import ChatContext from "~/contexts/ChatContext"

let scrollToBottom = jest.fn()
window.HTMLElement.prototype.scrollIntoView = scrollToBottom

function render(chatHistory) {
  const context = tlRender(
    <ChatContext.Provider
      value={{
        history: chatHistory,
      }}
    >
      <ChatHistory
        localTrack={{ name: "technicianData" }}
        remoteName="testcustomer"
      />
    </ChatContext.Provider>
  )

  return { ...context }
}

const oneLocalMessage = [
  {
    content: "testmessage",
    timestamp: 1628001751122,
    track: { name: "technicianData" },
  },
]
const twoLocalMessages = [
  {
    content: "testmessage",
    timestamp: 1628001751122,
    track: { name: "technicianData" },
  },
  {
    content: "testmessage2",
    timestamp: 1628001751125,
    track: { name: "technicianData" },
  },
]
const threeMixedMessages = [
  {
    content: "testmessage",
    timestamp: 1628001751122,
    track: { name: "technicianData" },
  },
  {
    content: "testmessage2",
    timestamp: 1628001751125,
    track: { name: "technicianData" },
  },
  {
    content: "testmessage3",
    timestamp: 1628001751125,
    track: { name: "customerData" },
  },
]

test("should render single local message 'testmessage' with timestamp & message-sender", () => {
  const { getByText } = render(oneLocalMessage)

  expect(scrollToBottom).toHaveBeenCalledTimes(1)

  const msgSpan = getByText("testmessage")
  expect(msgSpan).toBeInTheDocument()

  const msgParentDiv = msgSpan.parentElement
  expect(msgParentDiv).toHaveClass("message", "local-message")

  const msgFirstChild = msgParentDiv.firstChild
  expect(msgFirstChild).toHaveClass("message-sender")
  expect(msgFirstChild).toHaveTextContent("Dig")

  const msgParentParentDiv = msgParentDiv.parentElement
  expect(msgParentParentDiv.firstChild).toHaveClass("message-timestamp")
})

test("should render second local message 'testmessage2' without timestamp & message-sender", () => {
  const { getByText } = render(twoLocalMessages)

  expect(scrollToBottom).toHaveBeenCalledTimes(2)

  const msgSpan = getByText("testmessage2")
  expect(msgSpan).toBeInTheDocument()

  const msgParentDiv = msgSpan.parentElement
  expect(msgParentDiv).toHaveClass("message", "local-message")

  const msgFirstChild = msgParentDiv.firstChild
  expect(msgFirstChild).not.toHaveClass("message-sender")
  expect(msgFirstChild).not.toHaveTextContent("Dig")

  const msgParentParentDiv = msgParentDiv.parentElement
  expect(msgParentParentDiv.firstChild).not.toHaveClass("message-timestamp")
})

test("should render third message 'testmessage3' with timestamp & message-sender", () => {
  const { getByText } = render(threeMixedMessages)

  expect(scrollToBottom).toHaveBeenCalledTimes(3)

  const msgSpan = getByText("testmessage3")
  expect(msgSpan).toBeInTheDocument()

  const msgParentDiv = msgSpan.parentElement
  expect(msgParentDiv).toHaveClass("message", "remote-message")

  const msgFirstChild = msgParentDiv.firstChild
  expect(msgFirstChild).toHaveClass("message-sender")
  expect(msgFirstChild).toHaveTextContent("testcustomer")

  const msgParentParentDiv = msgParentDiv.parentElement
  expect(msgParentParentDiv.firstChild).toHaveClass("message-timestamp")
})
