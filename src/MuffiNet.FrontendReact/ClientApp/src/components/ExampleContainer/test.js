import React from "react"
import { act, render as tlRender, waitFor } from "@testing-library/react"
import axios from "axios"
import userEvent from "@testing-library/user-event"

import ExampleContainer from "./"
import useHub from "~/hooks/useHub"

jest.mock("axios")
jest.mock("~/hooks/useHub")

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

const unnamedEntity = {
  id: "2",
  name: undefined,
  description: "something",
  email: "some@email.dk",
  phone: "9999999",
}

afterEach(() => {
  axios._reset()
  useHub._reset()
})
beforeEach(() => {
  axios.get.mockResolvedValue({
    data: { exampleEntities: [entity, unnamedEntity] },
  })
  axios.post.mockResolvedValue({})
})
function render(accessToken) {
  const context = tlRender(<ExampleContainer accessToken={accessToken} />)
  return { ...context }
}

describe(ExampleContainer, () => {
  it("renders two tables", async () => {
    const { findAllByRole } = render()
    expect(await findAllByRole("table")).toHaveLength(2)
  })

  describe("when submitting the form ", () => {
    it("posts an exampleEntity to unauthorized endpoint", async () => {
      axios.put.mockResolvedValue({ data: { Id: "1234" } })
      const { getByLabelText, getByText } = render()
      userEvent.type(getByLabelText("Name"), "Name")
      userEvent.type(getByLabelText("Description"), "Description")
      userEvent.type(getByLabelText("E-mail"), "Em@a.il")
      userEvent.type(getByLabelText("Phone"), "12345678")
      userEvent.click(getByText("Submit"))
      await waitFor(() =>
        expect(axios.put).toHaveBeenCalledWith(
          "/api/example",
          {
            Name: "Name",
            Description: "Description",
            Email: "Em@a.il",
            Phone: "12345678",
          },
          { headers: { authorization: "Bearer undefined" } }
        )
      )
    })

    it("posts an unnamed exampleEntity to authorized endpoint", async () => {
      axios.put.mockResolvedValue({ data: { Id: "1234" } })
      const { getByLabelText, getByText } = render("accessToken")
      userEvent.type(getByLabelText("Description"), "Description")
      userEvent.type(getByLabelText("E-mail"), "Em@a.il")
      userEvent.type(getByLabelText("Phone"), "12345678")
      userEvent.click(getByText("Submit"))
      await waitFor(() =>
        expect(axios.put).toHaveBeenCalledWith(
          "/api/authorizedexample",
          {
            Name: "",
            Description: "Description",
            Email: "Em@a.il",
            Phone: "12345678",
          },
          { headers: { authorization: "Bearer accessToken" } }
        )
      )
    })
  })

  describe("when clicking remove ", () => {
    it("calls backend to remove", async () => {
      const { getAllByRole } = render()

      await waitFor(() =>
        expect(axios.get).toHaveBeenCalledWith("/api/example/all", {
          headers: { authorization: "Bearer undefined" },
        })
      )

      const removeBtn = await getAllByRole("button", {
        name: /Remove/i,
      })[0]
      userEvent.click(removeBtn)
      await waitFor(() =>
        expect(axios.post).toHaveBeenCalledWith(
          "/api/example",
          { id: "1" },
          {
            headers: { authorization: "Bearer undefined" },
          }
        )
      )
    })
  })

  describe("when entering page", () => {
    it("gets all current entities", async () => {
      const { findByText } = render()

      await waitFor(() =>
        expect(axios.get).toHaveBeenCalledWith("/api/example/all", {
          headers: { authorization: "Bearer undefined" },
        })
      )

      expect(await findByText(entity.name)).toBeInTheDocument()
      expect(await findByText(entity.description)).toBeInTheDocument()
      expect(await findByText(entity.email)).toBeInTheDocument()
      expect(await findByText(entity.phone)).toBeInTheDocument()

      expect(await findByText(unnamedEntity.description)).toBeInTheDocument()
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
