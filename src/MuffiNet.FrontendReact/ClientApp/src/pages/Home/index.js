import React, { useMemo, useCallback, useEffect } from "react"
import axios from "axios"

import Layout from "~/components/Layout"

import useMappedRecords from "~/hooks/useMappedRecords"
import ExampleForm from "~/components/ExampleForm"
import ExampleTable from "~/components/ExampleTable"
import useHub from "~/hooks/useHub"

export default function Home() {
  const [exampleEntityMap, upsertExampleEntity, deleteExampleEntity] =
    useMappedRecords((exampleEntity) => exampleEntity.id)

  const exampleEntityTables = useMemo(() => {
    const result = {
      named: [],
      unnamed: [],
    }
    Object.values(exampleEntityMap).forEach((exampleEntity) => {
      const table = exampleEntity.name ? result.named : result.unnamed
      table.push(exampleEntity)
    })
    return result
  }, [exampleEntityMap])

  useEffect(() => {
    axios.get("/api/example/all").then((response) => {
      const { exampleEntities } = response.data
      upsertExampleEntity(exampleEntities)
    })
  }, [upsertExampleEntity])

  const onHubConnected = useCallback(
    (connection) => {
      connection.on("SomeEntityCreated", (message) => {
        upsertExampleEntity(message.entity)
      })
      connection.on("SomeEntityUpdated", (message) => {
        upsertExampleEntity(message.entity)
      })
      connection.on("SomeEntityDeleted", (message) => {
        deleteExampleEntity({ id: message.entityId })
      })
    },
    [upsertExampleEntity, deleteExampleEntity]
  )
  useHub("/hubs/example", onHubConnected)

  const createExampleEntity = (formData) => {
    return axios.put("/api/example", formData).then((response) => {
      // console.log(response)
    })
  }

  const removeExampleEntity = (id) => {
    return axios.post("/api/example", { id: id }).then((response) => {
      // console.log(response)
    })
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
