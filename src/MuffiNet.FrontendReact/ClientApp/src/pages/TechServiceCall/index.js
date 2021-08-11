import React, { useEffect, useState } from "react"
import { useParams, useHistory } from "react-router-dom"
import axios from "axios"

import TechLayout from "~/components/TechLayout"
import TechRoom from "~/components/TechRoom"
import Spinner from "~/components/Spinner"

export default function TechServiceCall({ accessToken }) {
  let { supportTicketId } = useParams()
  let history = useHistory()

  const [isLoading, setIsLoading] = useState(true)
  const [supportTicket, setSupportTicket] = useState({})
  const [twilioAccessToken, setTwilioAccessToken] = useState(null)

  useEffect(() => {
    setIsLoading(true)

    axios
      .get(`api/technician/supportticket?supportticketid=${supportTicketId}`, {
        headers: {
          authorization: `Bearer ${accessToken}`,
        },
      })
      .then((result) => {
        const { supportTicket } = result.data
        const { twilioVideoGrantForTechnicianToken: twilioAccessToken } =
          supportTicket

        if (!twilioAccessToken) {
          history.push(`/technician`)
          return
        }

        setSupportTicket(supportTicket)
        setTwilioAccessToken(twilioAccessToken)
        setIsLoading(false)
      })
  }, [history, supportTicketId, accessToken])

  if (isLoading) {
    return <Spinner className="mx-auto mt-10" />
  }

  return (
    <TechLayout>
      <TechRoom
        accessToken={accessToken}
        twilioAccessToken={twilioAccessToken}
        supportTicket={supportTicket}
      />
    </TechLayout>
  )
}
