import React from "react"
import classNames from "classnames"

type HeaderProps = React.HTMLAttributes<HTMLDivElement> & {
  title: string
}

export default function Header({
  children,
  title,
  className,
  ...rest
}: HeaderProps): JSX.Element {
  return (
    <div className={classNames(className, "Dialog__Header")} {...rest}>
      <div className="Dialog__Title">{title}</div>
      {children}
    </div>
  )
}
