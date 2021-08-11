import React from "react"
import { render as tlRender } from "@testing-library/react"
import { waitFor } from "@testing-library/dom"
import userEvent from "@testing-library/user-event"
import { createMemoryHistory } from "history"
import { Router } from "react-router"

import axios from "axios"

import SignUp from "./"
import ThemeContext from "~/contexts/ThemeContext"
import { defaultTheme } from "~/themes"

function render() {
  const history = createMemoryHistory({
    initialEntries: [`${defaultTheme.prefix}/sign-up`],
  })

  const context = tlRender(
    <ThemeContext.Provider value={defaultTheme}>
      <Router history={history}>
        <SignUp />
      </Router>
    </ThemeContext.Provider>
  )

  return { ...context, history }
}

afterEach(() => axios._reset())

describe(SignUp, () => {
  it("renders correct inputs", () => {
    const { getByLabelText } = render()

    expect(getByLabelText("Navn")).toBeInTheDocument()
    expect(getByLabelText("E-mail")).toBeInTheDocument()
    expect(getByLabelText("Telefon")).toBeInTheDocument()
    expect(getByLabelText("Vælg din enhed")).toBeInTheDocument()
    expect(
      getByLabelText(
        "Jeg accepterer, at Live opkaldet bliver optaget og gemt, så længe sagen er aktiv."
      )
    ).toBeInTheDocument()
    expect(
      getByLabelText("Jeg accepterer Care1's privatlivspolitik.")
    ).toBeInTheDocument()
  })

  test("Privacy policy link is correct", () => {
    const { getByText } = render()

    const link = getByText("privatlivspolitik")

    expect(link.target).toBe("_blank")
    expect(link.href).toBe("https://care1.dk/om-care1/privatlivspolitik/")
  })

  it("should render submit button 'Videre'", () => {
    const { getByRole } = render()

    expect(getByRole("button")).toHaveTextContent("Videre")
  })

  describe("when submitting the form", () => {
    it("posts a report ticket and redirects to /care1/waiting-room/:supportTicketId", async () => {
      axios.post.mockResolvedValue({ data: { supportTicketId: 1234 } })

      const { getByLabelText, getByText, history } = render()

      userEvent.type(getByLabelText("Navn"), "Name")
      userEvent.type(getByLabelText("E-mail"), "Em@a.il")
      userEvent.type(getByLabelText("Telefon"), "12345678")
      userEvent.selectOptions(getByLabelText("Vælg din enhed"), "apple")
      userEvent.click(
        getByLabelText(
          "Jeg accepterer, at Live opkaldet bliver optaget og gemt, så længe sagen er aktiv."
        )
      )
      userEvent.click(
        getByLabelText("Jeg accepterer Care1's privatlivspolitik.")
      )

      userEvent.click(getByText("Videre"))

      expect(axios.post).toHaveBeenCalledWith("/api/signup", {
        CustomerName: "Name",
        CustomerEmail: "Em@a.il",
        CustomerPhone: "12345678",
        Brand: "apple",
        RecordMeConsent: true,
        PrivacyConsent: true,
      })

      await waitFor(() =>
        expect(history.location.pathname).toBe("/care1/waiting-room/1234")
      )
    })
  })
})
