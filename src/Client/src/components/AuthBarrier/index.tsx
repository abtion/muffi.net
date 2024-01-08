import { ReactNode, useEffect, useMemo } from "react"
import axios from "axios"
import ApiContext from "~/contexts/ApiContext"
import LoaderFullPage from "../LoaderFullPage"
import { hasAuthParams, useAuth } from "react-oidc-context"

interface AuthBarrierProps {
  children?: ReactNode | undefined
}

export default function AuthBarrier({
  children,
}: AuthBarrierProps): JSX.Element {
  const { user, isAuthenticated, error, signinRedirect } = useAuth()

  // Handle automatic sign-in:

  useEffect(() => {
    if (!hasAuthParams() && !isAuthenticated) {
      signinRedirect()
    }
  }, [])

  // Configure the API client:

  const api = useMemo(() => {
    return isAuthenticated && user
      ? axios.create({
          headers: {
            authorization: `Bearer ${user.id_token}`,
          },
        })
      : null
  }, [isAuthenticated, user])

  // Render:

  if (error) {
    return <div>ERROR: {error.message}</div>
  }

  if (isAuthenticated && api) {
    return <ApiContext.Provider value={api}>{children}</ApiContext.Provider>
  }

  return <LoaderFullPage text="Loading..." />
}
