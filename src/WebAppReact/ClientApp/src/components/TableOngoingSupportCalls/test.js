import React from "react"
import { render as tlRender } from "@testing-library/react"
import { createMemoryHistory } from "history"
import { Router } from "react-router"
import { waitFor } from "@testing-library/dom"

import TableOngoingSupportCalls from "./"
import userEvent from "@testing-library/user-event"
import withMockedDate from "~/utils/withMockedDate"

window.confirm = jest.fn(() => true)

const ongoingSupportTicket = {
  customerName: "Ruth Ongoing",
  customerEmail: "email@email.dk",
  customerPhone: "12345678",
  supportTicketId: "fc857719-7dfc-4168-b5c7-c4962f36dccf",
  brand: "apple",
  callEndedAt: null,
  callStartedAt: "2021-06-28T11:10:00.000000",
  createdAt: "2021-06-28T11:00:00.000000",
  technicianFullName: "Mikkel Mikkelsen",
  technicianUserId: "4b29aa3f-eca2-4867-802f-af2ed94c9d8b",
}

function render() {
  const history = createMemoryHistory({
    initialEntries: ["/technician"],
  })

  const context = tlRender(
    <Router history={history}>
      <TableOngoingSupportCalls supportTickets={[ongoingSupportTicket]} />
    </Router>
  )

  return { ...context, history }
}

describe(TableOngoingSupportCalls, () => {
  it("renders a table", async () => {
    const { findByRole } = render()
    expect(await findByRole("table")).toBeInTheDocument()
  })

  it("renders the support ticket with correct information", async () => {
    await withMockedDate(new Date("2021-06-28T11:35:00.000000"), async () => {
      const { findByText } = render()
      expect(await findByText("Ruth Ongoing")).toBeInTheDocument()
      expect(await findByText("email@email.dk")).toBeInTheDocument()
      expect(await findByText("12345678")).toBeInTheDocument()
      expect(await findByText("apple")).toBeInTheDocument()
      expect(await findByText("25")).toBeInTheDocument()
      expect(await findByText("Mikkel Mikkelsen")).toBeInTheDocument()
      expect(await findByText("Gå til kald")).toBeInTheDocument()
    })
  })

  describe("when clicking 'Gå til kald'", () => {
    it("it asks you to confirm your action", async () => {
      const { findByText } = render()
      userEvent.click(await findByText("Gå til kald"))

      await waitFor(() => {
        expect(window.confirm).toBeCalled()
      })
    })
    it("redirects to the ongoing servicecall", async () => {
      const { history, findByText } = render()
      userEvent.click(await findByText("Gå til kald"))

      await waitFor(() => {
        expect(history.location.pathname).toBe(
          `/technician/service-call/${ongoingSupportTicket.supportTicketId}`
        )
      })
    })
  })
})
