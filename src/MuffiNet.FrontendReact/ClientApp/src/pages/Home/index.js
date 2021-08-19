import React, { useCallback, useMemo, useEffect } from "react"
import axios from "axios"
import useMappedRecords from "~/hooks/useMappedRecords"
import Layout from "~/components/Layout"
import ExampleForm from "~/components/ExampleForm"
import ExampleTable from "~/components/ExampleTable"
import useHub from "~/hooks/useHub"

export default function Home() {
  const [exampleEntityMap, upsertExampleEntity, deleteExampleEntity] =
    useMappedRecords((exampleEntity) => exampleEntity.id)

  const exampleEntityTable = useMemo(() => {
    const result = {
      entities: [],
    }

    Object.values(exampleEntityMap).forEach((exampleEntity) => {
      result.entities.push(exampleEntity)
    })

    return result
  }, [exampleEntityMap])

  useEffect(() => {
    axios
      .get("/api/example/all")
      .then((response) => {
        const { exampleEntities } = response.data
        upsertExampleEntity(exampleEntities)
      })
      .catch((error) => {
        console.log(error)
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
    return axios
      .put("/api/example", formData)
      .then((response) => {
        // console.log(response)
      })
      .catch((error) => {
        // console.log(error)
      })
  }

  const removeExampleEntity = (id) => {
    return axios
      .post("/api/example", { id: id })
      .then((response) => {
        // console.log(response)
      })
      .catch((error) => {
        // console.log(error)
      })
  }

  return (
    <Layout>
      <h1 className="text-2xl mb-3">Title</h1>

      <ExampleForm onSubmit={createExampleEntity} />

      <ExampleTable
        entities={exampleEntityTable.entities}
        onRemove={removeExampleEntity}
      />
    </Layout>
  )
}
