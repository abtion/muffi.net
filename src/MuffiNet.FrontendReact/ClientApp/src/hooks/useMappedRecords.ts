/* eslint-disable default-case */
import { useReducer, useCallback } from "react"

interface KeyFinder<T> {
  (record: T): string | number
}

const defaultKeyFinder = <T>(entity: T) => entity.id as string

enum Actions {
  Upsert = "Upsert",
  Delete = "Delete",
}

export interface RecordMap<T> {
  [key: string]: T
}

interface RecordAction<T> {
  type: Actions
  records: T[]
}

const createReducer =
  <T>(keyFinder: KeyFinder<T>) =>
  (map: RecordMap<T>, { type, records }: RecordAction<T>) => {
    switch (type) {
      case Actions.Upsert: {
        const updatedMap = { ...map }

        records.forEach((record) => {
          const key = keyFinder(record)
          updatedMap[key] = record
        })

        return updatedMap
      }

      case Actions.Delete: {
        const updatedMap = { ...map }

        records.forEach((record) => {
          const key = keyFinder(record)
          delete updatedMap[key]
        })

        return updatedMap
      }
    }
  }

interface RecordUpserter<T> {
  (records: T | T[]): void
}

interface RecordDeleter<T> {
  (records: T | T[]): void
}

export default function useMappedRecords<T>(
  keyFinder: KeyFinder<T> = defaultKeyFinder
): [RecordMap<T>, RecordUpserter<T>, RecordDeleter<T>] {
  const [recordMap, dispatch] = useReducer(createReducer<T>(keyFinder), {})

  const upsertRecords: RecordUpserter<T> = useCallback(
    (records) => {
      const recordsArray: T[] = []

      return dispatch({
        type: Actions.Upsert,
        records: recordsArray.concat(records),
      })
    },
    [dispatch]
  )
  const deleteRecord: RecordDeleter<T> = useCallback(
    (records) => {
      const recordsArray: T[] = []

      dispatch({
        type: Actions.Delete,
        records: recordsArray.concat(records),
      })
    },
    [dispatch]
  )

  return [recordMap, upsertRecords, deleteRecord]
}
