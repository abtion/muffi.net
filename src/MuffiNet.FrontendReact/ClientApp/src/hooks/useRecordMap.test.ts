import { renderHook, act } from "@testing-library/react-hooks"
import useMappedRecords, { RecordMap } from "./useRecordMap"

interface Record {
  ID: string
  name: string
}

describe(useMappedRecords, () => {
  it("manages records", () => {
    const { result } = renderHook(() =>
      useMappedRecords<Record, Pick<Record, "ID">>((record) => record.ID)
    )
    let currentRecordMap: RecordMap<Record>

    // Add records
    act(() => {
      const [_recordMap, upsertRecords, _deleteRecord] = result.current

      upsertRecords([
        { ID: "1", name: "first record" },
        { ID: "2", name: "second record" },
      ])
    })

    currentRecordMap = result.current[0]
    expect(currentRecordMap).toEqual(
      new Map(
        Object.entries({
          1: { ID: "1", name: "first record" },
          2: { ID: "2", name: "second record" },
        })
      )
    )

    // Update records
    act(() => {
      const [_recordMap, upsertRecords, _deleteRecord] = result.current

      upsertRecords([{ ID: "2", name: "second record - updated" }])
    })

    currentRecordMap = result.current[0]
    expect(currentRecordMap).toEqual(
      new Map(
        Object.entries({
          1: { ID: "1", name: "first record" },
          2: { ID: "2", name: "second record - updated" },
        })
      )
    )

    // Delete records
    act(() => {
      const [_recordMap, _upsertRecords, deleteRecord] = result.current

      deleteRecord([{ ID: "2" }])
    })

    currentRecordMap = result.current[0]
    expect(currentRecordMap).toEqual(
      new Map(
        Object.entries({
          1: { ID: "1", name: "first record" },
        })
      )
    )
  })
})
