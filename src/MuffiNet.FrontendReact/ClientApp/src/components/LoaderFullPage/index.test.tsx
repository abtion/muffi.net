import React from "react"
import render from "~/utils/render"
import LoaderFullPage from "."

describe(LoaderFullPage, () => {
  it("renders the loader", async () => {
    const { getByText } = render(<LoaderFullPage text="test text" />)

    const text = getByText("test text")
    expect(text).toBeInTheDocument()
  })
})
