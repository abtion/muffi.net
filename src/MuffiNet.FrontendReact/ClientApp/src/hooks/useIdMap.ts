import useRecordMap, {
  RecordDeleter,
  RecordMap,
  RecordUpserter,
} from "./useRecordMap"

interface RecordWithJustId {
  id: string | number
}

export default function useRecordsById<FullRecord extends RecordWithJustId>(): [
  RecordMap<FullRecord>,
  RecordUpserter<FullRecord>,
  RecordDeleter<RecordWithJustId>
] {
  return useRecordMap<FullRecord, RecordWithJustId>(
    (entity: RecordWithJustId) => entity.id.toString()
  )
}
