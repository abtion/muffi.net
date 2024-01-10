import { render } from "~/utils/test-utils"
import Loader from "."

describe(Loader, () => {
  it("renders the text", async () => {
    const { getByText } = render(<Loader text="test text" />)

    const text = getByText("test text")
    expect(text).toBeInTheDocument()
  })

  it("adds spinnerClassName and textClassName to the correct elements", () => {
    const { container } = render(
      <Loader spinnerClassName="the-spinner" textClassName="the-text" />,
    )

    const spinnerElement = container.querySelector(".Loader__spinner")
    const textElement = container.querySelector(".Loader__text")

    expect(spinnerElement).toHaveClass("the-spinner")
    expect(textElement).toHaveClass("the-text")
  })
})
