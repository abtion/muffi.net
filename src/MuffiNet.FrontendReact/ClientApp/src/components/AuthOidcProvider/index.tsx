import React, { useEffect, useState, ReactNode } from "react"
import { AuthProvider, AuthProviderProps } from "oidc-react"
import axios from "axios"
import LoaderFullPage from "~/components/LoaderFullPage"

export default function AuthOidcProvider({
  children,
}: {
  children: ReactNode
}): JSX.Element {
  const [oidcConfig, setOidcConfig] = useState<AuthProviderProps>()

  useEffect(() => {
    axios.get("api/oidc/frontend-configuration").then(({ data }) => {
      setOidcConfig({
        redirectUri: `${location.origin}/admin`, // TODO redirect to the URL the user actually came from
        postLogoutRedirectUri: `${location.origin}/admin`,
        silentRedirectUri: `${location.origin}/admin`,
        authority: data.authority,
        clientId: data.clientId,
        scope: data.scopes,
      })
    })
  }, [])

  if (!oidcConfig) {
    return <LoaderFullPage />
  }

  return (
    <AuthProvider
      {...oidcConfig}
      automaticSilentRenew={true}
      onSignIn={
        /* istanbul ignore next */ () => {
          /* istanbul ignore next */
          history.replaceState(null, "", "/admin")
        }
      }
    >
      {children}
    </AuthProvider>
  )
}
