import { renderHook, act } from "@testing-library/react-hooks"
import { waitFor } from "@testing-library/react"
import { HubConnectionBuilder, connectionMock } from "@microsoft/signalr"

import useHub from "./useHub"

const path = "endpoint/api"
const onHubConnected = jest.fn()
const connectionOptions = jest.fn()

describe(useHub, () => {
  it("creates HubConnectionBuilder & tries to connect to hub", async () => {
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
      const consoleLogSpy = jest.spyOn(console, "log")
      consoleLogSpy.mockImplementation()

      connectionMock.start.mockImplementationOnce(async () => {
        throw new Error("Some error")
      })

      renderHook(() => useHub(path, onHubConnected, connectionOptions))

      await waitFor(() => {
        expect(console.log).toHaveBeenCalled()
      })

      consoleLogSpy.mockRestore()
    })
  })
})
