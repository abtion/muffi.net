import React from "react"
import NavMenu from "~/components/NavMenu"

export default function AuthorizedLayout({ children }) {
  return (
    <div style={{ display: "flex", flexDirection: "column", height: "100%" }}>
      <NavMenu />
      {children}
    </div>
  )
}
