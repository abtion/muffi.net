import { DependencyList, useEffect, useMemo, useState } from "react"

type OnFetchPage<TRow> = (
  page: number,
  pageSize: number
) => Promise<PageData<TRow>>

export type PageData<TRow> = { rows: Array<TRow>; totalRows: number }

export function usePaging<TRow>(
  fetch: OnFetchPage<TRow>,
  initPageSize: number,
  /**
   * Optional list of dependencies - if these change, the page number will reset to 1
   * and the data will be refreshed. You can use this to trigger table updates from
   * state changes - for example, if your table has filter options.
   */
  deps?: DependencyList
) {
  const [currentPage, _setCurrentPage] = useState(1)
  const [pageSize, setPageSize] = useState(initPageSize)
  const [data, _setData] = useState<PageData<TRow> | null>(null)
  const [dataPromise, _setDataPromise] = useState<Promise<PageData<TRow>>>()

  const isLoading = useMemo(() => data === null, [data])

  const totalPages = useMemo(
    () => (data === null ? 0 : Math.ceil(data.totalRows / pageSize)),
    [data]
  )

  function setCurrentPage(page: number) {
    _setCurrentPage(page)
    _setData(null)

    _setDataPromise(fetch(page, pageSize)) // triggers the effect below
  }

  useEffect(() => {
    let canceled = false // prevents races

    dataPromise?.then((data) => {
      if (!canceled) {
        _setData(data)
      }
    })

    return () => {
      canceled = true // if the promise gets replaced before it resolves, we cancel the _setData call above
    }
  }, [dataPromise])

  useEffect(() => {
    setCurrentPage(1) // load page 1 when the component is first created
  }, deps || [])

  return {
    rows: data?.rows || [],
    totalRows: data?.totalRows || 0,
    currentPage,
    setCurrentPage,
    pageSize,
    setPageSize,
    totalPages,
    isLoading,
  }
}
