import React from "react"
import { MemoryRouter } from "react-router"
import { act, render as tlRender, waitFor } from "@testing-library/react"
import userEvent from "@testing-library/user-event"
import axios from "axios"

import Home from "."

import useHub from "~/hooks/useHub"
import AxiosMock from "../../../__mocks__/axios"
import { UseHubMock } from "~/hooks/__mocks__/useHub"

const mockedAxios = axios as AxiosMock
const mockedUseHub = useHub as UseHubMock

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
  mockedAxios._reset()
  mockedUseHub._reset()
})
beforeEach(() => {
  mockedAxios.get.mockResolvedValue({
    data: { exampleEntities: [entity, unnamedEntity] },
  })
  mockedAxios.post.mockResolvedValue({})
})

function render() {
  const context = tlRender(
    <MemoryRouter>
      <Home />
    </MemoryRouter>
  )

  return { ...context }
}

describe(Home, () => {
  it("renders header 'Title'", async () => {
    const { findByText } = render()
    expect(await findByText("Title")).toBeInTheDocument()
  })

  it("renders two tables", async () => {
    const { findAllByRole } = render()
    expect(await findAllByRole("table")).toHaveLength(2)
  })

  describe("when submitting the form ", () => {
    it("posts an exampleEntity to unauthorized endpoint", async () => {
      mockedAxios.put.mockResolvedValue({ data: { Id: "1234" } })

      const { getByLabelText, getByText } = render()

      await userEvent.type(getByLabelText("Name"), "Name")
      await userEvent.type(getByLabelText("Description"), "Description")
      await userEvent.type(getByLabelText("E-mail"), "Em@a.il")
      await userEvent.type(getByLabelText("Phone"), "12345678")

      await userEvent.click(getByText("Submit"))

      await waitFor(() =>
        expect(mockedAxios.put).toHaveBeenCalledWith("/api/example", {
          Name: "Name",
          Description: "Description",
          Email: "Em@a.il",
          Phone: "12345678",
        })
      )
    })
  })

  describe("when clicking remove ", () => {
    it("calls backend to remove", async () => {
      const { getAllByRole } = render()

      await waitFor(() =>
        expect(mockedAxios.get).toHaveBeenCalledWith("/api/example/get-all")
      )

      const removeBtn = await getAllByRole("button", {
        name: /Remove/i,
      })[0]

      await userEvent.click(removeBtn)

      await waitFor(() =>
        expect(mockedAxios.post).toHaveBeenCalledWith("/api/example", {
          id: "1",
        })
      )
    })
  })

  describe("when entering page", () => {
    it("gets all current entities", async () => {
      const { findByText } = render()

      await waitFor(() =>
        expect(mockedAxios.get).toHaveBeenCalledWith("/api/example/get-all")
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
        mockedUseHub.connectionMock._trigger("SomeEntityCreated", {
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
        mockedUseHub.connectionMock._trigger("SomeEntityCreated", {
          entity,
        })
      })

      act(() => {
        mockedUseHub.connectionMock._trigger("SomeEntityUpdated", {
          entity: updatedEntity,
        })
      })

      expect(queryByText(entity.name)).not.toBeInTheDocument()
      expect(await findByText(updatedEntity.name)).toBeInTheDocument()
    })

    it("adds & then deletes records", async () => {
      const { findByText } = render()

      act(() => {
        mockedUseHub.connectionMock._trigger("SomeEntityCreated", {
          entity,
        })
      })

      const nameElement = await findByText(entity.name)
      expect(nameElement).toBeInTheDocument()

      act(() => {
        mockedUseHub.connectionMock._trigger("SomeEntityDeleted", {
          entityId: entity.id,
        })
      })

      expect(nameElement).not.toBeInTheDocument()
    })
  })
})
