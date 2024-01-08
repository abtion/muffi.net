import type { Preview } from "@storybook/react"
import React from "react"
import prepareColorVariables from "../src/utils/prepareColorVariables"
import colors from "../colors.json"

const preview: Preview = {
  decorators: [
    (Story) => {
      let colorStyles = {}
      const cssVariables = prepareColorVariables(colors).cssVariables
      Object.entries(cssVariables).map(
        ([name, value]) => (colorStyles[name] = value),
      )
      const wrapped = (
        <div id="colorVariables" style={colorStyles}>
          <Story />
        </div>
      )
      return wrapped
    },
  ],

  parameters: {
    layout: "centered",
    actions: { argTypesRegex: "^on[A-Z].*" },
    controls: {
      matchers: {
        color: /(background|color)$/i,
        date: /Date$/i,
      },
    },
  },
}

export default preview
