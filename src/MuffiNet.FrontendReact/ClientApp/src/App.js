import React, { Component } from "react"
import { Switch, Route } from "react-router"
import { Redirect } from "react-router-dom"

import AuthorizeRoute from "~/authorization/AuthorizeRoute"
import ApiAuthorizationRoutes from "~/authorization/ApiAuthorizationRoutes"
import { ApplicationPaths } from "~/authorization/ApiAuthorizationConstants"

import Home from "~/pages/Home"
// Visitors
import WaitingRoom from "~/pages/WaitingRoom"
import SignUp from "~/pages/SignUp"
import CustomerServiceCall from "~/pages/CustomerServiceCall"
import ThankYou from "~/pages/ThankYou"

// Technicians
import TechOverview from "~/pages/TechOverview"
import TechServiceCall from "~/pages/TechServiceCall"
import ThemeContext from "./contexts/ThemeContext"
import themes from "./themes"

export default class App extends Component {
  static displayName = App.name

  render() {
    return (
      <Switch>
        <Route
          path={ApplicationPaths.ApiAuthorizationPrefix}
          component={ApiAuthorizationRoutes}
        />

        <AuthorizeRoute exact path="/technician" component={TechOverview} />
        <AuthorizeRoute
          exact
          path="/technician/service-call/:supportTicketId"
          component={TechServiceCall}
        />

        <Route
          path="/:thirdparty"
          render={({
            match: {
              params: { thirdparty },
              path,
            },
          }) => {
            const theme = themes.find(
              ({ prefix }) => prefix === `/${thirdparty}`
            )

            if (!theme) return <Redirect to="/care1" />

            return (
              <ThemeContext.Provider value={theme}>
                <Switch>
                  <Route exact path={path} component={Home} />
                  <Route exact path={`${path}/sign-up`} component={SignUp} />
                  <Route
                    exact
                    path={`${path}/thank-you`}
                    component={ThankYou}
                  />
                  <Route
                    exact
                    path={`${path}/waiting-room/:supportTicketId`}
                    component={WaitingRoom}
                  />
                  <Route
                    exact
                    path={`${path}/service-call/:supportTicketId`}
                    component={CustomerServiceCall}
                  />
                </Switch>
              </ThemeContext.Provider>
            )
          }}
        />
        <Route exact path="/">
          <Redirect to="/care1" />
        </Route>
      </Switch>
    )
  }
}
