import { render } from "~/utils/test-utils"
import userEvent from "@testing-library/user-event"
import Dialog, { DialogProps } from "."
import { act } from "react-dom/test-utils"

describe(Dialog, () => {
  it("can close modal Dialog using ESC button", async () => {
    const onCloseMock = vi.fn()

    const props: DialogProps = {
      onClose: onCloseMock,
    }

    render(<Dialog {...props} />)

    await act(async () => {
      await userEvent.keyboard("{Escape}")
      await userEvent.keyboard("{Escape}")
    })

    expect(onCloseMock).toHaveBeenCalledTimes(1)
  })

  it("can close modal Dialog using 'x' icon", async () => {
    const onCloseMock = vi.fn()

    const props: DialogProps = {
      onClose: onCloseMock,
    }

    const { getByTitle } = render(<Dialog {...props} />)

    await act(async () => {
      await userEvent.click(getByTitle("Close"))
    })

    expect(onCloseMock).toHaveBeenCalledTimes(1)
  })
})

it("renders children", () => {
  const childrenMock = <p>Some Text</p>

  const props: DialogProps = {
    onClose: vi.fn(),
    children: childrenMock,
  }

  const { getByText } = render(<Dialog {...props} />)

  expect(getByText("Some Text")).toBeInTheDocument()
})

it("joins classnames on dialog", () => {
  const props: DialogProps = {
    onClose: vi.fn(),
    className: "john bob",
  }

  const { container } = render(<Dialog {...props} />)

  const dialog = container.firstElementChild?.children[1]

  expect(dialog).toHaveClass("john")
  expect(dialog).toHaveClass("bob")
  expect(dialog).toHaveClass("Dialog")
})

it("applies attributes to dialog", () => {
  const props: DialogProps = {
    onClose: vi.fn(),
    title: "SomeTitle",
    id: "SomeId",
  }

  const { container } = render(<Dialog {...props} />)

  const dialog = container.firstElementChild?.children[1]

  expect(dialog).toHaveAttribute("title", "SomeTitle")
  expect(dialog).toHaveAttribute("id", "SomeId")
})
