import { IHttpConnectionOptions } from "@microsoft/signalr"
import useHub from "../useHub"

// eslint-disable-next-line @typescript-eslint/no-explicit-any
type ConnectionMockListener = (...args: any[]) => void

type ConnectionMock = {
  invoke(): void
  on(methodName: string, newMethod: ConnectionMockListener): void
  _trigger(eventName: string, message: unknown): void
}

export interface UseHubMock extends jest.Mock {
  _reset(): void
  connectionMock: ConnectionMock
  (...args: Parameters<typeof useHub>): void
}

// eslint-disable-next-line @typescript-eslint/no-explicit-any
const useHubMock: any = jest.fn()

useHubMock._reset = () => {
  const listeners: Record<string, ConnectionMockListener[]> = {}
  const connectionMock: ConnectionMock = {
    invoke: jest.fn(),
    on: jest.fn((eventName, eventListener) => {
      if (!listeners[eventName]) listeners[eventName] = []
      listeners[eventName].push(eventListener)
    }),
    _trigger(eventName, message: unknown) {
      const eventListeners = listeners[eventName] || []
      eventListeners.forEach((eventListener) => eventListener(message))
    },
  }

  useHubMock.connectionMock = connectionMock

  useHubMock.mockImplementation(
    (
      _path: string,
      onHubConnected: (connection: ConnectionMock) => void,
      _connectionOptions?: IHttpConnectionOptions
    ) => {
      onHubConnected(connectionMock)
    }
  )
}

useHubMock._reset()

export default useHubMock as UseHubMock
