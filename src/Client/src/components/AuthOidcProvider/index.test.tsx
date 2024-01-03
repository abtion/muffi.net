import React from "react"
import AuthOidcProvider from "."
import axios from "axios"
import { render } from "@testing-library/react"
import { act } from "react-dom/test-utils"

const axiosMock = axios as jest.Mocked<typeof axios>

jest.mock("axios", () => ({
  get: jest.fn(),
}))

jest.mock("react-oidc-context", () => ({
  AuthProvider: ({ children }: { children: JSX.Element[] }) => (
    <div>{children}</div>
  ),
}))

describe(AuthOidcProvider, () => {
  it("fetches oidc config, then renders it's children inside an AuthProvider ", async () => {
    let resolveRequest: (value: unknown) => void
    const requestPromise = new Promise((resolve) => (resolveRequest = resolve))
    axiosMock.get.mockReturnValueOnce(requestPromise)

    const { queryByText } = render(
      <AuthOidcProvider>
        <div>Content</div>
      </AuthOidcProvider>,
    )

    expect(queryByText("Content")).not.toBeInTheDocument()

    await act(async () => {
      resolveRequest({
        data: {
          authority: "authority",
          clientId: "clientId",
          scopes: "scopes",
        },
      })

      // The state update happens in a callback for the resolved request.
      // The callback itself won't be executed until the next tick
      await new Promise(process.nextTick)
    })

    expect(queryByText("Content")).toBeInTheDocument()
  })
})
