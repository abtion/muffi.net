import React, { useCallback } from "react"
import axios from "axios"

import Layout from "~/components/Layout"
import ExampleForm from "~/components/ExampleForm"
import useHub from "~/hooks/useHub"

export default function Home() {
  const onHubConnected = useCallback((connection) => {
    connection.on("SomeEntityCreated", ({ message }) => {
      console.log("SomeEntityCreated", message)
    })

    connection.on("SomeEntityUpdated", ({ message }) => {
      console.log("SomeEntityUpdated", message)
    })

    connection.on("SomeEntityDeleted", ({ message }) => {
      console.log("SomeEntityDeleted", message)
    })
  }, [])
  useHub("/hubs/example", onHubConnected)

  const handleSubmit = (e) => {
    e.preventDefault()
    createExampleEntity()
  }

  const createExampleEntity = (formData) => {
    return axios
      .post("/api/example", formData)
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

      <ExampleForm onSubmit={handleSubmit} />
    </Layout>
  )
}
