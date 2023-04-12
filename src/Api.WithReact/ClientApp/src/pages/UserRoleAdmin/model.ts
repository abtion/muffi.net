/**
 * Users, Roles and Permissions (provided by the server)
 */
export interface UserRoleData {
  roles: Array<{
    id: string
    name: string
  }>
  users: Array<User>
}

export interface User {
  name: string
  userId: string
  appRoleIds: Array<string>
}

export interface UserDetails {
  email: string
}
