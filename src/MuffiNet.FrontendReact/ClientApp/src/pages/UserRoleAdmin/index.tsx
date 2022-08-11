import React, { useEffect, useContext, useState, useMemo } from "react"
import AuthorizedLayout from "~/components/AuthorizedLayout"
import Loader from "~/components/Loader"
import Pagination from "~/components/Pagination"
import Table from "~/components/Table"
import ApiContext from "~/contexts/ApiContext"
import { usePaging } from "~/hooks/usePaging"
import SearchIcon from "bootstrap-icons/icons/search.svg"
import Select from "~/components/Select"

/**
 * Users, Roles and Permissions (provided by the server)
 */
interface UserRoleData {
  roles: Array<{
    id: string
    name: string
  }>
  users: Array<User>
}

interface User {
  name: string
  userID: string
  appRoleIDs: Array<string>
}

// TODO add users, build out proper UI

export default function UserRoleAdmin(): JSX.Element {
  const api = useContext(ApiContext)

  const [data, setData] = useState<UserRoleData>()

  const roleNames = useMemo(() => {
    return data
      ? Object.fromEntries(data.roles.map((role) => [role.id, role.name]))
      : {}
  }, [data])

  useEffect(() => {
    api.get<UserRoleData>("/api/roleAdmin/get-data").then((response) => {
      setData(response.data)
    })
  }, [])

  const [selectedRoleID, setSelectedRoleID] = useState<string>()

  const [searchTerms, setSearchTerms] = useState<string>("")

  const { currentPage, totalPages, totalRows, pageFirstRow, pageLastRow, setCurrentPage, rows } = usePaging(
    async (page, pageSize) => {
      const offset = (page - 1) * pageSize;

      const visibleUsers = data?.users
        .filter(row => selectedRoleID
          ? row.appRoleIDs.includes(selectedRoleID)
          : true)
        .filter(row => searchTerms
          ? row.name.toLowerCase().includes(searchTerms.toLowerCase())
          : true) || []

      return {
        rows: visibleUsers.slice(offset, offset + pageSize) || [],
        totalRows: visibleUsers.length
      };
    },
    10,
    [data, searchTerms, selectedRoleID]
  )

  return (
    <AuthorizedLayout>
      <div className="container mt-5">
        {data == null
          ? (
            <Loader />
          )
          : (
            <div>
              <h2 className="text-2xl">Users</h2>
              <div className="w-full max-w-7xl border border-b-0 border-neutral-200 h-20 rounded-t-lg mt-2">
                <div className="h-full flex items-center gap-x-2 px-4">
                  <div className="flex-auto">
                    <div className="relative w-80">
                      <input className="w-80 h-9 border border-neutral-200 rounded pl-3 text-neutral-500" 
                             type="text"
                             placeholder="Search"
                             value={searchTerms}
                             onChange={e => setSearchTerms(e.target.value)}
                       />
                       <SearchIcon className="absolute top-2 right-3 w-5 h-5" />
                    </div>
                  </div>
                  <div className="flex-initial">
                    <Select value={selectedRoleID} onChange={e => setSelectedRoleID(e.target.value)}>
                      <option key={""} value="">Filter by role</option>
                      {data.roles.map(role => <option key={role.id} value={role.id}>{role.name}</option>)}
                    </Select>
                  </div>
                  <div className="flex-initial">
                    <button className="w-24 h-9 bg-primary-700 text-sm text-light rounded">
                        Add users
                    </button>
                  </div>
                </div>
              </div>
              <Table>
                <thead>
                  <tr>
                    <th>Name</th>
                    <th>Role</th>
                    <th></th>
                  </tr>
                </thead>
                <tbody>
                  {rows.map((u) => (
                    <tr key={u.userID}>
                      <td>{u.name}</td>
                      <td>{u.appRoleIDs.map((id) => roleNames[id]).join()}</td>
                      <td><button className="float-right">Edit</button></td>
                    </tr>
                  ))}
                </tbody>
              </Table>
              <div className="w-full h-16 max-w-7xl flex px-4 items-center border border-neutral-200 rounded-b-lg">
                <div className="flex-auto leading-9 text-sm">
                  Showing {pageFirstRow} to {pageLastRow} of {totalRows}
                </div>
                <div className="flex-initial">
                  <Pagination
                    currentPage={currentPage}
                    totalPages={totalPages}
                    onPageChange={setCurrentPage}
                  />
                </div>
              </div>
            </div>
          )
        }
      </div>
    </AuthorizedLayout>
  )
}
