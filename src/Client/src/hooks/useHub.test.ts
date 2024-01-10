import useHub from "./useHub"
import { renderHook, waitFor } from "~/utils/test-utils"

const path = "endpoint/api"
const withUrlMock = vi.fn()
const withAutomaticReconnectMock = vi.fn()
const buildMock = vi.fn()
const connectionMock = { start: vi.fn(() => Promise.resolve()), stop: vi.fn() }
const onHubConnected = vi.fn()

vi.mock("@microsoft/signalr", () => {
  return {
    HubConnectionBuilder: vi.fn().mockImplementation(() => ({
      withUrl: withUrlMock,
      withAutomaticReconnect: withAutomaticReconnectMock.mockImplementation(
        () => ({ build: buildMock.mockImplementation(() => connectionMock) }),
      ),
    })),
  }
})

describe(useHub, () => {
  it("creates HubConnectionBuilder & tries to connect to hub", async () => {
    const connectionOptions = {}

    renderHook(() => useHub(path, onHubConnected, connectionOptions))

    expect(withUrlMock).toHaveBeenCalledWith(path, connectionOptions)
    expect(withAutomaticReconnectMock).toHaveBeenCalled()
    expect(buildMock).toHaveBeenCalled()
    expect(connectionMock.start).toHaveBeenCalled()

    await waitFor(() => {
      expect(onHubConnected).toHaveBeenCalledWith(connectionMock)
    })
  })

  describe("when connection cannot be started", () => {
    it("fails with undefined connection", async () => {
      const consoleErrorSpy = vi.spyOn(console, "error")

      connectionMock.start.mockImplementationOnce(async () => {
        throw new Error("This error is expected!")
      })

      renderHook(() => useHub(path, onHubConnected))

      await waitFor(() => {
        expect(consoleErrorSpy).toHaveBeenCalled()
      })

      consoleErrorSpy.mockRestore()
    })
  })
})
