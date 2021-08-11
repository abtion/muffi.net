import React, { useContext, useState, useCallback } from "react"

import ChatContext from "~/contexts/ChatContext"
import AutosizedTextarea from "../AutosizedTextarea"

import "./style.scss"

export default function ChatInput({ track, maxHeight }) {
  const { postMessage } = useContext(ChatContext)
  const [content, setContent] = useState("")

  const handleInput = useCallback(
    ({ currentTarget: { value } }) => {
      setContent(value)
    },
    [setContent]
  )

  const handleSubmit = useCallback(
    (event) => {
      event.preventDefault()

      const contentAsJson = { text: content, timestamp: Date.now() }

      track.send(JSON.stringify(contentAsJson))
      postMessage({
        timestamp: contentAsJson.timestamp,
        content: contentAsJson.text,
        track,
      })
      setContent("")
    },
    [content, track, postMessage]
  )

  const handleEnterKeyDown = useCallback(
    (e) => {
      if (e.keyCode === 13 && e.shiftKey === false) {
        handleSubmit(e)
      }
    },
    [handleSubmit]
  )

  return (
    <div className="ChatInput">
      <form onSubmit={handleSubmit} className="ChatInput__InputForm">
        <AutosizedTextarea
          className="ChatInput__InputTextField"
          placeholder="Skriv besked"
          value={content}
          minHeight={0}
          maxHeight={maxHeight}
          onChange={handleInput}
          onKeyDown={handleEnterKeyDown}
        />
        <button
          disabled={content === ""}
          type="submit"
          className="ChatInput__BtnSendContent"
        >
          Send
        </button>
      </form>
    </div>
  )
}
