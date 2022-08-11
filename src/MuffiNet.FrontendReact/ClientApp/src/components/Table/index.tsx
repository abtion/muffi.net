import classNames from "classnames"
import React from "react"

import "./style.scss"

export default function Table({
  className,
  ...rest
}: React.TableHTMLAttributes<HTMLTableElement>): JSX.Element {
  const usedClassName = classNames("Table", className)

  return (
    <div className={usedClassName}>
      <table {...rest} />
    </div>
  )
}
