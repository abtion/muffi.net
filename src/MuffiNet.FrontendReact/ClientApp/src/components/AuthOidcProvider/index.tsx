import React, { useEffect, useState, ReactNode } from "react"
import { AuthProvider, AuthProviderProps } from "react-oidc-context"
import axios from "axios"
import LoaderFullPage from "~/components/LoaderFullPage"

// NOTE: uncomment these lines to enable diagnostic logging
// import { Log } from "oidc-client-ts";
// Log.setLogger(console)
// Log.setLevel(Log.DEBUG)

export default function AuthOidcProvider({
  children,
}: {
  children: ReactNode
}): JSX.Element {
  const [oidcConfig, setOidcConfig] = useState<AuthProviderProps>()

  useEffect(() => {
    axios.get("/api/oidc/frontend-configuration").then(({ data }) => {
      setOidcConfig({
        redirect_uri: `${location.origin}/admin`, // TODO redirect to the URL the user actually came from
        post_logout_redirect_uri: `${location.origin}/`,
        silent_redirect_uri: `${location.origin}/admin`,
        authority: data.authority,
        client_id: data.clientId,
        scope: data.scopes,
        automaticSilentRenew: true,
        onSigninCallback() {
          /* istanbul ignore next */
          history.replaceState(null, "", "/admin")
        },
        // TODO figure out how to fully log out.
        //      neither `post_logout_redirect_uri` or `onRemoveUser` have the intended effect.
        //      you're going to end up on `/admin` again, and get silently logged back in -
        //      which should not be possible, at all, after explicitly logging out.
        //      https://github.com/authts/react-oidc-context/issues/485#issuecomment-1231384126
        // onRemoveUser() {
        //   history.replaceState(null, "", "/")
        // }
      })
    })
  }, [])

  if (!oidcConfig) {
    return <LoaderFullPage />
  }

  return <AuthProvider {...oidcConfig}>{children}</AuthProvider>
}
