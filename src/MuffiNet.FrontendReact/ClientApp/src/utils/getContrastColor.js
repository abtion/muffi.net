// This file uses js and commonjs to be compatible with tailwind config

/* eslint-disable @typescript-eslint/no-var-requires */
import getLuminance from "./getLuminance.mjs"
/* eslint-enable @typescript-eslint/no-var-requires */

// Based on luminance, pick the best suited contrast color (dark/light) to the target color
export default function getContrastColor(target, dark, light) {
  const targetLuminance = getLuminance(target)
  const darkLuminance = getLuminance(dark)
  const lightLuminance = getLuminance(light)

  const darkDiff = Math.abs(targetLuminance - darkLuminance)
  const lightDiff = Math.abs(targetLuminance - lightLuminance)

  return darkDiff > lightDiff ? dark : light
}
