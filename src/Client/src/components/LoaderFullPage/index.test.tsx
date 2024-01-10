import { render } from "~/utils/test-utils"
import LoaderFullPage from "."

describe(LoaderFullPage, () => {
  it("renders the loader", async () => {
    const { getByText } = render(<LoaderFullPage text="test text" />)

    const text = getByText("test text")
    expect(text).toBeInTheDocument()
  })
})
