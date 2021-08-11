import React from "react"
import { render as tlRender } from "@testing-library/react"
import axios from "axios"
import { createMemoryHistory } from "history"
import { Router } from "react-router"

import TechServiceCall from "./"

const supportTicketId = "fc857719-7dfc-4168-b5c7-c4962f36dccf"

jest.mock("axios")
jest.mock("~/components/TechRoom", () => (props) => (
  <mock-TechRoom {...props}>TechRoom</mock-TechRoom>
))

function render() {
  const history = createMemoryHistory({
    initialEntries: [`/technician/service-call/${supportTicketId}`],
  })

  const context = tlRender(
    <Router history={history}>
      <TechServiceCall />
    </Router>
  )

  return { ...context, history }
}

describe(TechServiceCall, () => {
  it("renders", async () => {
    axios.get.mockResolvedValue({
      data: {
        supportTicket: {
          twilioVideoGrantForTechnicianToken: "some-video-grant",
        },
      },
    })

    const { findByText } = render()

    expect(await findByText("TechRoom")).toBeInTheDocument()
  })

  it("redirects to /technician when no supportTicket in response", async () => {
    axios.get.mockResolvedValue({
      data: {
        supportTicket: {},
      },
    })

    const { history } = await render()

    expect(history.location.pathname).toBe(`/technician`)
  })
})
