import React from "react"

export type OnChangePage = (page: number) => void

export const Spacer = Symbol("...")

interface PaginationProps {
  currentPage: number
  totalPages: number
  onPageChange: OnChangePage

  /**
   * Range of page buttons (in both directions) before/after `currentPage`
   */
  spread?: number
}

export default function Pagination({
  currentPage,
  totalPages,
  spread = 2,
  onPageChange,
}: PaginationProps): JSX.Element {
  const pages = getPages(currentPage, totalPages, spread)

  return (
    <div>
      {pages.map((page) =>
        page === Spacer ? (
          <span>...</span>
        ) : page === currentPage ? (
          <span>{page}</span>
        ) : (
          <span onClick={() => onPageChange(page)}>{page}</span>
        )
      )}
    </div>
  )
}

export function getPages(
  currentPage: number,
  totalPages: number,
  spread: number
): Array<number | typeof Spacer> {
  if (totalPages === 0) {
    return []
  }

  const range = spread * 2 + 1

  if (totalPages <= range + 4) {
    // full range:
    return fill(1, totalPages)
  }

  const left = currentPage - spread
  const right = currentPage + spread

  if (left <= spread + 1) {
    // left range:
    return [...fill(1, Math.max(range, right)), Spacer, totalPages]
  }

  if (right >= totalPages - spread) {
    // right range:
    return [
      1,
      Spacer,
      ...fill(Math.min(Math.max(left, 1), totalPages - range), totalPages),
    ]
  }

  // middle range:
  return [1, Spacer, ...fill(left, right), Spacer, totalPages]
}

function fill(start: number, end: number) {
  return new Array(end - start + 1).fill(0).map((_, i) => i + start)
}
