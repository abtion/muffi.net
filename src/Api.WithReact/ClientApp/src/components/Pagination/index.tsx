import classNames from "classnames"
import React from "react"
import ArrowLeft from "bootstrap-icons/icons/chevron-left.svg"
import ArrowRight from "bootstrap-icons/icons/chevron-right.svg"

import "./style.scss"

export type OnChangePage = (page: number) => void

export const Spacer = Symbol("...")

interface PaginationProps {
  currentPage: number
  totalPages: number

  /**
   * Range of page buttons (in both directions) before/after `currentPage`
   */
  spread?: number
  onPageChange: OnChangePage
  className?: string
}

export default function Pagination({
  currentPage,
  totalPages,
  spread = 2,
  onPageChange,
  className,
}: PaginationProps): JSX.Element {
  const pages = getPages(currentPage, totalPages, spread)

  return (
    <div className={classNames("Pagination", className)} role="navigation">
      <button
        onClick={() => onPageChange(currentPage - 1)}
        disabled={currentPage === 1}
      >
        <ArrowLeft />
      </button>
      {pages.map((page, index) =>
        page === Spacer ? (
          <button key={index}>...</button>
        ) : page === currentPage ? (
          <button key={index} aria-selected="true">
            {page}
          </button>
        ) : (
          <button key={index} onClick={() => onPageChange(page)}>
            {page}
          </button>
        )
      )}
      <button
        onClick={() => onPageChange(currentPage + 1)}
        disabled={currentPage === totalPages}
      >
        <ArrowRight />
      </button>
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
