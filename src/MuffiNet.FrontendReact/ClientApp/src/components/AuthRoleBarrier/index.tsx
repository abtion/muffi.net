import React, { ReactNode } from "react"
import { useAuth } from "react-oidc-context"

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
  const { isLoading, user } = useAuth()

  const visible =
    !isLoading && user?.profile?.roles?.some((role) => allow.includes(role))

  return <>{visible ? children : null}</>
}
