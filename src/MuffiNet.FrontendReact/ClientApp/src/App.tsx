import React from "react"
import { Route, Routes } from "react-router"

import AuthOidcProvider from "~/components/AuthOidcProvider"
import AuthBarrier from "~/components/AuthBarrier"

// Visitors
import Home from "~/pages/Home"

// Authorized pages
import AuthorizedHome from "~/pages/AuthorizedHome"

export default function App(): JSX.Element {
  return (
    <Routes>
      <Route index element={<Home />} />

      <Route
        path="/admin"
        element={
          <AuthOidcProvider>
            <AuthBarrier>
              <Routes>
                <Route index element={<AuthorizedHome />} />
              </Routes>
            </AuthBarrier>
          </AuthOidcProvider>
        }
      />
    </Routes>
  )
}
