import classNames from "classnames"
import React from "react"
import "./style.scss"

export interface LoaderProps extends React.HTMLAttributes<HTMLDivElement> {
  text?: string
  spinnerClassName?: string
  textClassName?: string
}

export default function Loader({
  text,
  className,
  spinnerClassName,
  textClassName,
  ...rest
}: LoaderProps): JSX.Element {
  const usedClassName = classNames("Loader", className)
  const usedSpinnerClassName = classNames("Loader__spinner", spinnerClassName)
  const usedTextClassName = classNames("Loader__text", textClassName)

  return (
    <div className={usedClassName} {...rest}>
      <div className={usedSpinnerClassName} />
      <div className={usedTextClassName}>{text}</div>
    </div>
  )
}
