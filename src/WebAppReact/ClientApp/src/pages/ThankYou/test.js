import React from "react"
import { render as tlRender } from "@testing-library/react"

import ThankYou from "./"

function render() {
  const context = tlRender(<ThankYou />)

  return { ...context }
}
describe(ThankYou, () => {
  it("renders", () => {
    const { getByText } = render()

    const thankYouDiv = getByText("Tak for dit opkald!")
    expect(thankYouDiv).toBeInTheDocument()
  })
})
