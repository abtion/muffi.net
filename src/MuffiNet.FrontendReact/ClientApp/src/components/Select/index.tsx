import classNames from "classnames"
import React from "react"

export default function Select({ className, children, ...otherProps }: React.SelectHTMLAttributes<HTMLSelectElement>) {
  return (
    <select className={classNames("w-56 h-9 pl-2 border border-neutral-200 rounded text-sm", className)} {...otherProps}>
      {children}
    </select>
  )
}
