import React from "react"
import ReactDOM from "react-dom"
import { BrowserRouter } from "react-router-dom"
import "~/main.scss"
import App from "~/App"

import prepareColorVariables from "~/utils/prepareColorVariables"
import colors from "../../../../colors.json"

const baseElement = document.querySelector("base")
const baseUrl = baseElement?.getAttribute("href") || "/"

const rootElement = document.getElementById("root")

const cssRoot: HTMLElement | null = document.querySelector(":root")
const cssVariables = prepareColorVariables(colors).cssVariables

Object.entries(cssVariables).forEach(([name, value]) =>
  cssRoot?.style.setProperty(name, value)
)

ReactDOM.render(
  <BrowserRouter basename={baseUrl}>
    <App />
  </BrowserRouter>,
  rootElement
)
