import React, { useState, useEffect, useMemo, useCallback } from "react"
import axios from "axios"

import useMappedRecords from "~/hooks/useMappedRecords"
import TechLayout from "~/components/TechLayout"
import TableOngoingSupportCalls from "~/components/TableOngoingSupportCalls"
import TableQueuedSupportCalls from "~/components/TableQueuedSupportCalls"
import Spinner from "~/components/Spinner"

import "./style.css"
import useHub from "~/hooks/useHub"
import { HttpTransportType } from "@microsoft/signalr"

const TechOverview = ({ accessToken }) => {
  const [loading, setLoading] = useState(true)
  const [supportTicketsMap, upsertSupportTickets, deleteSupportTicket] =
    useMappedRecords((supportTicket) => supportTicket.supportTicketId)

  const supportCallTables = useMemo(() => {
    const result = {
      queued: [],
      ongoing: [],
    }

    Object.values(supportTicketsMap).forEach((ticket) => {
      const isStarted = Boolean(ticket.callStartedAt)
      const table = isStarted ? result.ongoing : result.queued

      table.push(ticket)
    })

    return result
  }, [supportTicketsMap])

  const onHubConnected = useCallback(
    (connection) => {
      connection.on("SupportTicketCreated", ({ supportTicket }) => {
        upsertSupportTickets(supportTicket)
      })

      connection.on("SupportTicketUpdated", ({ supportTicket }) => {
        upsertSupportTickets(supportTicket)
      })

      connection.on("SupportTicketDeleted", ({ supportTicketId }) => {
        deleteSupportTicket({ supportTicketId })
      })
    },
    [upsertSupportTickets, deleteSupportTicket]
  )

  const connectionOptions = useMemo(
    () => ({
      accessTokenFactory: () => accessToken,
      transport: HttpTransportType.LongPolling,
    }),
    [accessToken]
  )

  useHub("/hubs/technician", onHubConnected, connectionOptions)

  useEffect(() => {
    axios
      .get("/api/technician/allsupporttickets", {
        headers: {
          authorization: `Bearer ${accessToken}`,
        },
      })
      .then((response) => {
        const { supportTickets } = response.data
        upsertSupportTickets(supportTickets)
        setLoading(false)
      })
      .catch((error) => {
        console.log(error)
      })
  }, [accessToken, upsertSupportTickets])

  return (
    <TechLayout>
      <div className="container mt-5">
        <h1 className="text-2xl mb-6">Supportkald</h1>

        {loading ? (
          <Spinner style={{ position: "absolute", left: "50%", top: "50%" }} />
        ) : (
          <>
            <h2 className="text-xl mb-2">I kø</h2>
            <TableQueuedSupportCalls
              supportTickets={supportCallTables.queued}
              accessToken={accessToken}
            />

            <h2 className="text-xl mb-2 mt-8">Igangværende</h2>
            <TableOngoingSupportCalls
              supportTickets={supportCallTables.ongoing}
            />
          </>
        )}
      </div>
    </TechLayout>
  )
}

export default TechOverview
