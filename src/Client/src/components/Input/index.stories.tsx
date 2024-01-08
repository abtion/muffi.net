import { type Meta, type StoryObj } from "@storybook/react"
import { within, expect, userEvent, waitFor } from "@storybook/test"
import Input, { InputProps } from "./"

const meta = {
  component: Input,
  argTypes: {
    onChange: { action: true },
  },
} satisfies Meta<typeof Input>
export default meta

export const Default: StoryObj<typeof meta> = {
  args: {
    placeholder: "Input placeholder",
    size: "md",
    variant: "default",
  } as InputProps,

  play: async ({ args, canvasElement }) => {
    const canvas = within(canvasElement)
    const input = canvas.getByPlaceholderText(args.placeholder as string)

    await userEvent.type(input, "input text")

    await waitFor(() => expect(args.onChange).toHaveBeenCalledTimes(10))
  },
}

export const Error: StoryObj<typeof meta> = {
  args: {
    ...Default.args,
    variant: "error",
  } as InputProps,
}
