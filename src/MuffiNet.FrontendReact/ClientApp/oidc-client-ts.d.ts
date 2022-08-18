import { IdTokenClaims } from "oidc-client-ts"

declare module "oidc-client-ts" {
  interface IdTokenClaims {
    roles?: string[] // provided by Active Directory
  }
}
