import React, { useEffect, useReducer } from "react"

import ChatContext from "~/contexts/ChatContext"

import "./style.scss"

const historyReducer = (state, message) => [...state, message]

export default function ChatArea({ children, remoteTrack }) {
  const [history, postMessage] = useReducer(historyReducer, [])

  useEffect(() => {
    const handleMessage = (content, track) => {
      const msgFromJson = JSON.parse(content)
      postMessage({
        timestamp: msgFromJson.timestamp,
        content: msgFromJson.text,
        track: track,
      })
    }

    remoteTrack && remoteTrack.on("message", handleMessage)

    return () => {
      remoteTrack && remoteTrack.off("message", handleMessage)
    }
  }, [remoteTrack, postMessage])

  return (
    <div className="ChatArea">
      <ChatContext.Provider value={{ history, postMessage }}>
        {children}
      </ChatContext.Provider>
    </div>
  )
}
