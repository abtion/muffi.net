import classNames from "classnames"
import React from "react"

import "./style.scss"

export default function Button({
  outline = false,
  color = "secondary",
  size = "md",
  ...props
}) {
  const usedProps = {
    ...props,
    className: classNames(
      "Button",
      `Button--${size}`,
      `Button--${color}`,
      {
        "Button--outline": outline,
      },
      props.className
    ),
  }

  return <button {...usedProps} />
}
