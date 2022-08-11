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

  const [selectedRole, setSelectedRole] = useState<string>()

  const { currentPage, totalPages, setCurrentPage, rows } = usePaging(
    async (page, pageSize) => {
      const offset = (page - 1) * pageSize;

      return {
        rows: data?.users.slice(offset, offset + pageSize) || [],
        totalRows: data?.users.length || 0
      };
    },
    10,
    [data]
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
                       />
                       <SearchIcon className="absolute top-2 right-3 w-5 h-5" />
                    </div>
                  </div>
                  <div className="flex-initial">
                    <Select value={selectedRole} onChange={e => setSelectedRole(e.target.value)}>
                      <option selected>Filter by role</option>
                      {data.roles.map(role => <option value={role.id}>{role.name}</option>)}
                    </Select>
                  </div>
                  <div className="flex-initial">
                    <button className="text-sm">Add users</button>
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
              <Pagination
                currentPage={currentPage}
                totalPages={totalPages}
                onPageChange={setCurrentPage}
              />
            </div>
          )
        }
      </div>
    </AuthorizedLayout>
  )
}
