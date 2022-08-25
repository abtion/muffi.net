import React from "react"
import { render } from "@testing-library/react"
import Input, { InputProps } from "."

const defaultProps: InputProps = {
  placeholder: "Input placeholder",
}

describe(Input, () => {
  it("displays Input text", () => {
    const { getByPlaceholderText } = render(<Input {...defaultProps} />)

    const input = getByPlaceholderText(defaultProps.placeholder as string)

    expect(input).toBeInTheDocument()
  })

  describe("when size is set", () => {
    it("adds size class", () => {
      const { getByPlaceholderText } = render(
        <Input {...defaultProps} size="md" />
      )

      const input = getByPlaceholderText(defaultProps.placeholder as string)

      expect(input).toHaveClass("Input--md")
    })
  })

  describe("when variant is set", () => {
    it("adds variant class", () => {
      const { getByPlaceholderText } = render(
        <Input {...defaultProps} variant="error" />
      )

      const input = getByPlaceholderText(defaultProps.placeholder as string)

      expect(input).toHaveClass("Input--error")
    })
  })

  describe("when variant or size are set to none", () => {
    it("does not add variant or size classes", () => {
      const { getByPlaceholderText } = render(
        <Input
          {...defaultProps}
          variant="none"
          size="none"
          className="text-lg"
        />
      )

      const input = getByPlaceholderText(defaultProps.placeholder as string)

      // eslint-disable-next-line jest-dom/prefer-to-have-class
      expect(input.className).toBe("Input text-lg")
    })
  })
})
