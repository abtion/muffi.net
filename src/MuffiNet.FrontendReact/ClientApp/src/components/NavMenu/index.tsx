import React, { useCallback, useState } from "react"
import { Link } from "react-router-dom"
import { ReactComponent as ListIcon } from "bootstrap-icons/icons/list.svg"
import classNames from "classnames"

import { LoginMenu } from "~/authorization/LoginMenu"
import NavItem from "~/components/NavItem"
import NavLink from "~/components/NavLink"

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

  return (
    <header>
      <nav className="bg-white shadow-md sticky inset-x-0">
        <div className="container">
          <div className="flex justify-between flex-wrap -mx-2">
            <div className="flex">
              <div>
                <Link
                  className="flex items-center py-4 px-2 text-gray-700 hover:text-gray-900"
                  to="/authhome"
                >
                  <span className="font-bold">MuffiNet</span>
                </Link>
              </div>
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
                <NavLink to="/authhome">Overview</NavLink>
              </NavItem>

              <LoginMenu />
            </ul>
          </div>
        </div>
      </nav>
    </header>
  )
}
