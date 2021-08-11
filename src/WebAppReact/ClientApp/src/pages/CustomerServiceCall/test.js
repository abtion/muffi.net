import React from "react"
import { act, render as tlRender } from "@testing-library/react"
import axios from "axios"
import { createMemoryHistory } from "history"
import { Router } from "react-router"
import { Route } from "react-router-dom"

import CustomerServiceCall from "./"
import ThemeContext from "~/contexts/ThemeContext"
import { defaultTheme } from "~/themes"
import useHub from "~/hooks/useHub"
import getFirstName from "~/utils/getFirstName"
const supportTicketId = "fc857719-7dfc-4168-b5c7-c4962f36dccf"

jest.mock("~/utils/getFirstName")
jest.mock("axios")
jest.mock("~/hooks/useHub")
jest.mock("~/components/CustomerRoom", () => (props) => (
  <mock-CustomerRoom {...props}>CustomerRoom</mock-CustomerRoom>
))

function render() {
  const history = createMemoryHistory({
    initialEntries: [`/care1/service-call/${supportTicketId}`],
  })

  const context = tlRender(
    <ThemeContext.Provider value={defaultTheme}>
      <Router history={history}>
        <Route
          path="/:thirdparty/service-call/:supportTicketId"
          component={CustomerServiceCall}
        />
      </Router>
    </ThemeContext.Provider>
  )

  return { ...context, history }
}

axios.get.mockResolvedValue({
  data: {
    token: {
      twilioVideoGrantForCustomerToken: "some-video-grant",
      technicianFullName: "Tech Name",
      customerName: "Customer Name",
    },
  },
})

describe(CustomerServiceCall, () => {
  it("renders", async () => {
    const { findByText } = render()

    expect(await findByText("CustomerRoom")).toBeInTheDocument()
    expect(getFirstName).toHaveBeenCalledTimes(1)
  })

  it("redirects back to /:prefix when no twilioAccessToken in response", async () => {
    axios.get.mockResolvedValue({
      data: {
        token: {
          technicianFullName: "Tech Name",
          customerName: "Customer Name",
        },
      },
    })

    const { history } = await render()

    expect(history.location.pathname).toBe(`/care1`)
  })

  describe("hub connection", () => {
    beforeEach(() => {
      // We don't want the get request to finish, since it will happen after the test is over and
      // trigger a warning
      axios.get.mockReturnValue(new Promise(() => {}))
    })

    it("joins the supportTicket's group", async () => {
      render()

      expect(useHub.connectionMock.invoke).toHaveBeenCalledWith(
        "JoinGroup",
        supportTicketId
      )
    })

    it("navigates to the service call room when the call is started", async () => {
      const { history } = render()

      act(() => {
        useHub.connectionMock._trigger("technicianhasendedcall", {
          supportTicketId,
        })
      })

      expect(history.location.pathname).toBe(`/care1/thank-you`)
    })
  })
})
