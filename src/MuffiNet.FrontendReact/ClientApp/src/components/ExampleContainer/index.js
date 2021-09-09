import React, { useMemo, useCallback, useEffect } from "react"
import axios from "axios"

import useMappedRecords from "~/hooks/useMappedRecords"
import ExampleForm from "~/components/ExampleForm"
import ExampleTable from "~/components/ExampleTable"
import useHub from "~/hooks/useHub"

export default function ExampleContainer({ connectionOptions, accessToken }) {
  const [exampleEntityMap, upsertExampleEntity, deleteExampleEntity] =
    useMappedRecords((exampleEntity) => exampleEntity.id)
  const endpoint = accessToken ? "/api/authorizedexample" : "/api/example"

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
    axios
      .get(`${endpoint}/all`, {
        headers: {
          authorization: `Bearer ${accessToken}`,
        },
      })
      .then((response) => {
        const { exampleEntities } = response.data
        upsertExampleEntity(exampleEntities)
      })
      .catch((error) => {
        console.log(error)
      })
  }, [accessToken, upsertExampleEntity])

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
  useHub("/hubs/example", onHubConnected, connectionOptions)

  const createExampleEntity = (formData) => {
    return axios
      .put(`${endpoint}`, formData, {
        headers: {
          authorization: `Bearer ${accessToken}`,
        },
      })
      .then((response) => {
        // console.log(response)
      })
      .catch((error) => {
        // console.log(error)
      })
  }

  const removeExampleEntity = (id) => {
    return axios
      .post(
        `${endpoint}`,
        { id: id },
        {
          headers: {
            authorization: `Bearer ${accessToken}`,
          },
        }
      )
      .then((response) => {
        // console.log(response)
      })
      .catch((error) => {
        // console.log(error)
      })
  }

  return (
    <>
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
    </>
  )
}
