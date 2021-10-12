import classNames from "classnames"
import React from "react"

import "./style.scss"

export default function Table({
  className,
  ...rest
}: React.TableHTMLAttributes<HTMLTableElement>): JSX.Element {
  const usedClassName = classNames("Table", className)

  return <table className={usedClassName} {...rest} />
}
