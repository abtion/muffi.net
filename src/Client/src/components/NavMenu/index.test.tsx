import { render } from "~/utils/test-utils"
import userEvent from "@testing-library/user-event"
import { act } from "react-dom/test-utils"

import NavMenu from "."
import { AuthContext, AuthContextProps } from "react-oidc-context"
import { MemoryRouter } from "react-router-dom"

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
    </AuthContext.Provider>,
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
