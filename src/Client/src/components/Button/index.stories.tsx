import { type Meta, type StoryObj } from "@storybook/react"
import { within, expect, userEvent, waitFor } from "@storybook/test"
import Button, { ButtonProps } from "./"

const meta = {
  component: Button,
  argTypes: {
    onClick: { action: true },
  },
} satisfies Meta<typeof Button>
export default meta

export const Primary: StoryObj<typeof meta> = {
  args: {
    children: <p>Button text</p>,
    size: "md",
    variant: "primary",
  } as ButtonProps,

  play: async ({ args, canvasElement }) => {
    const canvas = within(canvasElement)
    const button = canvas.getByText("Button text")
    await userEvent.click(button)
    await waitFor(() => expect(args.onClick).toHaveBeenCalled())
  },
}

export const Secondary: StoryObj<typeof meta> = {
  args: {
    ...Primary.args,
    variant: "secondary",
  } as ButtonProps,

  play: async (context) => await Primary.play?.(context),
}
