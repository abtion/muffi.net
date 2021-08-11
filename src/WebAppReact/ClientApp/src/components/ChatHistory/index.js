import React, { useContext, useRef, useEffect } from "react"
import Linkify from "linkifyjs/react"
import { DateTime } from "luxon"

import ChatContext from "~/contexts/ChatContext"

import "./style.scss"

export default function ChatHistory({ localTrack, remoteName }) {
  const { history } = useContext(ChatContext)

  const messagesEndRef = useRef(null)
  const scrollToBottom = () => {
    messagesEndRef.current?.scrollIntoView({ behavior: "smooth" })
  }

  useEffect(() => {
    scrollToBottom()
  }, [history])

  return (
    <div className="ChatHistory">
      {history.map(({ timestamp, content, track }, index, arr) => {
        const addNameToMsg =
          index === 0 || (index > 0 && track.name !== arr[index - 1].track.name)
        return (
          <div key={index}>
            {addNameToMsg && (
              <div className="message-timestamp">
                {DateTime.fromMillis(timestamp).toLocaleString({
                  hour: "2-digit",
                  minute: "2-digit",
                  hour12: false,
                })}
              </div>
            )}
            <div
              className={`message ${
                track.name === localTrack.name
                  ? "local-message"
                  : "remote-message"
              }`}
            >
              {addNameToMsg && (
                <div className="message-sender">
                  {track.name === localTrack.name ? "Dig" : remoteName}
                </div>
              )}
              <Linkify options={{ target: { url: "_blank" } }}>
                {content}
              </Linkify>
            </div>
          </div>
        )
      })}
      <div ref={messagesEndRef} />
    </div>
  )
}
