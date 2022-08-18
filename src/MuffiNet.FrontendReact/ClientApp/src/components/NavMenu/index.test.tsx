import React from "react"
import { render } from "@testing-library/react"
import userEvent from "@testing-library/user-event"
import { act } from "react-dom/test-utils"
import { MemoryRouter } from "react-router"

import NavMenu from "."
import { AuthContext, AuthContextProps } from "oidc-react"

function renderMenu() {
  const mockAuthContext = {
    userData: null,
    isLoading: false,
  } as Partial<AuthContextProps> as AuthContextProps

  const context = render(
    <AuthContext.Provider value={mockAuthContext}>
      <MemoryRouter>
        <NavMenu />
      </MemoryRouter>
    </AuthContext.Provider>
  )

  return { ...context }
}

describe(NavMenu, () => {
  it("allows toggling the nav menu for small screens", async () => {
    const { container } = renderMenu()

    const mobileMenuButton = container.querySelector(".mobile-menu-button")
    const navMenu = container.querySelector("ul")

    expect(navMenu).toHaveClass("hidden")

    await act(async () => {
      if (mobileMenuButton) {
        await userEvent.click(mobileMenuButton)
      }
    })

    expect(navMenu).not.toHaveClass("hidden")
  })
})
