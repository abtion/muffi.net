import React, { useMemo, useCallback, useEffect } from "react"
import axios from "axios"

import AuthorizedLayout from "~/components/AuthorizedLayout"
import ExampleForm, { ExampleFormData } from "~/components/ExampleForm"
import ExampleTable from "~/components/ExampleTable"
import { HttpTransportType, HubConnection } from "@microsoft/signalr"
import useIdMap from "~/hooks/useIdMap"
import useHub from "~/hooks/useHub"
import { ExampleEntity } from "~/types/ExampleEntity"

export default function AuthorizedHome({
  accessToken,
}: AuthorizedHomeProps): JSX.Element {
  const connectionOptions = useMemo(
    () => ({
      accessTokenFactory: () => accessToken,
      transport: HttpTransportType.LongPolling,
    }),
    [accessToken]
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
    axios
      .get(`/api/authorizedexample/get-all`, {
        headers: {
          authorization: `Bearer ${accessToken}`,
        },
      })
      .then((response) => {
        const { exampleEntities } = response.data
        upsertExampleEntity(exampleEntities)
      })
  }, [accessToken, upsertExampleEntity])

  const onHubConnected = useCallback(
    (connection: HubConnection) => {
      connection.on(
        "SomeEntityCreated",
        (message: { entity: ExampleEntity }) => {
          upsertExampleEntity(message.entity)
        }
      )
      connection.on(
        "SomeEntityUpdated",
        (message: { entity: ExampleEntity }) => {
          upsertExampleEntity(message.entity)
        }
      )
      connection.on("SomeEntityDeleted", (message: { entityId: number }) => {
        deleteExampleEntity({ id: message.entityId })
      })
    },
    [upsertExampleEntity, deleteExampleEntity]
  )
  useHub("/hubs/example", onHubConnected, connectionOptions)

  const createExampleEntity = (formData: ExampleFormData) => {
    return axios
      .put("/api/authorizedexample", formData, {
        headers: {
          authorization: `Bearer ${accessToken}`,
        },
      })
      .then((_response) => {
        // console.log(response)
      })
  }

  const removeExampleEntity = (id: string | number) => {
    return axios
      .post(
        "/api/authorizedexample",
        { id: id },
        {
          headers: {
            authorization: `Bearer ${accessToken}`,
          },
        }
      )
      .then((_response) => {
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

type AuthorizedHomeProps = {
  accessToken: string
}
