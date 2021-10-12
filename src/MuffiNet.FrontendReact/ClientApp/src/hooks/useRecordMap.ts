/* eslint-disable default-case */
import { useReducer, useCallback } from "react"

interface KeyFinder<Record> {
  (record: Record): string
}

enum Actions {
  Upsert = "Upsert",
  Delete = "Delete",
}

export type RecordMap<Record> = Map<string, Record>

interface UpsertAction<FullRecord> {
  type: Actions.Upsert
  records: FullRecord[]
}

interface DeleteAction<RecordWithKeyProp> {
  type: Actions.Delete
  records: RecordWithKeyProp[]
}

const createReducer =
  <FullRecord, RecordWithKeyProp>(
    keyFinder: KeyFinder<FullRecord | RecordWithKeyProp>
  ) =>
  (
    map: RecordMap<FullRecord>,
    action: UpsertAction<FullRecord> | DeleteAction<RecordWithKeyProp>
  ): RecordMap<FullRecord> => {
    switch (action.type) {
      case Actions.Upsert: {
        const updatedMap = new Map(map)

        action.records.forEach((record) => {
          const key = keyFinder(record)
          updatedMap.set(key, record)
        })

        return updatedMap
      }

      case Actions.Delete: {
        const updatedMap = new Map(map)

        action.records.forEach((record) => {
          const key = keyFinder(record)
          updatedMap.delete(key)
        })

        return updatedMap
      }
    }
  }

export interface RecordUpserter<T> {
  (records: T | T[]): void
}

export interface RecordDeleter<T> {
  (records: T | T[]): void
}

export default function useMappedRecords<
  FullRecord,
  RecordWithKeyProp = FullRecord
>(
  keyFinder: KeyFinder<FullRecord | RecordWithKeyProp>
): [
  RecordMap<FullRecord>,
  RecordUpserter<FullRecord>,
  RecordDeleter<RecordWithKeyProp>
] {
  const [recordMap, dispatch] = useReducer(
    createReducer<FullRecord, RecordWithKeyProp>(keyFinder),
    new Map()
  )

  const upsertRecords: RecordUpserter<FullRecord> = useCallback(
    (records) => {
      const recordsArray: FullRecord[] = []

      return dispatch({
        type: Actions.Upsert,
        records: recordsArray.concat(records),
      })
    },
    [dispatch]
  )
  const deleteRecord: RecordDeleter<RecordWithKeyProp> = useCallback(
    (records) => {
      const recordsArray: RecordWithKeyProp[] = []

      dispatch({
        type: Actions.Delete,
        records: recordsArray.concat(records),
      })
    },
    [dispatch]
  )

  return [recordMap, upsertRecords, deleteRecord]
}
