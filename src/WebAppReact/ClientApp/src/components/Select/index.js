import classNames from "classnames"
import React from "react"

export default function Select(props) {
  const usedProps = {
    ...props,
    className: classNames(
      "focus:ring-brand-blue",
      "focus:ring-1",
      "focus:border-brand-blue",

      "block",
      "w-full",

      "border-2",
      "border-gray-300",

      "rounded-md",

      "px-1",
      "py-2",

      "md:px-1",
      "md:py-3",
      props.className
    ),
  }

  return <select {...usedProps} />
}
