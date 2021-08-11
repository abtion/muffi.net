const useHubMock = jest.fn()

useHubMock._reset = () => {
  const listeners = {}
  const connectionMock = {
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

  useHubMock.mockImplementation((_path, onHubConnected, _connectionOptions) => {
    onHubConnected(connectionMock)
  })
}

useHubMock._reset()

export default useHubMock
