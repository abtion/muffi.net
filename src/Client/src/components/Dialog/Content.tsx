import React from "react"
import classNames from "classnames"

type ContentProps = React.HTMLAttributes<HTMLDivElement>

export default function Content({
  children,
  className,
  ...rest
}: ContentProps): JSX.Element {
  return (
    <div className={classNames(className, "Dialog__Content")} {...rest}>
      {children}
    </div>
  )
}
