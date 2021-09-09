import React from "react"
import { render as tlRender } from "@testing-library/react"
import { createMemoryHistory } from "history"
import { Router } from "react-router"

import Home from "./"

jest.mock("~/components/ExampleContainer", () => (props) => (
  <mock-ExampleContainer {...props}>ExampleContainer</mock-ExampleContainer>
))

function render() {
  const history = createMemoryHistory({
    initialEntries: [`/`],
  })

  const context = tlRender(
    <Router history={history}>
      <Home />
    </Router>
  )

  return { ...context, history }
}

describe(Home, () => {
  it("renders header 'Title'", async () => {
    const { findByText } = render()
    expect(await findByText("Title")).toBeInTheDocument()
  })
})
