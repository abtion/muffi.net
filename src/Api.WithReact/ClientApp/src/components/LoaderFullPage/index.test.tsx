import { render } from "@testing-library/react"
import React from "react"
import LoaderFullPage from "."

describe(LoaderFullPage, () => {
  it("renders the loader", async () => {
    const { getByText } = render(<LoaderFullPage text="test text" />)

    const text = getByText("test text")
    expect(text).toBeInTheDocument()
  })
})
