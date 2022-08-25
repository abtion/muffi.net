import Dialog from "~/components/Dialog"
import Variant from "~/const/variant"
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

  const [appRoleIDs, setAppRoleIDs] = useState(_user.appRoleIDs)

  const [saving, setSaving] = useState(false)

  useEffect(() => {
    api
      .get<UserDetails>("/api/roleAdmin/user-details", {
        params: { userID: _user.userID },
      })
      .then((response) => setDetails(response.data))
  }, [])

  function toggleRole(roleID: string, enabled: boolean) {
    setAppRoleIDs(
      enabled
        ? appRoleIDs.concat(roleID)
        : appRoleIDs.filter((id) => id !== roleID)
    )
  }

  async function saveUser() {
    setDetails(undefined)
    setSaving(true)

    await api.post("/api/roleAdmin/update-user", {
      userID: _user.userID,
      name,
      appRoleIDs,
    })

    onClose(true)
  }

  async function revokeAccess() {
    setDetails(undefined)
    setSaving(true)

    await api.post("/api/roleAdmin/revoke-access", "", { params: { userID: _user.userID } })

    onClose(true)
  }

  function cancel() {
    onClose(false)
  }

  return (
    <Dialog isOpen={true} onClose={cancel}>
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
                className="block w-full"
                variant={Variant.Neutral}
                value={name}
                onChange={(e) => setName(e.currentTarget.value)}
              />
            </label>
            <label className="block mb-5 text-sm">
              Email
              <Input
                variant={Variant.Neutral}
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
                  checked={appRoleIDs.includes(role.id)}
                  onChange={(e) => toggleRole(role.id, e.currentTarget.checked)}
                />{" "}
                {role.name}
              </label>
            ))}
          </Dialog.Content>

          <Dialog.Footer>
            <Button variant={Variant.Neutral} onClick={cancel}>
              Cancel
            </Button>
            <Button variant={Variant.Danger} onClick={revokeAccess}>Revoke access</Button>
            <Button onClick={saveUser}>Save changes</Button>
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
