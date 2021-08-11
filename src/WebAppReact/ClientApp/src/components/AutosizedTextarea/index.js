import React, { useCallback, useEffect, useRef } from "react"
import useWindowSizeEffect from "~/hooks/useWindowSizeEffect"

export default function AutosizedTextarea({
  minHeight = 500,
  maxHeight = Infinity,
  ...restProps
}) {
  const textareaRef = useRef()

  const resize = useCallback(() => {
    const element = textareaRef.current

    element.style.height = "0"

    let newHeight = Math.max(minHeight, element.scrollHeight)
    newHeight = Math.min(newHeight, maxHeight)

    const computedProps = getComputedStyle(element)

    newHeight += parseInt(computedProps["border-top-width"])
    newHeight += parseInt(computedProps["border-bottom-width"])

    element.style.height = newHeight + "px"
  }, [minHeight, maxHeight])

  useWindowSizeEffect(resize)

  useEffect(() => {
    if (restProps.autoFocus) {
      const end = textareaRef.current.value.length
      textareaRef.current.setSelectionRange(end, end)
    }
  }, [restProps.autoFocus])

  useEffect(resize, [resize, restProps.value])

  return (
    <textarea
      rows="1"
      ref={textareaRef}
      {...restProps}
      style={{ resize: "none", padding: "10px", overflow: "hidden" }}
    />
  )
}
