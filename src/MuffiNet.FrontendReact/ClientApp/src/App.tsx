import React from "react"
import { Switch, Route } from "react-router"

import AuthOidcProvider from "~/components/AuthOidcProvider"
import AuthBarrier from "~/components/AuthBarrier"

// Visitors
import Home from "~/pages/Home"

// Authorized pages
import AuthorizedHome from "~/pages/AuthorizedHome"

export default function App(): JSX.Element {
  return (
    <Switch>
      <Route exact path="/" component={Home} />

      <Route path="/admin">
        <AuthOidcProvider>
          <AuthBarrier>
            <Switch>
              <Route exact path="" component={AuthorizedHome} />
            </Switch>
          </AuthBarrier>
        </AuthOidcProvider>
      </Route>
    </Switch>
  )
}
