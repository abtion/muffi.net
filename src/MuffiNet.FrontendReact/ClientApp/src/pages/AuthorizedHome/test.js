import React from "react"
import { act, render as tlRender } from "@testing-library/react"
import axios from "axios"
import { createMemoryHistory } from "history"
import userEvent from "@testing-library/user-event"

import AuthorizedHome from "./"
import useHub from "~/hooks/useHub"
import { Router } from "react-router-dom"

jest.mock("axios")
jest.mock("~/hooks/useHub")

afterEach(() => {
  axios._reset()
  useHub._reset()
})

function render() {
  const history = createMemoryHistory({
    initialEntries: [`/authhome`],
  })

  const context = tlRender(
    <Router history={history}>
      <AuthorizedHome />
    </Router>
  )
  return { ...context }
}

describe(AuthorizedHome, () => {
  it("renders two tables", async () => {
    const { findAllByRole } = render()
    expect(await findAllByRole("table")).toHaveLength(2)
  })

  describe("when submitting the form", () => {
    it("posts an exampleEntity", () => {
      axios.put.mockResolvedValue({ data: { Id: "1234" } })
      const { getByLabelText, getByText } = render()
      userEvent.type(getByLabelText("Name"), "Name")
      userEvent.type(getByLabelText("Description"), "Description")
      userEvent.type(getByLabelText("E-mail"), "Em@a.il")
      userEvent.type(getByLabelText("Phone"), "12345678")
      userEvent.click(getByText("Submit"))
      expect(axios.put).toHaveBeenCalledWith(
        "/api/authorizedexample",
        {
          Name: "Name",
          Description: "Description",
          Email: "Em@a.il",
          Phone: "12345678",
        },
        { headers: { authorization: "Bearer undefined" } }
      )
    })
  })

  describe("hub connection", () => {
    const entity = {
      id: "1",
      name: "SomeRandomName",
      description: "someRandomDescription",
      email: "an@email.dk",
      phone: "12345678",
    }
    const updatedEntity = {
      ...entity,
      name: "updated name",
    }

    it("adds new records", async () => {
      const { findByText } = render()

      act(() => {
        useHub.connectionMock._trigger("SomeEntityCreated", {
          entity,
        })
      })

      expect(await findByText(entity.name)).toBeInTheDocument()
      expect(await findByText(entity.description)).toBeInTheDocument()
      expect(await findByText(entity.email)).toBeInTheDocument()
      expect(await findByText(entity.phone)).toBeInTheDocument()
    })

    it("adds & then updates existing records", async () => {
      const { findByText, queryByText } = render()
      act(() => {
        useHub.connectionMock._trigger("SomeEntityCreated", {
          entity,
        })
      })

      act(() => {
        useHub.connectionMock._trigger("SomeEntityUpdated", {
          entity: updatedEntity,
        })
      })

      expect(queryByText(entity.name)).not.toBeInTheDocument()
      expect(await findByText(updatedEntity.name)).toBeInTheDocument()
    })

    it("adds & then deletes records", async () => {
      const { findByText } = render()

      act(() => {
        useHub.connectionMock._trigger("SomeEntityCreated", {
          entity,
        })
      })

      const nameElement = await findByText(entity.name)
      expect(nameElement).toBeInTheDocument()

      act(() => {
        useHub.connectionMock._trigger("SomeEntityDeleted", {
          entityId: entity.id,
        })
      })

      expect(nameElement).not.toBeInTheDocument()
    })
  })
})
