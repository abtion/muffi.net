import React from "react"
import { act, render as tlRender } from "@testing-library/react"
import { createMemoryHistory } from "history"
import { Router } from "react-router"
import axios from "axios"

import WaitingRoom from "./"
import useHub from "~/hooks/useHub"
import { Route } from "react-router-dom"
import { defaultTheme } from "~/themes"
import ThemeContext from "~/contexts/ThemeContext"

jest.mock("axios")
jest.mock("~/hooks/useHub")
jest.mock("~/components/CallPreview", () => jest.fn(() => null))

const supportTicketId = "fc857719-7dfc-4168-b5c7-c4962f36dccf"
const estimatedWaitingTimeResponseData = {
  numberOfUnansweredCalls: 10,
}

beforeEach(() => {
  axios.get.mockResolvedValue({ data: estimatedWaitingTimeResponseData })
})

afterEach(() => {
  axios._reset()
  useHub._reset()
})

function render() {
  const history = createMemoryHistory({
    initialEntries: [`/care1/waiting-room/${supportTicketId}`],
  })

  const context = tlRender(
    <ThemeContext.Provider value={defaultTheme}>
      <Router history={history}>
        <Route
          path="/:thirdparty/waiting-room/:supportTicketId"
          component={WaitingRoom}
        />
      </Router>
    </ThemeContext.Provider>
  )

  return { ...context, history }
}

describe(WaitingRoom, () => {
  describe("polling for waiting time", () => {
    it("renders 'calculating number in queue' text", async () => {
      axios.get.mockReturnValue(new Promise(() => {}))
      const { getByText } = render()

      const queueText = getByText("Beregner plads i køen")
      expect(queueText).toBeInTheDocument()
    })

    it("displays customer's number in queue", async () => {
      axios.get.mockResolvedValueOnce({
        data: {
          numberOfUnansweredCalls: 1,
        },
      })
      const { findByText } = render()

      expect(await findByText("Du er nummer 2 i køen.")).toBeInTheDocument()
    })

    it("displays customer is next in queue", async () => {
      axios.get.mockResolvedValueOnce({
        data: {
          numberOfUnansweredCalls: 0,
        },
      })
      const { findByText } = render()

      expect(await findByText("Du er den næste i køen.")).toBeInTheDocument()
    })
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
        useHub.connectionMock._trigger("technicianhasstartedcall", {
          supportTicketId,
        })
      })

      expect(history.location.pathname).toBe(
        `/care1/service-call/${supportTicketId}`
      )
    })
  })
})
