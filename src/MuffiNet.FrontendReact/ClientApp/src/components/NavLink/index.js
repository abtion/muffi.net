import classNames from "classnames"
import React from "react"
import { Link } from "react-router-dom"

export default function NavLink({ size = "md", ...props }) {
  const usedProps = {
    ...props,
    className: classNames(
      "py-2 px-2 md:py-4 text-gray-700 hover:text-gray-900 block",
      props.className
    ),
  }

  return <Link {...usedProps} />
}
