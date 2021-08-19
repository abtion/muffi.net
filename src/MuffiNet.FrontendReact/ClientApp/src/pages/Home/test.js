import React from "react"
import { act, render as tlRender } from "@testing-library/react"
import userEvent from "@testing-library/user-event"
import { createMemoryHistory } from "history"
import { Router } from "react-router"

import Home from "./"
import axios from "axios"
import useHub from "~/hooks/useHub"

jest.mock("axios")
jest.mock("~/hooks/useHub")

function render() {
  const history = createMemoryHistory({
    initialEntries: [`/`],
  })

  const context = tlRender(
    <Router history={history}>
      <Home />
    </Router>
  )

  return { ...context, history }
}
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

afterEach(() => {
  axios._reset()
  useHub._reset()
})

beforeEach(() => {
  axios.get.mockResolvedValue({
    data: { exampleEntities: [entity] },
  })
})

describe(Home, () => {
  it("renders header 'Title'", () => {
    const { getByText } = render()
    expect(getByText("Title")).toBeInTheDocument()
  })

  describe("when submitting the form", () => {
    it("puts an exampleEntity", () => {
      axios.put.mockResolvedValue({ data: { Id: "1234" } })
      const { getByLabelText, getByText } = render()
      userEvent.type(getByLabelText("Name"), "Name")
      userEvent.type(getByLabelText("Description"), "Description")
      userEvent.type(getByLabelText("E-mail"), "Em@a.il")
      userEvent.type(getByLabelText("Phone"), "12345678")
      userEvent.click(getByText("Submit"))
      expect(axios.put).toHaveBeenCalledWith("/api/example", {
        Name: "Name",
        Description: "Description",
        Email: "Em@a.il",
        Phone: "12345678",
      })
    })
  })

  describe("when entering page", () => {
    it("gets all current entities", async () => {
      const { findByText } = render()

      expect(await findByText(entity.name)).toBeInTheDocument()
      expect(await findByText(entity.description)).toBeInTheDocument()
      expect(await findByText(entity.email)).toBeInTheDocument()
      expect(await findByText(entity.phone)).toBeInTheDocument()

      expect(axios.get).toHaveBeenCalledWith("/api/example/all")
    })
  })

  describe("hub connection", () => {
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
