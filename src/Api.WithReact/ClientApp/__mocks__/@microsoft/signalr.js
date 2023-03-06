export * from "@microsoft/signalr"
import connectionMock from "./signalr/connectionMock"

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
