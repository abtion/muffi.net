import React from "react"
import { render as tlRender } from "@testing-library/react"
import { createMemoryHistory } from "history"
import { Router } from "react-router"

import AuthorizedHome from "./"

jest.mock("~/components/ExampleContainer", () => (props) => (
  <mock-ExampleContainer {...props}>
    <p>ExampleContainer</p>
    <p>{props.accessToken}</p>
  </mock-ExampleContainer>
))

function render() {
  const history = createMemoryHistory({
    initialEntries: [`/authhome`],
  })

  const context = tlRender(
    <Router history={history}>
      <AuthorizedHome accessToken={"accessTokenText"} />
    </Router>
  )

  return { ...context, history }
}

describe(AuthorizedHome, () => {
  it("renders ExampleContainer", async () => {
    const { findByText } = render()
    expect(await findByText("ExampleContainer")).toBeInTheDocument()
  })
  it("serves accessToken to ExampleContainer", async () => {
    const { findByText } = render()
    expect(await findByText("accessTokenText")).toBeInTheDocument()
  })
})
