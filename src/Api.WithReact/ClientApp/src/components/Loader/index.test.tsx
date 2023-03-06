import { render } from "@testing-library/react"
import React from "react"
import Loader from "."

describe(Loader, () => {
  it("renders the text", async () => {
    const { getByText } = render(<Loader text="test text" />)

    const text = getByText("test text")
    expect(text).toBeInTheDocument()
  })

  it("renders the loader icon", () => {
    const { container } = render(<Loader />)

    const icon = container.querySelector(".Loader__icon")
    expect(icon).toContainHTML("Mocked SVG Icon")
  })
})
