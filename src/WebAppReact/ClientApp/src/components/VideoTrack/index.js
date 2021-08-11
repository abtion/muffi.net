import React, { useRef, useEffect } from "react"
import classNames from "classnames"

import "./style.css"
import useTrackIsEnabled from "~/hooks/useTrackIsEnabled"

export default function VideoTrack({
  className,
  track,
  contain = false,
  mirror = false,
  children,
}) {
  const ref = useRef(null)
  const isEnabled = useTrackIsEnabled(track)

  const usedClassName = classNames(
    "VideoTrack",
    {
      "VideoTrack--disabled": !isEnabled,
    },
    className
  )

  useEffect(() => {
    const previousVideoElement = ref.current.querySelector(".VideoTrack__video")
    if (previousVideoElement) {
      ref.current.removeChild(previousVideoElement)
    }

    if (track) {
      const child = track.attach()

      child.classList.add("VideoTrack__video")
      child.classList.add(track.kind)
      child.classList.add(track.name)
      child.classList.toggle("VideoTrack__video--contain", contain)
      child.classList.toggle("VideoTrack__video--mirror", mirror)

      ref.current.appendChild(child)
    }
  }, [track, contain, mirror])

  return (
    <div className={usedClassName} ref={ref}>
      {children}
    </div>
  )
}
