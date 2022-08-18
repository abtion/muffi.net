import { useAuth } from "oidc-react"
import React, { ReactNode } from "react"

interface AuthRoleBarrier {
  children: ReactNode
  /**
   * List of the Roles allowed to view the contents behind this barrier.
   */
  allow: string[]
}

/**
 * Limits the visibility of child elements to Users with a given Role assignment.
 */
export default function AuthRoleBarrier({
  children,
  allow,
}: AuthRoleBarrier): JSX.Element {
  const { isLoading, userData } = useAuth()

  const visible =
    !isLoading && userData?.profile?.roles?.some((role) => allow.includes(role))

  return <>{visible ? children : null}</>
}
