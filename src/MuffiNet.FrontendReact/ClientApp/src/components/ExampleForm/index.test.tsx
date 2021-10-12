import React from "react"
import { render as tlRender } from "@testing-library/react"
import userEvent from "@testing-library/user-event"

import axios from "axios"

import ExampleForm from "./"
import AxiosMock from "../../../__mocks__/axios"

const mockedAxios = axios as AxiosMock
const onSubmit = jest.fn()

function render() {
  const context = tlRender(<ExampleForm onSubmit={onSubmit} />)

  return { ...context }
}

afterEach(() => mockedAxios._reset())

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
      const { getByLabelText, getByText } = render()
      userEvent.type(getByLabelText("Name"), "Name")
      userEvent.type(getByLabelText("Description"), "Description")
      userEvent.type(getByLabelText("E-mail"), "Em@a.il")
      userEvent.type(getByLabelText("Phone"), "12345678")

      userEvent.click(getByText("Submit"))

      expect(onSubmit).toHaveBeenCalledTimes(1)
    })
  })
})
