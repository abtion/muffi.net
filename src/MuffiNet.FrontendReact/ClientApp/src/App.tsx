import React from "react"
import { Switch, Route } from "react-router"

import AuthorizeRoute from "~/authorization/AuthorizeRoute"
import ApiAuthorizationRoutes from "~/authorization/ApiAuthorizationRoutes"
import { ApplicationPaths } from "~/authorization/ApiAuthorizationConstants"

// Visitors
import Home from "~/pages/Home"

// Authorized pages
import AuthorizedHome from "~/pages/AuthorizedHome"

export default function App(): JSX.Element {
  return (
    <Switch>
      <Route
        path={ApplicationPaths.ApiAuthorizationPrefix}
        component={ApiAuthorizationRoutes}
      />

      {/* https://docs.microsoft.com/en-us/azure/active-directory/develop/scenario-spa-app-registration */}
      <AuthorizeRoute exact path="/authhome" component={AuthorizedHome} />

      <Switch>
        <Route exact path={"/"} component={Home} />
      </Switch>
    </Switch>
  )
}
