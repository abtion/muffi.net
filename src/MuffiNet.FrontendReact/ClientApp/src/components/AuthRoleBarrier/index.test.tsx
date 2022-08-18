import React from "react"
import { AuthContext, AuthContextProps, User } from "oidc-react"
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
    signIn: jest.fn(),
    signInPopup: jest.fn(),
    signOut: jest.fn(),
    signOutRedirect: jest.fn(),
    isLoading: false,
    userData: userRoles
      ? ({
          profile: {
            roles: userRoles,
          },
        } as Partial<User> as User)
      : null,
  } as Partial<AuthContextProps> as AuthContextProps

  return render(
    <AuthContext.Provider value={mockAuthContext}>
      <AuthRoleBarrier allow={allow}>Contents of Role Barrier</AuthRoleBarrier>
    </AuthContext.Provider>
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
