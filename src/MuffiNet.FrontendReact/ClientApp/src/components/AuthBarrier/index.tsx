import React, { ReactNode, useMemo } from "react"
import { useAuth } from "oidc-react"
import axios from "axios"
import ApiContext from "~/contexts/ApiContext"
import LoaderFullPage from "../LoaderFullPage"

interface AuthBarrierProps {
  children?: ReactNode | undefined
}

export default function AuthBarrier(props: AuthBarrierProps): JSX.Element {
  const { isLoading, userData } = useAuth()

  const api = useMemo(() => {
    if (isLoading || !userData?.id_token) return null
    
    // TODO allow some means of checking userData.profile.roles? and use different <AuthBarrier/> instances around restricted areas
    
    return axios.create({
      headers: {
        authorization: `Bearer ${userData.id_token}`,
      },
    })
  }, [isLoading, userData])

  // TODO er, this isn't always being used as a full page (top level) component, so..?
  
  if (!api) return <LoaderFullPage text="Loading..." />

  return <ApiContext.Provider value={api} {...props} />
}
