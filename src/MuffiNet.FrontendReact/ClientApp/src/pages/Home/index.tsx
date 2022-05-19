import React, { useMemo, useCallback, useEffect } from "react"
import axios from "axios"

import Layout from "~/components/Layout"

import useIdMap from "~/hooks/useIdMap"
import ExampleForm, { ExampleFormData } from "~/components/ExampleForm"
import ExampleTable from "~/components/ExampleTable"
import useHub from "~/hooks/useHub"
import { ExampleEntity } from "~/types/ExampleEntity"
import { HubConnection } from "@microsoft/signalr"

export default function Home(): JSX.Element {
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
    axios.get("/api/example/get-all").then((response) => {
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
  useHub("/hubs/example", onHubConnected)

  const createExampleEntity = async (formData: ExampleFormData) => {
    const _response = await axios.put("/api/example", formData)
    // console.log(_response)
  }

  const removeExampleEntity = async (id: string | number) => {
    const _response = await axios.post("/api/example", { id })
    // console.log(_response)
  }

  return (
    <Layout>
      <h1 className="text-2xl mb-3">Title</h1>

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
    </Layout>
  )
}
