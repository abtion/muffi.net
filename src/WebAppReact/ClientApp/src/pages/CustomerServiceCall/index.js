import React, { useCallback, useEffect, useState, useContext } from "react"
import { useParams, useHistory } from "react-router-dom"
import axios from "axios"

import CustomerRoom from "~/components/CustomerRoom"
import useHub from "~/hooks/useHub"
import ThemeContext from "~/contexts/ThemeContext"
import Spinner from "~/components/Spinner"
import getFirstName from "~/utils/getFirstName"

export default function CustomerServiceCall() {
  let { supportTicketId } = useParams()
  let history = useHistory()
  const { prefix } = useContext(ThemeContext)

  const [isLoading, setIsLoading] = useState(true)
  const [twilioAccessToken, setTwilioAccessToken] = useState(null)
  const [ossCaseId, setOssCaseId] = useState(null)
  const [technicianName, setTechnicianName] = useState("Tekniker")

  useEffect(() => {
    setIsLoading(true)

    axios
      .get(`api/signup/roomgranttoken?supportticketid=${supportTicketId}`)
      .then((result) => {
        const {
          twilioVideoGrantForCustomerToken: twilioAccessToken,
          technicianFullName,
          ossId,
        } = result.data.token

        if (!twilioAccessToken) {
          history.push(prefix)
          return
        }
        if (technicianFullName)
          setTechnicianName(getFirstName(technicianFullName))
        if (ossId) setOssCaseId(ossId)
        setTwilioAccessToken(twilioAccessToken)
        setIsLoading(false)
      })
  }, [history, supportTicketId, prefix])

  const onHubConnected = useCallback(
    (connection) => {
      connection.invoke("JoinGroup", supportTicketId)

      connection.on("technicianhasendedcall", (message) => {
        history.push(`${prefix}/thank-you`)
      })

      connection.on("technicianhascreatedosscase", (message) => {
        setOssCaseId(message.ossId)
      })
    },
    [history, supportTicketId, prefix]
  )

  useHub("/hubs/customer", onHubConnected)

  if (isLoading) {
    return <Spinner className="mx-auto mt-10" />
  }

  return (
    <CustomerRoom
      accessToken={twilioAccessToken}
      ossId={ossCaseId}
      technicianName={technicianName}
    />
  )
}
