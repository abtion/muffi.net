import React from "react"
import { act, render as tlRender } from "@testing-library/react"
import { createMemoryHistory } from "history"
import { Router } from "react-router"
import axios from "axios"

import TechOverview from "./"
import useHub from "~/hooks/useHub"

jest.mock("axios")
jest.mock("~/hooks/useHub")

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

beforeEach(() => {
  axios.get.mockResolvedValue({
    data: { supportTickets: [queuedSupportTicket, ongoingSupportTicket] },
  })
})

afterEach(() => {
  axios._reset()
  useHub._reset()
})

function render() {
  const history = createMemoryHistory({
    initialEntries: ["/technician"],
  })

  const context = tlRender(
    <Router history={history}>
      <TechOverview />
    </Router>
  )

  return { ...context, history }
}

describe(TechOverview, () => {
  it("renders heading 'Supportkald'", async () => {
    const { findByRole } = render()

    expect(await findByRole("heading", { level: 1 })).toHaveTextContent(
      "Supportkald"
    )
  })

  it("renders two tables, one for the queue and one for ongoing calls", async () => {
    const { findAllByRole } = render()
    expect(await findAllByRole("table")).toHaveLength(2)
  })

  it("gets all support tickets", async () => {
    const { findByText } = render()

    expect(await findByText("Start")).toBeInTheDocument()
    expect(await findByText("Gå til kald")).toBeInTheDocument()

    expect(axios.get).toHaveBeenCalledWith(
      "/api/technician/allsupporttickets",
      { headers: { authorization: "Bearer undefined" } }
    )
  })

  describe("hub connection", () => {
    it("adds new records", async () => {
      const { findByText } = render()

      const supportTicket = {
        customerName: "Jørgen",
        customerEmail: "jørgen@email.dk",
        customerPhone: "",
        supportTicketId: "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
        createdAt: "2021-06-28T11:00:00.000000",
      }

      act(() => {
        useHub.connectionMock._trigger("SupportTicketCreated", {
          supportTicket,
        })
      })

      expect(await findByText("Jørgen")).toBeInTheDocument()
    })

    it("updates existing records", async () => {
      const { findByText } = render()

      expect(
        await findByText(queuedSupportTicket.customerName)
      ).toBeInTheDocument()

      const updatedSupportTicket = {
        ...queuedSupportTicket,
        customerName: "Updated customer name",
      }

      act(() => {
        useHub.connectionMock._trigger("SupportTicketUpdated", {
          supportTicket: updatedSupportTicket,
        })
      })

      expect(await findByText("Updated customer name")).toBeInTheDocument()
    })

    it("deletes records", async () => {
      const { findByText } = render()

      const customerNameElement = await findByText(
        queuedSupportTicket.customerName
      )

      expect(customerNameElement).toBeInTheDocument()

      act(() => {
        useHub.connectionMock._trigger("SupportTicketDeleted", {
          supportTicketId: queuedSupportTicket.supportTicketId,
        })
      })

      expect(customerNameElement).not.toBeInTheDocument()
    })
  })
})
