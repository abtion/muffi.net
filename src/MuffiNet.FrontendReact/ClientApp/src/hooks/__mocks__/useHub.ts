import { IHttpConnectionOptions } from "@microsoft/signalr"
import useHub from "../useHub"

type ConnectionMock = {
  invoke(): void
  on(eventName: string, listener: Function): void
  _trigger(eventName: string, message: unknown): void
}

export interface UseHubMock extends jest.Mock {
  _reset(): void
  connectionMock: ConnectionMock
  (...args: Parameters<typeof useHub>): void
}

const useHubMock: any = jest.fn()

useHubMock._reset = () => {
  const listeners: Record<string, Function[]> = {}
  const connectionMock: ConnectionMock = {
    invoke: jest.fn(),
    on: jest.fn((eventName, listener) => {
      if (!listeners[eventName]) listeners[eventName] = []
      listeners[eventName].push(listener)
    }),
    _trigger(eventName, message) {
      const eventListeners = listeners[eventName] || []
      eventListeners.forEach((listener) => listener(message))
    },
  }

  useHubMock.connectionMock = connectionMock

  useHubMock.mockImplementation(
    (
      _path: string,
      onHubConnected: Function,
      _connectionOptions?: IHttpConnectionOptions
    ) => {
      onHubConnected(connectionMock)
    }
  )
}

useHubMock._reset()

export default useHubMock as UseHubMock
