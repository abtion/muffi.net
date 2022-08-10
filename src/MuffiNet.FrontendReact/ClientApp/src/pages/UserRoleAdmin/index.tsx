import React, { useEffect, useContext, useState, useMemo } from "react"
import AuthorizedLayout from "~/components/AuthorizedLayout"
import Loader from "~/components/Loader"
import Pagination from "~/components/Pagination"
import Table from "~/components/Table"
import ApiContext from "~/contexts/ApiContext"
import { usePaging } from "~/hooks/usePaging"

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
      {data ? (
        <div className="container mt-5">
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
          <Table>
            <thead>
              <tr>
                <th>Name</th>
                <th>Role(s)</th>
              </tr>
            </thead>
            <tbody>
              {rows.map((u) => (
                <tr key={u.userID}>
                  <td>{u.name}</td>
                  <td>{u.appRoleIDs.map((id) => roleNames[id]).join()}</td>
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
      ) : (
        <Loader />
      )}
    </AuthorizedLayout>
  )
}
