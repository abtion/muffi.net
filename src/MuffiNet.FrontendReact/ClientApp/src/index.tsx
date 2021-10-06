import React from "react"
import ReactDOM from "react-dom"
import { BrowserRouter } from "react-router-dom"
import "~/main.scss"
import App from "~/App"

import prepareColorVariables from "~/utils/prepareColorVariables"
import colors from "../../../../colors.json"

const baseUrl = document.getElementsByTagName("base")[0].getAttribute("href")
const rootElement = document.getElementById("root")

const cssRoot: HTMLElement = document.querySelector(":root")
const cssVariables = prepareColorVariables(colors).cssVariables
Object.entries(cssVariables).forEach(([name, value]) =>
  cssRoot.style.setProperty(name, value)
)

ReactDOM.render(
  <BrowserRouter basename={baseUrl}>
    <App />
  </BrowserRouter>,
  rootElement
)
