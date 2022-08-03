import React, { useEffect, useContext, useState } from "react"
import AuthorizedLayout from "~/components/AuthorizedLayout"
import Loader from "~/components/Loader";
import ApiContext from "~/contexts/ApiContext"

/**
 * Users, Roles and Permissions (provided by the server)
 */
interface UserRoleData {
  roles: Array<{
    id: number;
    name: string;
  }>
}

// TODO add users, build out proper UI

export default function UserRoleAdmin(): JSX.Element {
  const api = useContext(ApiContext)

  const [data, setData] = useState<UserRoleData>();

  useEffect(() => {
    api.get<UserRoleData>("/api/roleAdmin/get-data")
      .then(response => { setData(response.data) })
  }, [])

  return (
    <AuthorizedLayout>
      <div className="container mt-5">
        {data
          ? (
            <label>
              Role:
              <select>
                {data.roles.map(({ id, name }) => <option key={id} value={id}>{name}</option>)}
              </select>
            </label>
          )
          : <Loader/>}
      </div>
    </AuthorizedLayout>
  )
}
