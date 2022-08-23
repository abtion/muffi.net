import React from "react"
import classNames from "classnames"

type FooterProps = React.HTMLAttributes<HTMLDivElement>

export default function Footer({
  children,
  className,
  ...rest
}: FooterProps): JSX.Element {
  return (
    <div className={classNames(className, "Dialog__Footer")} {...rest}>
      {children}
    </div>
  )
}
