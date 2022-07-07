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
    it("posts an exampleEntity", async () => {
      const { getByLabelText, getByText } = render()

      await userEvent.type(getByLabelText("Name"), "Name")
      await userEvent.type(getByLabelText("Description"), "Description")
      await userEvent.type(getByLabelText("E-mail"), "Em@a.il")
      await userEvent.type(getByLabelText("Phone"), "12345678")

      await userEvent.click(getByText("Submit"))

      expect(onSubmit).toHaveBeenCalledTimes(1)
    })
  })
})
