import { useState } from "react"
import { Duration as LuxonDuration } from "luxon"
import useInterval from "~/hooks/useInterval"

const oneMinuteMs = 60000

const getDuration = (since) => Date.now() - since

export default function Duration({ since, format = "hh:mm", upperLimit }) {
  const [duration, setDuration] = useState(getDuration(since))

  useInterval(
    () => {
      setDuration(getDuration(since))
    },
    oneMinuteMs,
    [since]
  )

  if (upperLimit && duration > upperLimit) {
    return LuxonDuration.fromMillis(upperLimit).toFormat(`> ${format}`)
  }

  if (duration < 0) return "00:00"

  return LuxonDuration.fromMillis(duration).toFormat(format)
}
