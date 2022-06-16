import React, { useContext } from "react"
import { AuthContext } from "oidc-react"
import AuthBarrier from "."
import { render } from "@testing-library/react"
import ApiContext from "~/contexts/ApiContext"

jest.unmock("axios")

const emptyAuthContext = {
  signIn: jest.fn(),
  signInPopup: jest.fn(),
  signOut: jest.fn(),
  signOutRedirect: jest.fn(),
  // eslint-disable-next-line camelcase
  userData: null,
  isLoading: false,
}

describe(AuthBarrier, () => {
  it("renders a loader", () => {
    describe("when not logged in", () => {
      const { queryByText } = render(
        // @ts-expect-error: No need to implement the whole auth context
        <AuthContext.Provider value={emptyAuthContext}>
          <AuthBarrier>
            <div>Child component text</div>
          </AuthBarrier>
        </AuthContext.Provider>
      )

      expect(queryByText("Child component text")).not.toBeInTheDocument()
      expect(queryByText("Loading...")).toBeInTheDocument()
    })
  })

  describe("when logged in", () => {
    it("renders the child component", () => {
      const loggedInContext = {
        ...emptyAuthContext,
        isLoading: false,
        // eslint-disable-next-line camelcase
        userData: { id_token: "abcd1234" },
      }

      const { queryByText } = render(
        // @ts-expect-error: No need to implement the whole auth context
        <AuthContext.Provider value={loggedInContext}>
          <AuthBarrier>
            <div>Child component text</div>
          </AuthBarrier>
        </AuthContext.Provider>
      )

      expect(queryByText("Loading...")).not.toBeInTheDocument()
      expect(queryByText("Child component text")).toBeInTheDocument()
    })

    it('provides an "authenticated" API through context', () => {
      const loggedInContext = {
        ...emptyAuthContext,
        isLoading: false,
        // eslint-disable-next-line camelcase
        userData: { id_token: "abcd1234" },
      }

      const InnerComponent = () => {
        const api = useContext(ApiContext)

        expect(api.defaults.headers.authorization).toEqual("Bearer abcd1234")

        return null
      }

      render(
        // @ts-expect-error: No need to implement the whole auth context
        <AuthContext.Provider value={loggedInContext}>
          <AuthBarrier>
            <InnerComponent />
          </AuthBarrier>
        </AuthContext.Provider>
      )
    })
  })
})
