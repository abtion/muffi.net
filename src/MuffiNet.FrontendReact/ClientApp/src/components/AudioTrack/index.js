import React, { useRef, useEffect } from "react"

export default function AudioTrack({ track }) {
  const ref = useRef(null)

  useEffect(() => {
    ref.current.innerHTML = ""

    if (track) {
      const child = track.attach()
      child.classList.add(track.kind)
      child.classList.add(track.name)
      ref.current.appendChild(child)
    }
  }, [track])

  return <div className="hidden" ref={ref}></div>
}
