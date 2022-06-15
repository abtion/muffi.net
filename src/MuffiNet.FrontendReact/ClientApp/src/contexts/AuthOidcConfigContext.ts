import { createContext } from "react"
import { AuthProviderProps } from "oidc-react"

export type AuthOidcConfig = Pick<
  AuthProviderProps,
  | "redirectUri"
  | "postLogoutRedirectUri"
  | "silentRedirectUri"
  | "authority"
  | "clientId"
  | "scope"
  | "responseType"
>

export default createContext<AuthOidcConfig>({
  redirectUri: "N/A",
  postLogoutRedirectUri: "N/A",
  silentRedirectUri: "N/A",
  authority: "N/A",
  clientId: "N/A",
  scope: "N/A",
  responseType: "N/A",
})
