import React from "react"
import { render as tlRender } from "@testing-library/react"
import { createMemoryHistory } from "history"
import { Router } from "react-router"
import axios from "axios"
import { waitFor } from "@testing-library/dom"

import TableQueuedSupportCalls from "./"
import userEvent from "@testing-library/user-event"
import withMockedDate from "~/utils/withMockedDate"

jest.mock("axios")

const queuedSupportTicket = {
  customerName: "Ruth InQueue",
  customerEmail: "email@email.dk",
  customerPhone: "12345678",
  supportTicketId: "1128c831-bc42-427c-bbfc-3a2febf0abad",
  brand: "apple",
  callEndedAt: null,
  callStartedAt: null,
  createdAt: "2021-06-28T10:00:00.000000",
  technicianFullName: "",
  technicianUserId: null,
}
beforeEach(() => {
  axios.post.mockResolvedValue()
})

afterEach(() => axios._reset())
function render() {
  const history = createMemoryHistory({
    initialEntries: ["/technician"],
  })

  const context = tlRender(
    <Router history={history}>
      <TableQueuedSupportCalls supportTickets={[queuedSupportTicket]} />
    </Router>
  )

  return { ...context, history }
}

describe(TableQueuedSupportCalls, () => {
  it("renders a table", async () => {
    const { findByRole } = render()
    expect(await findByRole("table")).toBeInTheDocument()
  })

  it("renders the support ticket with correct information", async () => {
    await withMockedDate(new Date("2021-06-28T10:11:00.000000"), async () => {
      const { findByText } = render()

      expect(await findByText("Ruth InQueue")).toBeInTheDocument()
      expect(await findByText("email@email.dk")).toBeInTheDocument()
      expect(await findByText("12345678")).toBeInTheDocument()
      expect(await findByText("apple")).toBeInTheDocument()
      expect(await findByText("11")).toBeInTheDocument()
      expect(await findByText("Start")).toBeInTheDocument()
    })
  })

  describe("when clicking start video room", () => {
    it("creates a room using axios post", async () => {
      const { findByText } = render()
      userEvent.click(await findByText("Start"))

      expect(axios.post).toHaveBeenCalledWith(
        "/api/technician/createroom",
        { SupportTicketId: queuedSupportTicket.supportTicketId },
        {
          headers: {
            authorization: "Bearer undefined",
          },
        }
      )
    })
    it("redirects to the service call", async () => {
      const { history, findByText } = render()
      userEvent.click(await findByText("Start"))

      await waitFor(() => {
        expect(history.location.pathname).toBe(
          `/technician/service-call/${queuedSupportTicket.supportTicketId}`
        )
      })
    })
  })
})
