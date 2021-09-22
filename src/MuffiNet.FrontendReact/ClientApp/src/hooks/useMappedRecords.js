/* eslint-disable default-case */
import { useReducer, useCallback } from "react"

const defaultKeyFinder = (entity) => entity.id

const ACTIONS = {
  UPSERT: "UPSERT",
  DELETE: "DELETE",
}

const createReducer =
  (keyFinder) =>
  (map, { type, records }) => {
    switch (type) {
      case ACTIONS.UPSERT: {
        const updatedMap = { ...map }

        records.forEach((record) => {
          const key = keyFinder(record)
          updatedMap[key] = record
        })

        return updatedMap
      }

      case ACTIONS.DELETE: {
        const updatedMap = { ...map }

        records.forEach((record) => {
          const key = keyFinder(record)
          delete updatedMap[key]
        })

        return updatedMap
      }
    }
  }

export default function useMappedRecords(keyFinder = defaultKeyFinder) {
  const [recordMap, dispatch] = useReducer(createReducer(keyFinder), {})

  const upsertRecords = useCallback(
    (records) =>
      dispatch({ type: ACTIONS.UPSERT, records: [].concat(records) }),
    [dispatch]
  )
  const deleteRecord = useCallback(
    (records) =>
      dispatch({ type: ACTIONS.DELETE, records: [].concat(records) }),
    [dispatch]
  )

  return [recordMap, upsertRecords, deleteRecord]
}
