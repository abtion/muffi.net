import React from "react"
import { render as tlRender } from "@testing-library/react"
import userEvent from "@testing-library/user-event"
import { act } from "react-dom/test-utils"
import { MemoryRouter } from "react-router"

import NavMenu from "."

function render() {
  const context = tlRender(
    <MemoryRouter>
      <NavMenu />
    </MemoryRouter>
  )

  return { ...context }
}

describe(NavMenu, () => {
  it("allows toggling the nav menu for small screens", async () => {
    const { container } = render()

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
