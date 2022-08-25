import React from "react"
import classNames from "classnames"
import "./index.scss"

export interface ButtonProps
  extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  size?: "xs" | "sm" | "md" | "lg" | "xl" | "none"
  variant?: "standard" | "primary" | "danger" | "none"
}

export default function Button({
  size = "md",
  variant = "standard",
  className,
  ...rest
}: ButtonProps): JSX.Element {
  const usedClassName = classNames(
    "Button",
    size !== "none" && `Button--${size}`,
    variant !== "none" && `Button--${variant}`,
    className
  )

  return <button className={usedClassName} {...rest} />
}
