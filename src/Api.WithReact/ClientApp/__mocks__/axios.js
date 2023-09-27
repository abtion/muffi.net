const verbs = ["get", "post", "put", "patch", "delete"]

const axiosMock = {
  _reset() {
    verbs.forEach(
      (verb) =>
        (axiosMock[verb] = jest.fn((url) => {
          const message = [
            `Request not correctly mocked: ${verb.toUpperCase()} "${url}"\n`,
            "Mock the request with:",
            "",
            'import axios from "axios"',
            "...",
            `axios.${verb}.mockResolvedValue(yourMockedResponseObject)`,
          ].join("\n")

          throw new Error(message)
        })),
    )
  },
}

axiosMock._reset()

export default axiosMock
