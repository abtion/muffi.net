import axios from "axios"

export = AxiosMock

interface AxiosMock extends jest.Mocked<typeof axios> {
  _reset(): void
}
