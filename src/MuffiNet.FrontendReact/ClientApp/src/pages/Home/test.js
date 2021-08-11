import React from "react"
import { render as tlRender } from "@testing-library/react"
import userEvent from "@testing-library/user-event"
import { createMemoryHistory } from "history"
import { Router } from "react-router"

import Home from "./"
import ThemeContext from "~/contexts/ThemeContext"
import { defaultTheme } from "~/themes"

function render() {
  const history = createMemoryHistory()

  const context = tlRender(
    <ThemeContext.Provider value={defaultTheme}>
      <Router history={history}>
        <Home />
      </Router>
    </ThemeContext.Provider>
  )

  return { ...context, history }
}

test("should render 'Altid lige ved hånden'", () => {
  const { getByText } = render()

  const heading = getByText("Altid lige ved hånden")
  expect(heading).toBeInTheDocument()
})

test("should render introduction text", () => {
  const { getByText } = render()

  const el1 = getByText(
    "Få rådgivning af nogle af Danmarks dygtigste, autoriserede teknikere inden for IT- og mobiltelefoni. Vores uddannede teknikere sidder klar til at hjælpe dig, og du er blot få klik fra at få den rådgivning, du behøver!"
  )
  expect(el1).toBeInTheDocument()

  const el2 = getByText(
    "Vi anbefaler så vidt muligt ikke at benytte den enhed, det drejer sig om. Hav venligst dit serienummer eller IMEI nummer klar, hvis du alligevel benytter enheden."
  )
  expect(el2).toBeInTheDocument()
})

test("should render a continue button", () => {
  const { getByText } = render()

  const buttonEl = getByText("Start Live Service")
  expect(buttonEl).toBeInTheDocument()
})

test("Button click should lead to SignUp page", () => {
  const { getByText, history } = render()

  const buttonEl = getByText("Start Live Service")
  userEvent.click(buttonEl)

  expect(history.location.pathname).toBe("/care1/sign-up")
})
