import React from "react"
import { act, render as tlRender } from "@testing-library/react"
import axios from "axios"
import { createMemoryHistory } from "history"

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

  describe("hub connection", () => {
    const entity = {
      id: "1",
      name: "SomeRandomName",
      description: "someRandomDescription",
      email: "an@email.dk",
      phone: "12345678",
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
      const updatedEntity = {
        ...entity,
        name: "updated name",
      }

      act(() => {
        useHub.connectionMock._trigger("SomeEntityUpdated", {
          entity: updatedEntity,
        })
      })

      expect(queryByText(entity.name)).not.toBeInTheDocument()
      expect(await findByText(updatedEntity.name)).toBeInTheDocument()
    })

    // it("adds & then deletes records", async () => {
    //   const { findByText } = render()

    //   const namedEntity = {
    //     id: 1,
    //     name: "test name",
    //     description: "test description 1",
    //     email: "test1@user.com",
    //     phone: "12345678",
    //   }

    //   act(() => {
    //     useHub.connectionMock._trigger("SomeEntityCreated", {
    //       namedEntity,
    //     })
    //   })

    //   expect(await findByText(namedEntity.name)).toBeInTheDocument()

    //   act(() => {
    //     useHub.connectionMock._trigger("SomeEntityDeleted", {
    //       supportTicketId: namedEntity.id,
    //     })
    //   })

    //   expect(await findByText(namedEntity.name)).not.toBeInTheDocument()
    // })
  })
})
