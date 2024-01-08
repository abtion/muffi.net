import { act, render, waitFor } from "@testing-library/react"
import axios from "axios"
import userEvent from "@testing-library/user-event"
import { MemoryRouter } from "react-router"

import AuthorizedHome from "./"

import useHub from "~/hooks/useHub"
import AxiosMock from "../../../__mocks__/axios"
import { UseHubMock } from "~/hooks/__mocks__/useHub"
import ApiContext from "~/contexts/ApiContext"
import { AuthContext } from "react-oidc-context"

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

function renderPage() {
  const userData = {
    // eslint-disable-next-line camelcase
    id_token: "1234",
  }

  const context = render(
    <MemoryRouter initialEntries={[`/admin`]}>
      <ApiContext.Provider value={mockedAxios}>
        <AuthContext.Provider
          // @ts-expect-error No need to supply a full set of user data
          value={{ userData }}
        >
          <AuthorizedHome />
        </AuthContext.Provider>
      </ApiContext.Provider>
    </MemoryRouter>,
  )

  return { ...context }
}

describe(AuthorizedHome, () => {
  it("renders two tables", async () => {
    const { findAllByRole } = renderPage()
    expect(await findAllByRole("table")).toHaveLength(2)
  })

  describe("when submitting the form ", () => {
    it("posts an unnamed exampleEntity to authorized endpoint", async () => {
      mockedAxios.put.mockResolvedValue({ data: { Id: "1234" } })

      const { findByLabelText, findByText } = renderPage()

      await userEvent.type(await findByLabelText("Description"), "Description")
      await userEvent.type(await findByLabelText("E-mail"), "Em@a.il")
      await userEvent.type(await findByLabelText("Phone"), "12345678")
      await userEvent.click(await findByText("Submit"))

      await waitFor(() =>
        expect(mockedAxios.put).toHaveBeenCalledWith("/api/authorizedexample", {
          Name: "",
          Description: "Description",
          Email: "Em@a.il",
          Phone: "12345678",
        }),
      )
    })
  })

  describe("when clicking remove ", () => {
    it("calls backend to remove", async () => {
      const { findAllByRole } = renderPage()

      await waitFor(() =>
        expect(mockedAxios.get).toHaveBeenCalledWith(
          "/api/authorizedexample/get-all",
        ),
      )

      const removeBtn = (
        await findAllByRole("button", {
          name: /Remove/i,
        })
      )[0]

      await userEvent.click(removeBtn)

      await waitFor(() =>
        expect(mockedAxios.post).toHaveBeenCalledWith(
          "/api/authorizedexample",
          { id: "1" },
        ),
      )
    })
  })

  describe("when entering page", () => {
    it("gets all current entities", async () => {
      const { findByText } = renderPage()

      await waitFor(() =>
        expect(mockedAxios.get).toHaveBeenCalledWith(
          "/api/authorizedexample/get-all",
        ),
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
      const { findByText } = renderPage()

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
      const { findByText, queryByText } = renderPage()
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
      const { findByText } = renderPage()

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
      renderPage()

      const useHubParams = mockedUseHub.mock.calls[0]
      const [_path, _onConnected, connectionOptions] = useHubParams

      waitFor(() => {
        expect(connectionOptions.accessTokenFactory()).toEqual("1234")
      })
    })
  })
})
