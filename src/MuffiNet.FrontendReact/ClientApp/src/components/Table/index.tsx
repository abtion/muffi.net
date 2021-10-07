import classNames from "classnames"
import React from "react"

import "./style.scss"

export default function Table(props): JSX.Element {
  const usedProps = {
    ...props,
    className: classNames("Table", props.className),
  }

  return <table {...usedProps} />
}
