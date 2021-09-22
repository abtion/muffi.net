export * from "@microsoft/signalr"

export const connectionMock = {
  start: jest.fn(() => Promise.resolve()),
  stop: jest.fn(),
}

const mocks = {
  withUrl: jest.fn(),
  withAutomaticReconnect: jest.fn(),
  build: jest.fn(() => connectionMock),
}

export function HubConnectionBuilder() {
  this.withUrl.mockImplementation(() => this)
  this.withAutomaticReconnect.mockImplementation(() => this)
}

HubConnectionBuilder.prototype = mocks
