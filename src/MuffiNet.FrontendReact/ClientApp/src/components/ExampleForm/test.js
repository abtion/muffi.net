import React from "react"
import { render as tlRender } from "@testing-library/react"
import { waitFor } from "@testing-library/dom"
import userEvent from "@testing-library/user-event"
import { createMemoryHistory } from "history"
import { Router } from "react-router"

import axios from "axios"

import ExampleForm from "./"

var onSubmit = jest.fn()

function render() {
  const context = tlRender(<ExampleForm onSubmit={onSubmit} />)

  return { ...context }
}

afterEach(() => axios._reset())

describe(ExampleForm, () => {
  it("renders correct inputs", () => {
    const { getByLabelText } = render()

    expect(getByLabelText("Name")).toBeInTheDocument()
    expect(getByLabelText("Description")).toBeInTheDocument()
    expect(getByLabelText("E-mail")).toBeInTheDocument()
    expect(getByLabelText("Phone")).toBeInTheDocument()
  })

  it("should render submit button 'Submit'", () => {
    const { getByRole } = render()

    expect(getByRole("button")).toHaveTextContent("Submit")
  })

  describe("when submitting the form", () => {
    it("posts an exampleEntity", () => {
      //axios.post.mockResolvedValue({ data: { Id: "1234" } })
      const { getByLabelText, getByText } = render()
      userEvent.type(getByLabelText("Name"), "Name")
      userEvent.type(getByLabelText("Description"), "Description")
      userEvent.type(getByLabelText("E-mail"), "Em@a.il")
      userEvent.type(getByLabelText("Phone"), "12345678")
      userEvent.click(getByText("Submit"))
      //   expect(axios.post).toHaveBeenCalledWith("/api/signup", {
      //     Name: "Name",
      //     Description: "Description",
      //     Email: "Em@a.il",
      //     Phone: "12345678",
      //   })
      expect(onSubmit).toHaveBeenCalledTimes(1)
    })
  })
})
