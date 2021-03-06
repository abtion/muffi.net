import React from "react"
import { act, render as tlRender, waitFor } from "@testing-library/react"
import axios from "axios"
import userEvent from "@testing-library/user-event"
import { MemoryRouter } from "react-router"

import AuthorizedHome from "./"

import useHub from "~/hooks/useHub"
import AxiosMock from "../../../__mocks__/axios"
import { UseHubMock } from "~/hooks/__mocks__/useHub"
import ApiContext from "~/contexts/ApiContext"
import { AuthContext } from "oidc-react"

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
  const userData = {
    // eslint-disable-next-line camelcase
    id_token: "1234",
  }

  const context = tlRender(
    <MemoryRouter initialEntries={[`/admin`]}>
      <ApiContext.Provider value={mockedAxios}>
        <AuthContext.Provider
          // @ts-expect-error No need to supply a full set of user data
          value={{ userData }}
        >
          <AuthorizedHome />
        </AuthContext.Provider>
      </ApiContext.Provider>
    </MemoryRouter>
  )

  return { ...context }
}

describe(AuthorizedHome, () => {
  it("renders two tables", async () => {
    const { findAllByRole } = render()
    expect(await findAllByRole("table")).toHaveLength(2)
  })

  describe("when submitting the form ", () => {
    it("posts an unnamed exampleEntity to authorized endpoint", async () => {
      mockedAxios.put.mockResolvedValue({ data: { Id: "1234" } })

      const { getByLabelText, getByText } = render()

      await userEvent.type(getByLabelText("Description"), "Description")
      await userEvent.type(getByLabelText("E-mail"), "Em@a.il")
      await userEvent.type(getByLabelText("Phone"), "12345678")
      await userEvent.click(getByText("Submit"))

      await waitFor(() =>
        expect(mockedAxios.put).toHaveBeenCalledWith("/api/authorizedexample", {
          Name: "",
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
        expect(mockedAxios.get).toHaveBeenCalledWith(
          "/api/authorizedexample/get-all"
        )
      )

      const removeBtn = await getAllByRole("button", {
        name: /Remove/i,
      })[0]

      await userEvent.click(removeBtn)

      await waitFor(() =>
        expect(mockedAxios.post).toHaveBeenCalledWith(
          "/api/authorizedexample",
          { id: "1" }
        )
      )
    })
  })

  describe("when entering page", () => {
    it("gets all current entities", async () => {
      const { findByText } = render()

      await waitFor(() =>
        expect(mockedAxios.get).toHaveBeenCalledWith(
          "/api/authorizedexample/get-all"
        )
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

    it("uses the right access token", async () => {
      const { findByText } = render()

      // Wait for component to finish loading to prevent "not wrapped in act" error
      await findByText(entity.name)

      const useHubParams = mockedUseHub.mock.calls[0]
      const [_path, _onConnected, connectionOptions] = useHubParams

      expect(connectionOptions.accessTokenFactory()).toEqual("1234")
    })
  })
})
