/* eslint-disable default-case */
import { useReducer, useCallback } from "react"

interface KeyFinder {
  (record: unknown): string | number
}

const defaultKeyFinder = (entity: { id: string }) => entity.id

enum Actions {
  Upsert = "Upsert",
  Delete = "Delete",
}

export interface RecordMap {
  [key: string]: unknown
}

interface RecordAction {
  type: Actions
  records: [unknown]
}

const createReducer =
  (keyFinder: KeyFinder) =>
  (map: RecordMap, { type, records }: RecordAction) => {
    switch (type) {
      case Actions.Upsert: {
        const updatedMap = { ...map }

        records.forEach((record: unknown) => {
          const key = keyFinder(record)
          updatedMap[key] = record
        })

        return updatedMap
      }

      case Actions.Delete: {
        const updatedMap = { ...map }

        records.forEach((record: unknown) => {
          const key = keyFinder(record)
          delete updatedMap[key]
        })

        return updatedMap
      }
    }
  }

interface RecordUpserter {
  (records: unknown | [unknown]): void
}

interface RecordDeleter {
  (records: unknown | [unknown]): void
}

export default function useMappedRecords(
  keyFinder: KeyFinder = defaultKeyFinder
): [RecordMap, RecordUpserter, RecordDeleter] {
  const [recordMap, dispatch] = useReducer(createReducer(keyFinder), {})

  const upsertRecords: RecordUpserter = useCallback(
    (records) =>
      dispatch({
        type: Actions.Upsert,
        records: [].concat(records) as [unknown],
      }),
    [dispatch]
  )
  const deleteRecord: RecordDeleter = useCallback(
    (records) =>
      dispatch({
        type: Actions.Delete,
        records: [].concat(records) as [unknown],
      }),
    [dispatch]
  )

  return [recordMap, upsertRecords, deleteRecord]
}
