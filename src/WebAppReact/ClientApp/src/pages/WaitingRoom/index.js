import React, { useCallback, useState, useContext } from "react"
import { useParams, useHistory } from "react-router-dom"
import axios from "axios"

import ThemeContext from "~/contexts/ThemeContext"
import Layout from "~/components/Layout"
import CallPreview from "~/components/CallPreview"
import useInterval from "~/hooks/useInterval"
import useHub from "~/hooks/useHub"

const pollWaitingTimeIntervalSeconds = 60

export default function WaitingRoom() {
  const { supportTicketId } = useParams()
  const [positionInQueue, setPositionInQueue] = useState(null)
  const { logo, prefix } = useContext(ThemeContext)

  let history = useHistory()

  const onHubConnected = useCallback(
    (connection) => {
      connection.invoke("JoinGroup", supportTicketId)

      connection.on("technicianhasstartedcall", ({ supportTicketId }) => {
        history.push(`${prefix}/service-call/${supportTicketId}`)
      })
    },
    [history, supportTicketId, prefix]
  )

  useHub("/hubs/customer", onHubConnected)

  useInterval(
    () => {
      axios
        .get(
          `api/signup/estimatedwaitingtime?supportticketid=${supportTicketId}`
        )
        .then((result) => {
          setPositionInQueue(result.data.numberOfUnansweredCalls + 1)
        })
        .catch((error) => {
          console.log(error)
        })
    },
    pollWaitingTimeIntervalSeconds * 1000,
    [supportTicketId]
  )

  let positionMessage
  switch (positionInQueue) {
    case null: {
      positionMessage = "Beregner plads i køen"
      break
    }
    case 1: {
      positionMessage = "Du er den næste i køen."
      break
    }
    default: {
      positionMessage = `Du er nummer ${positionInQueue} i køen.`
      break
    }
  }

  return (
    <Layout>
      <img className="mb-8 w-1/3 mx-auto" src={logo} alt="" />
      <div className="text-center">
        <h1 className="text-xl mb-5">Velkommen til Care1 Live</h1>
        <CallPreview />
        <p className="mt-5">{positionMessage}</p>
      </div>
    </Layout>
  )
}
