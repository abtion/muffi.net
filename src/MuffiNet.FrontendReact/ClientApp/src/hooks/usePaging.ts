import { useEffect, useMemo, useState } from "react";

type OnFetchPage<TRow> = (page: number, pageSize: number) => Promise<PageData<TRow>>;

export type PageData<TRow> = { rows: Array<TRow>, totalRows: number };

export function usePaging<TRow>(fetch: OnFetchPage<TRow>, initPageSize: number) {
  const [currentPage, _setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(initPageSize);
  const [rows, _setRows] = useState<Array<TRow> | null>(null);
  const [mostRecentPagePromise, _setMostRecentPagePromise] = useState<Promise<PageData<TRow>>>();

  const isLoading = useMemo(() => rows === null, [rows]);

  const totalPages = useMemo(() => rows === null ? 0 : Math.ceil(rows.length / pageSize), [rows]);
  
  function setCurrentPage(page: number) {
    _setCurrentPage(page);
    _setRows(null);

    const currentPagePromise = fetch(page, pageSize);

    _setMostRecentPagePromise(currentPagePromise);

    currentPagePromise.then(pageData => {
      if (currentPagePromise !== mostRecentPagePromise) {
        return; // resolved a previous promise (from a prior click for which we didn't finish)
      }

      _setRows(pageData.rows);
    })
  }

  useEffect(() => {
    setCurrentPage(1);
  }, []);

  return {
    rows,
    currentPage,
    setCurrentPage,
    pageSize,
    setPageSize,
    totalPages,
    isLoading,
  }
}
