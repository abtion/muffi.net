import React from "react"
import { act, render as tlRender } from "@testing-library/react"
import axios from "axios"

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
  const context = tlRender(
    <Router>
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
    it("adds new records", async () => {
      const { findByText } = render()

      const namedEntity = {
        id: 1,
        name: "test name",
        description: "test description 1",
        email: "test1@user.com",
        phone: "12345678",
      }

      act(() => {
        useHub.connectionMock._trigger("SomeEntityCreated", {
          namedEntity,
        })
      })

      expect(await findByText("test name")).toBeInTheDocument()
    })

    it("adds & then updates existing records", async () => {
      const { findByText } = render()

      const namedEntity = {
        id: 1,
        name: "test name",
        description: "test description 1",
        email: "test1@user.com",
        phone: "12345678",
      }

      act(() => {
        useHub.connectionMock._trigger("SomeEntityCreated", {
          namedEntity,
        })
      })
      expect(await findByText(namedEntity.name)).toBeInTheDocument()

      const updatedEntity = {
        ...namedEntity,
        name: "updated name",
      }

      act(() => {
        useHub.connectionMock._trigger("SomeEntityUpdated", {
          updatedEntity,
        })
      })

      expect(await findByText(namedEntity.name)).not.toBeInTheDocument()
      expect(await findByText(updatedEntity.name)).toBeInTheDocument()
    })

    it("adds & then deletes records", async () => {
      const { findByText } = render()

      const namedEntity = {
        id: 1,
        name: "test name",
        description: "test description 1",
        email: "test1@user.com",
        phone: "12345678",
      }

      act(() => {
        useHub.connectionMock._trigger("SomeEntityCreated", {
          namedEntity,
        })
      })

      expect(await findByText(namedEntity.name)).toBeInTheDocument()

      act(() => {
        useHub.connectionMock._trigger("SomeEntityDeleted", {
          supportTicketId: namedEntity.id,
        })
      })

      expect(await findByText(namedEntity.name)).not.toBeInTheDocument()
    })
  })
})
