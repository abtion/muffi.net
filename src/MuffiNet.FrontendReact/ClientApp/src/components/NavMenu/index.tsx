import React, { useCallback, useState } from "react"
import { Link } from "react-router-dom"
import ListIcon from "bootstrap-icons/icons/list.svg"
import classNames from "classnames"

import NavItem from "~/components/NavItem"
import NavLink from "~/components/NavLink"
import AuthRoleBarrier from "../AuthRoleBarrier"
import Button from "../Button"
import { useAuth } from "react-oidc-context"

export default function NavMenu(): JSX.Element {
  const [isCollapsed, setIsCollapsed] = useState(true)

  const toggleNavbar = useCallback(() => {
    setIsCollapsed(!isCollapsed)
  }, [setIsCollapsed, isCollapsed])

  const collapseClass = classNames(
    {
      hidden: isCollapsed,
    },
    "w-full md:w-auto md:flex items-center"
  )

  const { user, signoutRedirect } = useAuth()

  return (
    <header>
      <nav className="bg-white shadow-md sticky inset-x-0">
        <div className="container">
          <div className="flex justify-between flex-wrap -mx-2">
            <div className="flex items-center">
              <Link
                to="/admin"
                className="font-bold px-2 text-gray-700 hover:text-gray-900"
              >
                <span className="font-bold">MuffiNet</span>
              </Link>
              <span className="text-sm px-6">
                {user?.profile.preferred_username}
              </span>
              <Button
                variant="standard"
                size="xs"
                onClick={() => signoutRedirect()}
              >
                Log out
              </Button>
            </div>

            <div className="md:hidden flex items-center">
              <button
                onClick={toggleNavbar}
                className="mobile-menu-button focus:outline-none py-4 px-2"
              >
                <ListIcon />
              </button>
            </div>

            <ul className={collapseClass}>
              <NavItem>
                <NavLink to="/admin">Overview</NavLink>
              </NavItem>
              <AuthRoleBarrier allow={["Administrators"]}>
                <NavItem>
                  <NavLink to="/admin/roles">Roles</NavLink>
                </NavItem>
              </AuthRoleBarrier>
            </ul>
          </div>
        </div>
      </nav>
    </header>
  )
}
