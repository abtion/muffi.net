import React, { useCallback, useMemo } from "react"
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

  const onHubConnected = useCallback(
    (connection) => {
      connection.on("SomeEntityCreated", (message) => {
        upsertExampleEntity(message.entity)
      })
      // connection.on("SomeEntityUpdated", (message) => {
      //   upsertExampleEntity(message)
      // })
      // connection.on("SomeEntityDeleted", (message) => {
      //   deleteExampleEntity({ message })
      // })
    },
    [upsertExampleEntity, deleteExampleEntity]
  )

  useHub("/hubs/example", onHubConnected)

  const handleSubmit = (formData) => {
    createExampleEntity(formData)
  }

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

  return (
    <Layout>
      <h1 className="text-2xl mb-3">Title</h1>

      <ExampleTable entities={exampleEntityTable.entities} />

      <ExampleForm onSubmit={handleSubmit} />
    </Layout>
  )
}
