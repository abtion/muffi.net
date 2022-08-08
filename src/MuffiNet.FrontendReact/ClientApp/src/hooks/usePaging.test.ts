import { renderHook, act } from "@testing-library/react-hooks"
import { usePaging } from "./usePaging"

interface Row {
  name: string
}

const rows : Row[] = [
  { name: "Bob 1" },
  { name: "Bob 2" },
  { name: "Bob 3" },
  { name: "Bob 4" },
  { name: "Bob 5" },
]

describe(usePaging, () => {
  it("can fetch pages", async () => {
    const callback = jest.fn(async (page: number, pageSize: number) => {
      const offset = (page - 1) * pageSize;

      return {
        rows: rows.slice(offset, offset + pageSize),
        totalRows: rows.length
      };
    });

    const { result, waitFor } = renderHook(() => usePaging(callback, 3))

    await waitFor(() => result.current.rows !== null);

    expect(callback).toHaveBeenCalledTimes(1)

    // TODO fix these tests

    // act(() => {
    // });

  })
})
