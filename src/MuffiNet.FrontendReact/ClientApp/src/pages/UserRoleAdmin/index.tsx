import React, { useEffect, useContext, useState, useMemo } from "react"
import AuthorizedLayout from "~/components/AuthorizedLayout"
import Loader from "~/components/Loader"
import ApiContext from "~/contexts/ApiContext"

/**
 * Users, Roles and Permissions (provided by the server)
 */
interface UserRoleData {
  roles: Array<{
    id: number
    name: string
  }>
  users: Array<{
    name: string
    userID: string
    appRoleIDs: Array<string>
  }>
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

  const userRoles = data?.users.map((u) => {
    return (
      <tr key={u.userID}>
        <td>{u.name}</td>
        <td>{u.appRoleIDs.map((id) => roleNames[id]).join()}</td>
      </tr>
    )
  })

  // Fix inline style //
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
          <table style={{ border: "1px solid black" }}>
            <thead>
              <tr>
                <th>Name</th>
                <th>Role(s)</th>
              </tr>
            </thead>
            <tbody>{userRoles}</tbody>
          </table>
        </div>
      ) : (
        <Loader />
      )}
    </AuthorizedLayout>
  )
}
