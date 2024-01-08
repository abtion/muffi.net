import { useMemo, useCallback, useEffect, useContext } from "react"

import AuthorizedLayout from "~/components/AuthorizedLayout"
import ExampleForm, { ExampleFormData } from "~/components/ExampleForm"
import ExampleTable from "~/components/ExampleTable"
import { HubConnection } from "@microsoft/signalr"
import useIdMap from "~/hooks/useIdMap"
import useHub from "~/hooks/useHub"
import { ExampleEntity } from "~/types/ExampleEntity"
import ApiContext from "~/contexts/ApiContext"
import { useAuth } from "react-oidc-context"

export default function AuthorizedHome(): JSX.Element {
  const { user } = useAuth()

  const idToken = user?.id_token || ""

  const api = useContext(ApiContext)

  const connectionOptions = useMemo(
    () => ({ accessTokenFactory: () => idToken }),
    [idToken],
  )

  const [exampleEntityMap, upsertExampleEntity, deleteExampleEntity] =
    useIdMap<ExampleEntity>()

  const exampleEntityTables = useMemo(() => {
    const result = {
      named: [] as ExampleEntity[],
      unnamed: [] as ExampleEntity[],
    }

    for (const exampleEntity of exampleEntityMap.values()) {
      const table = exampleEntity.name ? result.named : result.unnamed
      table.push(exampleEntity)
    }

    return result
  }, [exampleEntityMap])

  useEffect(() => {
    api.get("/api/authorizedexample/get-all").then((response) => {
      const { exampleEntities } = response.data
      upsertExampleEntity(exampleEntities)
    })
  }, [upsertExampleEntity])

  const onHubConnected = useCallback(
    (connection: HubConnection) => {
      connection.on(
        "SomeEntityCreated",
        (message: { entity: ExampleEntity }) => {
          upsertExampleEntity(message.entity)
        },
      )
      connection.on(
        "SomeEntityUpdated",
        (message: { entity: ExampleEntity }) => {
          upsertExampleEntity(message.entity)
        },
      )
      connection.on("SomeEntityDeleted", (message: { entityId: number }) => {
        deleteExampleEntity({ id: message.entityId })
      })
    },
    [upsertExampleEntity, deleteExampleEntity],
  )
  useHub("/hubs/example", onHubConnected, connectionOptions)

  const createExampleEntity = (formData: ExampleFormData) => {
    return api.put("/api/authorizedexample", formData).then((_response) => {
      // console.log(response)
    })
  }

  const removeExampleEntity = (id: string | number) => {
    return api.post("/api/authorizedexample", { id: id }).then((_response) => {
      // console.log(response)
    })
  }
  return (
    <AuthorizedLayout>
      <div className="AuthorizedHome container mt-5">
        <ExampleForm onSubmit={createExampleEntity} />

        <h2 className="text-xl mb-2">Named</h2>
        <ExampleTable
          entities={exampleEntityTables.named}
          onRemove={removeExampleEntity}
        />

        <h2 className="text-xl mb-2 mt-8">Unnamed</h2>
        <ExampleTable
          entities={exampleEntityTables.unnamed}
          onRemove={removeExampleEntity}
        />
      </div>
    </AuthorizedLayout>
  )
}
