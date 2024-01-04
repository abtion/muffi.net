import React from "react"
import { createRoot } from "react-dom/client"
import App from "~/App"
import "~/main.scss"

import prepareColorVariables from "~/utils/prepareColorVariables"
import colors from "../colors.json"

const rootElement = document.getElementById("root")

if (!rootElement) throw new Error("root element not found")

const cssRoot = document.documentElement
const cssVariables = prepareColorVariables(colors).cssVariables

Object.entries(cssVariables).forEach(([name, value]) =>
  cssRoot.style.setProperty(name, value),
)

createRoot(rootElement).render(
  <React.StrictMode>
    <App />
  </React.StrictMode>,
)
