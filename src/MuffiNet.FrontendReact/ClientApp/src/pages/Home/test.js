import React from "react"
import { render as tlRender } from "@testing-library/react"
import userEvent from "@testing-library/user-event"
import { createMemoryHistory } from "history"
import { Router } from "react-router"

import Home from "./"
import ThemeContext from "~/contexts/ThemeContext"
import { defaultTheme } from "~/themes"
import axios from "axios"
import useHub from "~/hooks/useHub"

jest.mock("axios")
jest.mock("~/hooks/useHub")

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

afterEach(() => {
  axios._reset()
  useHub._reset()
})

describe(Home, () => {

  it("renders header 'Title'", () => {
    const { getByText } = render()
    expect(getByText("Title")).toBeInTheDocument()
  })

  describe("when submitting the form", () => {
    it("posts an exampleEntity", () => {
      axios.post.mockResolvedValue({ data: { Id: "1234" } })
      const { getByLabelText, getByText } = render()
      userEvent.type(getByLabelText("Name"), "Name")
      userEvent.type(getByLabelText("Description"), "Description")
      userEvent.type(getByLabelText("E-mail"), "Em@a.il")
      userEvent.type(getByLabelText("Phone"), "12345678")
      userEvent.click(getByText("Submit"))
      expect(axios.post).toHaveBeenCalledWith("/api/example", {
        Name: "Name",
        Description: "Description",
        Email: "Em@a.il",
        Phone: "12345678",
      })
    })
  })
})
