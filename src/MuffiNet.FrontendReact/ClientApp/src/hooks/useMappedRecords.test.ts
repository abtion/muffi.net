import { renderHook, act } from "@testing-library/react-hooks"
import useMappedRecords, { RecordMap } from "./useMappedRecords"

describe(useMappedRecords, () => {
  it("manages records", () => {
    const { result } = renderHook(() => useMappedRecords())
    let currentRecordMap: RecordMap

    // Add records
    act(() => {
      const [_recordMap, upsertRecords, _deleteRecord] = result.current

      upsertRecords([
        { id: "1", name: "first record" },
        { id: "2", name: "second record" },
      ])
    })

    currentRecordMap = result.current[0]
    expect(currentRecordMap).toEqual({
      1: { id: "1", name: "first record" },
      2: { id: "2", name: "second record" },
    })

    // Update records
    act(() => {
      const [_recordMap, upsertRecords, _deleteRecord] = result.current

      upsertRecords([{ id: "2", name: "second record - updated" }])
    })

    currentRecordMap = result.current[0]
    expect(currentRecordMap).toEqual({
      1: { id: "1", name: "first record" },
      2: { id: "2", name: "second record - updated" },
    })

    // Delete records
    act(() => {
      const [_recordMap, _upsertRecords, deleteRecord] = result.current

      deleteRecord([{ id: "2" }])
    })

    currentRecordMap = result.current[0]
    expect(currentRecordMap).toEqual({
      1: { id: "1", name: "first record" },
    })
  })
})
