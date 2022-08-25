import React from "react"
import { render } from "@testing-library/react"

import Button, { ButtonProps } from "."

const defaultProps: ButtonProps = {
  children: "Button text",
}

describe(Button, () => {
  it("displays button text", () => {
    const { getByText } = render(<Button {...defaultProps} />)

    const button = getByText(defaultProps.children as string)

    expect(button).toBeInTheDocument()
  })

  describe("when size is set", () => {
    it("adds size class", () => {
      const { getByText } = render(
        <Button {...defaultProps} size="md" />
      )

      const button = getByText(defaultProps.children as string)

      expect(button).toHaveClass("Button--md")
    })
  })

  describe("when variant is set", () => {
    it("adds variant class", () => {
      const { getByText } = render(
        <Button {...defaultProps} variant="danger" />
      )

      const button = getByText(defaultProps.children as string)

      expect(button).toHaveClass("Button--danger")
    })
  })

  describe("when variant or size are set to none", () => {
    it("does not add variant or size classes", () => {
      const { getByText } = render(
        <Button {...defaultProps} variant="none" size="none" className="font-lg" />
      )

      const button = getByText(defaultProps.children as string)

      expect(button.className).toBe("Button font-lg")
    })
  })
})
