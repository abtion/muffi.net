import classNames from "classnames"
import React, { useEffect } from "react"
import "./index.scss"
import Header from "./Header"
import Footer from "./Footer"
import Content from "./Content"

const closeIcon = (
  <svg
    width="14"
    height="14"
    viewBox="0 0 14 14"
    fill="none"
    xmlns="http://www.w3.org/2000/svg"
  >
    <path
      d="M1 13L13 1M1 1L13 13"
      stroke="#9CA3AF"
      strokeWidth="2"
      strokeLinecap="round"
      strokeLinejoin="round"
    />
  </svg>
)

export type DialogProps = Omit<
  React.HTMLAttributes<HTMLDivElement>,
  "onClick"
> & {
  onClose: () => void
}

function Dialog({
  children,
  onClose,
  className,
  ...rest
}: DialogProps): JSX.Element {
  // Handle `cancel` event: (allows pressing ESC to close)
  useEffect(() => {
    function onCancel(event: KeyboardEvent) {
      if (event.key == "Escape") {
        event.preventDefault()
        onClose()
      }
    }

    document.addEventListener("keydown", onCancel)

    return () => {
      document.removeEventListener("keydown", onCancel)
    }
  }, [onClose])

  return (
    <div className="Dialog-Container">
      <div className="Dialog-Curtain"></div>
      <div className={classNames(className, "Dialog")} {...rest}>
        <button title="Close" onClick={onClose} className="Dialog__Close">
          {closeIcon}
        </button>
        {children}
      </div>
    </div>
  )
}

export default Object.assign(Dialog, {
  Header,
  Footer,
  Content,
})
