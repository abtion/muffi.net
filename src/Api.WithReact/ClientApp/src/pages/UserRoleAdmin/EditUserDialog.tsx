import Dialog from "~/components/Dialog"
import Input from "~/components/Input"
import { User, UserDetails, UserRoleData } from "./model"
import React, { useContext, useEffect, useState } from "react"
import ApiContext from "~/contexts/ApiContext"
import Button from "~/components/Button"
import Loader from "~/components/Loader"

interface UserEditDialogProps {
  user: User
  data: UserRoleData
  onClose: (saved: boolean) => void
}

export default function UserEditDialog({
  user: _user,
  data,
  onClose,
}: UserEditDialogProps): JSX.Element {
  const api = useContext(ApiContext)

  const [details, setDetails] = useState<UserDetails>()

  const [name, setName] = useState(_user.name)

  const [appRoleIds, setAppRoleIds] = useState(_user.appRoleIds)

  const [saving, setSaving] = useState(false)

  useEffect(() => {
    api
      .get<UserDetails>("/api/roleAdmin/user-details", {
        params: { userID: _user.userId },
      })
      .then((response) => setDetails(response.data))
  }, [])

  function toggleRole(roleId: string, enabled: boolean) {
    setAppRoleIds(
      enabled
        ? appRoleIds.concat(roleId)
        : appRoleIds.filter((id) => id !== roleId),
    )
  }

  async function saveUser() {
    setDetails(undefined)
    setSaving(true)

    await api.post("/api/roleAdmin/update-user", {
      userID: _user.userId,
      name,
      appRoleIds,
    })

    onClose(true)
  }

  async function revokeAccess() {
    setDetails(undefined)
    setSaving(true)

    await api.post("/api/roleAdmin/revoke-access", "", {
      params: { userId: _user.userId },
    })

    onClose(true)
  }

  function cancel() {
    onClose(false)
  }

  return (
    <Dialog onClose={cancel}>
      {details ? (
        <>
          <Dialog.Header title="Edit user profile">
            Change the basic information and role(s) of the user.
          </Dialog.Header>

          <Dialog.Content>
            <div className="font-bold text-lg mb-3">Info</div>
            <label className="block mb-5 text-sm">
              Name
              <Input
                size="md"
                variant="default"
                className="block w-full"
                value={name}
                onChange={(e) => setName(e.currentTarget.value)}
              />
            </label>
            <label className="block mb-5 text-sm">
              Email
              <Input
                size="md"
                variant="default"
                className="block w-full"
                readOnly
                value={details?.email}
              />
            </label>
            <div className="font-bold text-lg">Roles</div>
            {data.roles.map((role) => (
              <label key={role.id}>
                <input
                  type="checkbox"
                  checked={appRoleIds.includes(role.id)}
                  onChange={(e) => toggleRole(role.id, e.currentTarget.checked)}
                />{" "}
                {role.name}
              </label>
            ))}
          </Dialog.Content>

          <Dialog.Footer>
            <Button variant="secondary" onClick={cancel}>
              Cancel
            </Button>
            <Button variant="danger" onClick={revokeAccess}>
              Revoke access
            </Button>
            <Button variant="primary" onClick={saveUser}>
              Save changes
            </Button>
          </Dialog.Footer>
        </>
      ) : (
        <Dialog.Content>
          <Loader text={saving ? "Saving..." : "Loading..."} />
        </Dialog.Content>
      )}
    </Dialog>
  )
}
