import React from "react"
import { render as tlRender } from "@testing-library/react"
import userEvent from "@testing-library/user-event"
import axios from "axios"

import CustomerInfo from "./"

jest.mock("axios")

const supportTicket = {
  customerName: "Ruth",
  customerEmail: "email@email.dk",
  customerPhone: "12345678",
  supportTicketId: "1128c831-bc42-427c-bbfc-3a2febf0abad",
  created: "0001-01-01T00:00:00",
  brand: "apple",
}

function render() {
  const context = tlRender(<CustomerInfo supportTicket={supportTicket} />)

  return { ...context }
}

describe(CustomerInfo, () => {
  it("renders name", () => {
    const { getByText } = render()

    expect(getByText("Ruth")).toBeInTheDocument()
  })
  it("renders email", () => {
    const { getByText } = render()

    expect(getByText("email@email.dk")).toBeInTheDocument()
  })
  it("renders phone", () => {
    const { getByText } = render()

    expect(getByText("12345678")).toBeInTheDocument()
  })
  it("renders create-oss-case button", () => {
    const { getByText } = render()
    axios.get.mockReturnValue(new Promise(() => {}))

    expect(getByText("Create Oss Case")).toBeInTheDocument()
  })
  it("renders oss id from axios get request when clicking crease oss case button", async () => {
    const { getByText, findByText } = render()
    axios.get.mockResolvedValueOnce({
      data: {
        ossId: "123456",
      },
    })

    userEvent.click(getByText("Create Oss Case"))

    expect(await findByText("123456")).toBeInTheDocument()
  })
})
