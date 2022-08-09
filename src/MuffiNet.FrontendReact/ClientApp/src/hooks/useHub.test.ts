import { renderHook } from "@testing-library/react"
import { waitFor } from "@testing-library/react"
import { HubConnectionBuilder } from "@microsoft/signalr"
import connectionMock from "../../__mocks__/@microsoft/signalr/connectionMock"

import useHub from "./useHub"

const path = "endpoint/api"
const onHubConnected = jest.fn()

describe(useHub, () => {
  it("creates HubConnectionBuilder & tries to connect to hub", async () => {
    const connectionOptions = {}

    renderHook(() => useHub(path, onHubConnected, connectionOptions))

    expect(HubConnectionBuilder.prototype.withUrl).toHaveBeenCalledWith(
      path,
      connectionOptions
    )
    expect(
      HubConnectionBuilder.prototype.withAutomaticReconnect
    ).toHaveBeenCalled()
    expect(HubConnectionBuilder.prototype.build).toHaveBeenCalled()
    expect(connectionMock.start).toHaveBeenCalled()

    await waitFor(() => {
      expect(onHubConnected).toHaveBeenCalledWith(connectionMock)
    })
  })

  describe("when connection cannot be started", () => {
    it("fails with undefined connection", async () => {
      const consoleErrorSpy = jest.spyOn(console, "error")
      consoleErrorSpy.mockImplementation()

      connectionMock.start.mockImplementationOnce(async () => {
        throw new Error("Some error")
      })

      renderHook(() => useHub(path, onHubConnected))

      await waitFor(() => {
        expect(consoleErrorSpy).toHaveBeenCalled()
      })

      consoleErrorSpy.mockRestore()
    })
  })
})
