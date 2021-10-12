import React from "react"
import NavMenu from "~/components/NavMenu"

export default function AuthorizedLayout({
  children,
}: AuthorizedLayoutProps): JSX.Element {
  return (
    <div style={{ display: "flex", flexDirection: "column", height: "100%" }}>
      <NavMenu />
      {children}
    </div>
  )
}

type AuthorizedLayoutProps = {
  children?: JSX.Element
}
