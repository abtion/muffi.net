import React from "react"
import classNames from "classnames"

export default function NavItem({
  className,
  ...rest
}: React.ButtonHTMLAttributes<HTMLLIElement>): JSX.Element {
  const usedClassName = classNames("block", className)

  return <li className={usedClassName} {...rest} />
}
