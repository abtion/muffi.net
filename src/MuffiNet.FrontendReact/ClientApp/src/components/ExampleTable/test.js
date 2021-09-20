import React from "react"
import { render as tlRender } from "@testing-library/react"
import userEvent from "@testing-library/user-event"

import ExampleTable from "./"

let onRemove = jest.fn()
function render(entityList) {
  const context = tlRender(
    <ExampleTable entities={entityList} onRemove={onRemove} />
  )

  return { ...context }
}

describe(ExampleTable, () => {
  it("renders table", async () => {
    const { getByRole } = render([])
    expect(getByRole("table")).toBeInTheDocument()
  })

  it("calls onRemove on click", async () => {
    const entity = {
      id: "1",
      name: undefined,
      description: "someRandomDescription",
      email: "an@email.dk",
      phone: "12345678",
    }
    const { findByText } = render([entity])
    const removeBtn = await findByText("Remove")
    expect(removeBtn).toBeInTheDocument()

    userEvent.click(removeBtn)
    expect(onRemove).toHaveBeenCalledTimes(1)
  })
})
