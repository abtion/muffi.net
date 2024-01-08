import { AuthContext, AuthContextProps } from "react-oidc-context"
import AuthRoleBarrier from "."
import { render } from "@testing-library/react"

jest.unmock("axios")

function setup({
  allow,
  userRoles,
}: {
  allow: string[]
  userRoles?: string[]
}) {
  const mockAuthContext = {
    signinRedirect: jest.fn(),
    removeUser: jest.fn(),
    isLoading: false,
    user: userRoles
      ? {
          profile: {
            roles: userRoles,
          },
        }
      : null,
  } as Partial<AuthContextProps> as AuthContextProps

  return render(
    <AuthContext.Provider value={mockAuthContext}>
      <AuthRoleBarrier allow={allow}>Contents of Role Barrier</AuthRoleBarrier>
    </AuthContext.Provider>,
  )
}

test(`can display elements to Users with a required Role`, () => {
  const { queryByText } = setup({
    allow: ["Administrators"],
    userRoles: ["Administrators"],
  })

  expect(queryByText("Contents of Role Barrier")).toBeInTheDocument()
})

test(`can hide elements from Users that are not logged in`, () => {
  const { queryByText } = setup({ allow: ["Administrators"] })

  expect(queryByText("Contents of Role Barrier")).not.toBeInTheDocument()
})

test(`can hide elements from Users who do not have a required Role`, () => {
  const { queryByText } = setup({
    allow: ["Administrators"],
    userRoles: ["SomeOtherRole"],
  })

  expect(queryByText("Contents of Role Barrier")).not.toBeInTheDocument()
})
