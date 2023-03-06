import React from "react"
import NavMenu from "~/components/NavMenu"

type AuthorizedLayoutProps = React.HTMLAttributes<HTMLDivElement>

export default function AuthorizedLayout({
  style,
  children,
}: AuthorizedLayoutProps): JSX.Element {
  return (
    // TODO use Tailwind
    <div
      style={{
        ...(style || {}),
        display: "flex",
        flexDirection: "column",
        height: "100%",
      }}
    >
      <NavMenu />
      {children}
    </div>
  )
}
