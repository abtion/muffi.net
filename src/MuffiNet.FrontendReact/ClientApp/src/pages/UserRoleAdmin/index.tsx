import { ColumnDef, createColumnHelper } from "@tanstack/react-table"
import React, { useEffect, useContext, useState, useMemo } from "react"
import AuthorizedLayout from "~/components/AuthorizedLayout"
import Loader from "~/components/Loader"
import Pagination from "~/components/Pagination"
import ApiContext from "~/contexts/ApiContext"

/**
 * Users, Roles and Permissions (provided by the server)
 */
interface UserRoleData {
  roles: Array<{
    id: number
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

const createColumn = createColumnHelper<User>()

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

  // TODO fix this?
  // const columns = useMemo<Array<ColumnDef<User>>>(() => [
  //   createColumn.accessor("name", { cell: info => info.getValue() })
  // ], []);

  const [page, setPage] = useState(0)

  return (
    <AuthorizedLayout>
      {data ? (
        <div className="container mt-5">
          <div>Current page: {page}</div>
          <Pagination
            currentPage={page}
            totalPages={30}
            onPageChange={setPage}
          />

          <label>
            Role:
            <select>
              {data.roles.map(({ id, name }) => (
                <option key={id} value={id}>
                  {name}
                </option>
              ))}
            </select>
          </label>
          <table className="border">
            <thead>
              <tr>
                <th>Name</th>
                <th>Role(s)</th>
              </tr>
            </thead>
            <tbody>
              {data.users.map((u) => (
                <tr key={u.userID}>
                  <td>{u.name}</td>
                  <td>{u.appRoleIDs.map((id) => roleNames[id]).join()}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      ) : (
        <Loader />
      )}
    </AuthorizedLayout>
  )
}
