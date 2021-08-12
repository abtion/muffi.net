import React, { useMemo, useCallback } from "react"
import axios from "axios"

import useMappedRecords from "~/hooks/useMappedRecords"
import AuthorizedLayout from "~/components/TechLayout"
import ExampleForm from "~/components/ExampleForm"
import ExampleTable from "~/components/ExampleTable"

import useHub from "~/hooks/useHub"
import { HttpTransportType } from "@microsoft/signalr"

export default function AuthorizedHome({ accessToken }) {
  const [exampleEntityMap, upsertExampleEntity, deleteExampleEntity] =
    useMappedRecords((exampleEntity) => exampleEntity.Id)

  const exampleEntityTables = useMemo(() => {
    const result = {
      named: [],
      unnamed: [],
    }

    Object.values(exampleEntityMap).forEach((exampleEntity) => {
      const isNamed = Boolean(exampleEntity.Name)
      const table = isNamed ? result.named : result.unnamed

      table.push(exampleEntity)
    })

    return result
  }, [exampleEntityMap])

  const onHubConnected = useCallback(
    (connection) => {
      connection.on("SomeEntityCreated", ({ message }) => {
        console.log("SomeEntityCreated", message)
        upsertExampleEntity(message)
      })

      connection.on("SomeEntityUpdated", ({ message }) => {
        console.log("SomeEntityUpdated", message)
        upsertExampleEntity(message)
      })

      connection.on("SomeEntityDeleted", ({ message }) => {
        console.log("SomeEntityDeleted", message)
        deleteExampleEntity({ message })
      })
    },
    [upsertExampleEntity, deleteExampleEntity]
  )

  const connectionOptions = useMemo(
    () => ({
      accessTokenFactory: () => accessToken,
      transport: HttpTransportType.LongPolling,
    }),
    [accessToken]
  )
  useHub("/hubs/example", onHubConnected, connectionOptions)

  const handleSubmit = (e) => {
    e.preventDefault()
    createExampleEntity()
  }

  const createExampleEntity = (formData) => {
    return axios
      .post("/api/example", formData, {
        headers: {
          authorization: `Bearer ${accessToken}`,
        },
      })
      .then((response) => {
        console.log(response)
      })
      .catch((error) => {
        console.log(error)
      })
  }

  return (
    <AuthorizedLayout>
      <div className="container mt-5">
        <ExampleForm onSubmit={handleSubmit} />

        <h2 className="text-xl mb-2">Named</h2>
        <ExampleTable entities={exampleEntityTables.named} />

        <h2 className="text-xl mb-2 mt-8">Unnamed</h2>
        <ExampleTable entities={exampleEntityTables.unnamed} />
      </div>
    </AuthorizedLayout>
  )
}
