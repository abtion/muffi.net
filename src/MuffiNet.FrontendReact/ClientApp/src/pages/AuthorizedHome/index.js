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

  const onHubConnected = useCallback(
    (connection) => {
      connection.on("SomeEntityCreated", (message) => {
        console.log(message)
        upsertExampleEntity(message.entity)
      })

      connection.on("SomeEntityUpdated", (message) => {
        upsertExampleEntity(message)
      })

      connection.on("SomeEntityDeleted", (message) => {
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

  const handleSubmit = (data) => {
    console.log(data)
    createExampleEntity(data)
  }

  const createExampleEntity = (formData) => {
    return axios
      .put("/api/example", formData, {
        headers: {
          authorization: `Bearer ${accessToken}`,
        },
      })
      .then((response) => {
        console.log("createExampleEntity ", response)
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
