import React from "react"
import { createRoot } from "react-dom/client"
import { BrowserRouter } from "react-router-dom"
import "~/main.scss"
import App from "~/App"

import prepareColorVariables from "~/utils/prepareColorVariables"
import colors from "../../../../colors.json"

const rootElement = document.getElementById("root")

if (!rootElement) {
  throw new Error("root element not found")
}

const cssRoot = document.documentElement
const cssVariables = prepareColorVariables(colors).cssVariables

Object.entries(cssVariables).forEach(([name, value]) =>
  cssRoot.style.setProperty(name, value)
)

const root = createRoot(rootElement)

// TODO remove basename?
root.render(
  <BrowserRouter basename={"/"}>
    <App />
  </BrowserRouter>
)
