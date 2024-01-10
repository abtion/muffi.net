import { render } from "~/utils/test-utils"
import userEvent from "@testing-library/user-event"

import ExampleTable from "./"
import { ExampleEntity } from "~/types/ExampleEntity"

const onRemove = vi.fn()

function renderTable(entityList: ExampleEntity[]) {
  const context = render(
    <ExampleTable entities={entityList} onRemove={onRemove} />,
  )

  return { ...context }
}

describe(ExampleTable, () => {
  it("renders table", async () => {
    const { getByRole } = renderTable([])
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
    const { findByText } = renderTable([entity])
    const removeBtn = await findByText("Remove")
    expect(removeBtn).toBeInTheDocument()

    await userEvent.click(removeBtn)
    expect(onRemove).toHaveBeenCalledTimes(1)
  })
})
