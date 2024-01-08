import { getPages, Spacer } from "."

describe(getPages, () => {
  it("produces meaningful page ranges", async () => {
    expectRanges(1, 1, 2, "[1]")
    expectRanges(1, 2, 2, "[1] 2")
    expectRanges(2, 2, 2, "1 [2]")

    expectRanges(1, 3, 2, "[1] 2 3")
    expectRanges(2, 3, 2, "1 [2] 3")
    expectRanges(3, 3, 2, "1 2 [3]")

    expectRanges(1, 5, 2, "[1] 2 3 4 5")
    expectRanges(2, 5, 2, "1 [2] 3 4 5")
    expectRanges(3, 5, 2, "1 2 [3] 4 5")
    expectRanges(4, 5, 2, "1 2 3 [4] 5")
    expectRanges(5, 5, 2, "1 2 3 4 [5]")

    expectRanges(1, 8, 2, "[1] 2 3 4 5 6 7 8")
    expectRanges(2, 8, 2, "1 [2] 3 4 5 6 7 8")
    expectRanges(3, 8, 2, "1 2 [3] 4 5 6 7 8")
    expectRanges(4, 8, 2, "1 2 3 [4] 5 6 7 8")
    expectRanges(5, 8, 2, "1 2 3 4 [5] 6 7 8")
    expectRanges(6, 8, 2, "1 2 3 4 5 [6] 7 8")
    expectRanges(7, 8, 2, "1 2 3 4 5 6 [7] 8")
    expectRanges(8, 8, 2, "1 2 3 4 5 6 7 [8]")

    expectRanges(1, 9, 2, "[1] 2 3 4 5 6 7 8 9")
    expectRanges(2, 9, 2, "1 [2] 3 4 5 6 7 8 9")
    expectRanges(3, 9, 2, "1 2 [3] 4 5 6 7 8 9")
    expectRanges(4, 9, 2, "1 2 3 [4] 5 6 7 8 9")
    expectRanges(5, 9, 2, "1 2 3 4 [5] 6 7 8 9")
    expectRanges(6, 9, 2, "1 2 3 4 5 [6] 7 8 9")
    expectRanges(7, 9, 2, "1 2 3 4 5 6 [7] 8 9")
    expectRanges(8, 9, 2, "1 2 3 4 5 6 7 [8] 9")
    expectRanges(9, 9, 2, "1 2 3 4 5 6 7 8 [9]")

    expectRanges(1, 10, 2, "[1] 2 3 4 5 .. 10")
    expectRanges(2, 10, 2, "1 [2] 3 4 5 .. 10")
    expectRanges(3, 10, 2, "1 2 [3] 4 5 .. 10")
    expectRanges(4, 10, 2, "1 2 3 [4] 5 6 .. 10")
    expectRanges(5, 10, 2, "1 2 3 4 [5] 6 7 .. 10")
    expectRanges(6, 10, 2, "1 .. 4 5 [6] 7 8 9 10")
    expectRanges(7, 10, 2, "1 .. 5 6 [7] 8 9 10")
    expectRanges(8, 10, 2, "1 .. 5 6 7 [8] 9 10")
    expectRanges(9, 10, 2, "1 .. 5 6 7 8 [9] 10")
    expectRanges(10, 10, 2, "1 .. 5 6 7 8 9 [10]")

    expectRanges(1, 11, 2, "[1] 2 3 4 5 .. 11")
    expectRanges(2, 11, 2, "1 [2] 3 4 5 .. 11")
    expectRanges(3, 11, 2, "1 2 [3] 4 5 .. 11")
    expectRanges(4, 11, 2, "1 2 3 [4] 5 6 .. 11")
    expectRanges(5, 11, 2, "1 2 3 4 [5] 6 7 .. 11")
    expectRanges(6, 11, 2, "1 .. 4 5 [6] 7 8 .. 11")
    expectRanges(7, 11, 2, "1 .. 5 6 [7] 8 9 10 11")
    expectRanges(8, 11, 2, "1 .. 6 7 [8] 9 10 11")
    expectRanges(9, 11, 2, "1 .. 6 7 8 [9] 10 11")
    expectRanges(10, 11, 2, "1 .. 6 7 8 9 [10] 11")
    expectRanges(11, 11, 2, "1 .. 6 7 8 9 10 [11]")

    expectRanges(1, 12, 2, "[1] 2 3 4 5 .. 12")
    expectRanges(2, 12, 2, "1 [2] 3 4 5 .. 12")
    expectRanges(3, 12, 2, "1 2 [3] 4 5 .. 12")
    expectRanges(4, 12, 2, "1 2 3 [4] 5 6 .. 12")
    expectRanges(5, 12, 2, "1 2 3 4 [5] 6 7 .. 12")
    expectRanges(6, 12, 2, "1 .. 4 5 [6] 7 8 .. 12")
    expectRanges(7, 12, 2, "1 .. 5 6 [7] 8 9 .. 12")
    expectRanges(8, 12, 2, "1 .. 6 7 [8] 9 10 11 12")
    expectRanges(9, 12, 2, "1 .. 7 8 [9] 10 11 12")
    expectRanges(10, 12, 2, "1 .. 7 8 9 [10] 11 12")
    expectRanges(11, 12, 2, "1 .. 7 8 9 10 [11] 12")
    expectRanges(12, 12, 2, "1 .. 7 8 9 10 11 [12]")

    function expectRanges(
      currentPage: number,
      totalPages: number,
      spread: number,
      expected: string,
    ) {
      const ranges = getPages(currentPage, totalPages, spread)

      const pager = ranges
        .map((page) =>
          page === Spacer ? ".." : page === currentPage ? `[${page}]` : page,
        )
        .join(" ")

      expect(pager).toBe(expected)
    }
  })
})
