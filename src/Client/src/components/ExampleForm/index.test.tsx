import userEvent from "@testing-library/user-event"
import ExampleForm from "./"
import { act, render } from "~/utils/test-utils"

const onSubmit = vi.fn()

function renderForm() {
  const context = render(<ExampleForm onSubmit={onSubmit} />)
  return { ...context }
}

describe(ExampleForm, () => {
  it("renders correct inputs", () => {
    const { getByLabelText } = renderForm()

    expect(getByLabelText("Name")).toBeInTheDocument()
    expect(getByLabelText("Description")).toBeInTheDocument()
    expect(getByLabelText("E-mail")).toBeInTheDocument()
    expect(getByLabelText("Phone")).toBeInTheDocument()
  })

  it("should render submit button 'Submit'", () => {
    const { getByRole } = renderForm()

    expect(getByRole("button")).toHaveTextContent("Submit")
  })

  describe("when submitting the form", () => {
    it("posts an exampleEntity", async () => {
      const { getByLabelText, getByText } = renderForm()

      await act(async () => {
        await userEvent.type(getByLabelText("Name"), "Name")
        await userEvent.type(getByLabelText("Description"), "Description")
        await userEvent.type(getByLabelText("E-mail"), "Em@a.il")
        await userEvent.type(getByLabelText("Phone"), "12345678")

        await userEvent.click(getByText("Submit"))
      })

      expect(onSubmit).toHaveBeenCalledTimes(1)
    })
  })
})
