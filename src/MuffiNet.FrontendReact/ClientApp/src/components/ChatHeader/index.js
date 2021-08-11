import classNames from "classnames"
import React from "react"

import "./style.scss"

export default function ChatHeader(props) {
  const usedProps = {
    ...props,
    className: classNames("ChatHeader", props.className),
  }

  return <div {...usedProps} />
}
