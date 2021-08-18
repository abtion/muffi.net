import React, { useCallback } from "react"
import axios from "axios"
import useMappedRecords from "~/hooks/useMappedRecords"
import Layout from "~/components/Layout"
import ExampleForm from "~/components/ExampleForm"
import ExampleTable from "~/components/ExampleTable"
import useHub from "~/hooks/useHub"

export default function Home() {

  const [exampleEntityMap, upsertExampleEntity, deleteExampleEntity] =
    useMappedRecords((exampleEntity) => exampleEntity.id)
   
  const onHubConnected = useCallback((connection) => {
    connection.on("SomeEntityCreated", ({ exampleEntity }) => {
      upsertExampleEntity(exampleEntity)
      console.log("SomeEntityCreated", exampleEntity) 
    })

    connection.on("SomeEntityUpdated", ({ exampleEntity }) => {
      upsertExampleEntity(exampleEntity)
      console.log("SomeEntityUpdated", exampleEntity)
    })

    connection.on("SomeEntityDeleted", ({ exampleEntity }) => {
      deleteExampleEntity(exampleEntity)
      console.log("SomeEntityDeleted", exampleEntity)
    })
  }, [upsertExampleEntity, deleteExampleEntity])

  useHub("/hubs/example", onHubConnected)

  const handleSubmit = (formData) => {
    createExampleEntity(formData)
  }

  const createExampleEntity = (formData) => {
    return axios
      .post("/api/example/ExampleCommand", formData)
      .then((response) => {
        console.log(response)
      })
      .catch((error) => {
        console.log(error)
      })
  }

  return (
    <Layout>
      <h1 className="text-2xl mb-3">Title</h1>
      <ExampleTable entities={Object.values(exampleEntityMap)}/>
      <ExampleForm onSubmit={handleSubmit} />
    </Layout>
  )
}
