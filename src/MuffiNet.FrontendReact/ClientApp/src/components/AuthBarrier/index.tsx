import React, { ReactNode, useMemo } from "react"
import { useAuth } from "oidc-react"
import axios from "axios"
import ApiContext from "~/contexts/ApiContext"
import LoaderFullPage from "../LoaderFullPage"

interface AuthBarrierProps {
  children?: ReactNode | undefined
}

export default function AuthBarrier({ children }: AuthBarrierProps): JSX.Element {
  const { isLoading, userData } = useAuth()

  const api = useMemo(() => {
    if (isLoading || !userData?.id_token) {
      return null
    }

    return axios.create({
      headers: {
        authorization: `Bearer ${userData.id_token}`,
      },
    })
  }, [isLoading, userData])

  if (!api) {
    return <LoaderFullPage text="Loading..." />
  }

  return (
    <ApiContext.Provider value={api}>
      {children}
    </ApiContext.Provider>
  )
}
