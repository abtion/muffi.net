import React from "react"
import LoaderIcon from "./icon.svg"
import "./style.scss"

export default function Loader({ text }: { text?: string }): JSX.Element {
  return (
    <div className="Loader">
      <div className="Loader__icon">
        <LoaderIcon />
      </div>
      <div className="Loader__text">{text}</div>
    </div>
  )
}
