import React from "react"
import classNames from "classnames"
import "./index.scss"

export interface InputProps
  extends Omit<React.InputHTMLAttributes<HTMLInputElement>, "size"> {
  size?: "xs" | "sm" | "md" | "lg" | "xl" | "none"
  variant?: "default" | "error" | "none"
}

export default function Input({
  size = "md",
  variant = "default",
  className,
  ...rest
}: InputProps): JSX.Element {
  const usedClassName = classNames(
    "Input",
    size !== "none" && `Input--${size}`,
    variant !== "none" && `Input--${variant}`,
    className
  )

  return <input className={usedClassName} {...rest} />
}
