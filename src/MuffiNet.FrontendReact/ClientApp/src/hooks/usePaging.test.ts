import { renderHook, act, waitFor } from "@testing-library/react"
import { usePaging } from "./usePaging"

interface Row {
  name: string
}

const rows: Row[] = [
  { name: "Bob 1" },
  { name: "Bob 2" },
  { name: "Bob 3" },
  { name: "Bob 4" },
  { name: "Bob 5" },
]

describe(usePaging, () => {
  it("can fetch pages", async () => {
    const callback = jest.fn(async (page: number, pageSize: number) => {
      const offset = (page - 1) * pageSize

      return {
        rows: rows.slice(offset, offset + pageSize),
        totalRows: rows.length,
      }
    })

    const initPageSize = 3

    const { result } = renderHook(() => usePaging(callback, initPageSize))

    // we initially have no rows, and a loading indicator:

    expect(result.current.rows).toStrictEqual([])

    expect(result.current.totalRows).toBe(0)

    expect(result.current.pageSize).toBe(initPageSize)

    expect(result.current.totalPages).toBe(0)

    expect(result.current.isLoading).toBe(true)

    await waitFor(() => expect(result.current.isLoading).toBe(false))

    // once the hook reports that loading is done, we should have rows:

    expect(result.current.totalPages).toBe(2)

    expect(result.current.rows.length).toBe(3) // there are 3 results on page 1

    expect(result.current.totalRows).toBe(5) // total number of rows (reported by the callback)

    // let's go to page 2:

    act(() => {
      result.current.setCurrentPage(2)
    })

    expect(result.current.isLoading).toBe(true)

    await waitFor(() => expect(result.current.isLoading).toBe(false))

    expect(result.current.rows.length).toBe(2) // there are 2 results on page 2
  })
})
