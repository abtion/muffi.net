import React, { useEffect, useState, ReactNode } from "react"
import { AuthProvider } from "oidc-react"
import axios from "axios"
import LoaderFullPage from "~/components/LoaderFullPage"
import AuthOidcConfigContext, {
  AuthOidcConfig,
} from "~/contexts/AuthOidcConfigContext"

export default function AuthOidcProvider({
  children,
}: {
  children: ReactNode
}): JSX.Element {
  const [oidcConfig, setOidcConfig] = useState<AuthOidcConfig>()

  useEffect(() => {
    axios.get("api/oidc/frontend-configuration").then(({ data }) => {
      setOidcConfig({
        redirectUri: `${location.origin}/admin`,
        postLogoutRedirectUri: `${location.origin}/admin`,
        silentRedirectUri: `${location.origin}/admin`,
        authority: data.authority,
        clientId: data.clientId,
        scope: data.scopes,
        responseType: "id_token token",
      })
    })
  }, [])

  if (!oidcConfig) return <LoaderFullPage />
  return (
    <AuthOidcConfigContext.Provider value={oidcConfig}>
      <AuthProvider
        {...oidcConfig}
        automaticSilentRenew={true}
        onSignIn={
          /* istanbul ignore next */ () => {
            /* istanbul ignore next */
            history.replaceState(null, "/admin", " ")
          }
        }
      >
        {children}
      </AuthProvider>
    </AuthOidcConfigContext.Provider>
  )
}
