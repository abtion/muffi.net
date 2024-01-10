import { useContext } from "react"
import { AuthContext, AuthContextProps } from "react-oidc-context"
import AuthBarrier from "."
import { render } from "~/utils/test-utils"
import ApiContext from "~/contexts/ApiContext"

vi.unmock("axios")

const emptyAuthContext = {
  signinRedirect: vi.fn(),
  removeUser: vi.fn(),
  // eslint-disable-next-line camelcase
  user: null,
  isLoading: false,
  isAuthenticated: false,
} as Partial<AuthContextProps> as AuthContextProps

describe(AuthBarrier, () => {
  describe("when not logged in", () => {
    it("renders a loader", () => {
      const { queryByText } = render(
        <AuthContext.Provider value={emptyAuthContext}>
          <AuthBarrier>
            <div>Child component text</div>
          </AuthBarrier>
        </AuthContext.Provider>,
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
        isAuthenticated: true,
        // eslint-disable-next-line camelcase
        user: { id_token: "abcd1234" },
      } as Partial<AuthContextProps> as AuthContextProps

      const { queryByText } = render(
        <AuthContext.Provider value={loggedInContext}>
          <AuthBarrier>
            <div>Child component text</div>
          </AuthBarrier>
        </AuthContext.Provider>,
      )

      expect(queryByText("Loading...")).not.toBeInTheDocument()
      expect(queryByText("Child component text")).toBeInTheDocument()
    })

    it('provides an "authenticated" API through context', () => {
      const loggedInContext = {
        ...emptyAuthContext,
        isLoading: false,
        isAuthenticated: true,
        // eslint-disable-next-line camelcase
        user: { id_token: "abcd1234" },
      } as Partial<AuthContextProps> as AuthContextProps

      const InnerComponent = () => {
        const api = useContext(ApiContext)

        expect(api.defaults.headers.authorization).toEqual("Bearer abcd1234")

        return null
      }

      render(
        <AuthContext.Provider value={loggedInContext}>
          <AuthBarrier>
            <InnerComponent />
          </AuthBarrier>
        </AuthContext.Provider>,
      )
    })
  })
})
