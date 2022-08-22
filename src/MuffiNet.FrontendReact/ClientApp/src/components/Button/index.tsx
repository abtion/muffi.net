import React from "react"
import classNames from "classnames"
import Variant from "../../const/variant"
import Size from "../../const/size"
import "./index.scss"

export interface ButtonProps
  extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  size?: Size
  variant?: Variant
  outline?: boolean
}

// TODO adjust Button options and styles to better match Jacob's design

export default function Button(props: ButtonProps): JSX.Element {
  const { size, variant, outline, className, ...rest } = props

  const usedClassName = classNames(
    "Button",
    {
      [`Button--${size}`]: size,
      [`Button--${variant}`]: variant,
      "Button--outline": outline,
    },
    className
  )

  return <button className={usedClassName} {...rest} />
}
