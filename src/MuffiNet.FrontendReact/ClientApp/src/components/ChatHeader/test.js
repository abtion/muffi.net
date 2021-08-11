import React from "react"
import { render as tlRender } from "@testing-library/react"

import ChatHeader from "./"

function render() {
  const context = tlRender(
    <ChatHeader className="some-classname">
      <p>some child element</p>
      <div>another child element</div>
    </ChatHeader>
  )

  return { ...context }
}

describe(ChatHeader, () => {
  it("should render child elements", () => {
    const { getByText } = render()

    const child1 = getByText("some child element")
    expect(child1).toBeInTheDocument()

    const child2 = getByText("another child element")
    expect(child2).toBeInTheDocument()
  })

  it("should use classnames from props", () => {
    const { getByText } = render()

    const parentDiv = getByText("some child element").parentElement
    expect(parentDiv).toHaveClass("ChatHeader", "some-classname")
  })
})
