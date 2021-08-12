import React, { Component } from "react"
import { Switch, Route } from "react-router"
import { Redirect } from "react-router-dom"

import AuthorizeRoute from "~/authorization/AuthorizeRoute"
import ApiAuthorizationRoutes from "~/authorization/ApiAuthorizationRoutes"
import { ApplicationPaths } from "~/authorization/ApiAuthorizationConstants"

// Visitors
import Home from "~/pages/Home"

// Authorized pages
import AuthorizedHome from "~/pages/AuthorizedHome"

export default class App extends Component {
  static displayName = App.name

  render() {
    return (
      <Switch>
        <Route
          path={ApplicationPaths.ApiAuthorizationPrefix}
          component={ApiAuthorizationRoutes}
        />

        <AuthorizeRoute exact path="/authhome" component={AuthorizedHome} />

        <Switch>
          <Route exact path={"/"} component={Home} />
        </Switch>
      </Switch>
    )
  }
}
