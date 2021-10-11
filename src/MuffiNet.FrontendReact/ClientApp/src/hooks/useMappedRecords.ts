/* eslint-disable default-case */
import { useReducer, useCallback } from "react"

interface IdWise {
  id: string | number
}

interface KeyFinder<T> {
  (record: T): string | number
}

const defaultKeyFinder = <T extends IdWise>(entity: T) => entity.id as string

export interface RecordMap<T> {
  [key: string]: T
}

class UpsertAction<T> {
  records: T[]

  constructor(records: T[]) {
    this.records = records
  }
}

class DeleteAction {
  keys: string[]

  constructor(keys: string[]) {
    this.keys = keys
  }
}

const createReducer =
  <T>(keyFinder: KeyFinder<T>) =>
  (map: RecordMap<T>, action: UpsertAction<T> | DeleteAction): RecordMap<T> => {
    if (action instanceof UpsertAction) {
      const updatedMap = { ...map }

      action.records.forEach((record) => {
        const key = keyFinder(record)
        updatedMap[key] = record
      })

      return updatedMap
    } else if (action instanceof DeleteAction) {
      const updatedMap = { ...map }

      action.keys.forEach((key) => {
        delete updatedMap[key]
      })

      return updatedMap
    }

    return map
  }

interface RecordUpserter<T> {
  (records: T | T[]): void
}

interface RecordDeleter {
  (keys: string | string[]): void
}

export default function useMappedRecords<T extends IdWise>(
  keyFinder: KeyFinder<T> = defaultKeyFinder
): [RecordMap<T>, RecordUpserter<T>, RecordDeleter] {
  const [recordMap, dispatch] = useReducer(createReducer<T>(keyFinder), {})

  const upsertRecords: RecordUpserter<T> = useCallback(
    (records) => {
      const recordsArray: T[] = []

      return dispatch(new UpsertAction(recordsArray.concat(records)))
    },
    [dispatch]
  )
  const deleteRecord: RecordDeleter = useCallback(
    (keys) => {
      const keysArray: string[] = []

      return dispatch(new DeleteAction(keysArray.concat(keys)))
    },
    [dispatch]
  )

  return [recordMap, upsertRecords, deleteRecord]
}
