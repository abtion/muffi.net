/* eslint-disable default-case */
import { useReducer, useCallback } from "react"

interface KeyFinder<TRecord> {
  (record: TRecord): string
}

enum Actions {
  Upsert = "Upsert",
  Delete = "Delete",
}

export type RecordMap<Record> = Map<string, Record>

interface UpsertAction<TFullRecord> {
  type: Actions.Upsert
  records: TFullRecord[]
}

interface DeleteAction<TRecordWithKeyProp> {
  type: Actions.Delete
  records: TRecordWithKeyProp[]
}

function createReducer<TFullRecord, TRecordWithKeyProp>(
  keyFinder: KeyFinder<TFullRecord | TRecordWithKeyProp>,
) {
  return (
    map: RecordMap<TFullRecord>,
    action: UpsertAction<TFullRecord> | DeleteAction<TRecordWithKeyProp>,
  ): RecordMap<TFullRecord> => {
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
}

export interface RecordUpserter<T> {
  (records: T | T[]): void
}

export interface RecordDeleter<T> {
  (records: T | T[]): void
}

export default function useMappedRecords<
  TFullRecord,
  TRecordWithKeyProp = TFullRecord,
>(
  keyFinder: KeyFinder<TFullRecord | TRecordWithKeyProp>,
): [
  RecordMap<TFullRecord>,
  RecordUpserter<TFullRecord>,
  RecordDeleter<TRecordWithKeyProp>,
] {
  const [recordMap, dispatch] = useReducer(
    createReducer<TFullRecord, TRecordWithKeyProp>(keyFinder),
    new Map(),
  )

  const upsertRecords: RecordUpserter<TFullRecord> = useCallback(
    (records) => {
      const recordsArray: TFullRecord[] = []

      return dispatch({
        type: Actions.Upsert,
        records: recordsArray.concat(records),
      })
    },
    [dispatch],
  )
  const deleteRecord: RecordDeleter<TRecordWithKeyProp> = useCallback(
    (records) => {
      const recordsArray: TRecordWithKeyProp[] = []

      dispatch({
        type: Actions.Delete,
        records: recordsArray.concat(records),
      })
    },
    [dispatch],
  )

  return [recordMap, upsertRecords, deleteRecord]
}
