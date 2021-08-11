import React from "react"
import { useHistory } from "react-router-dom"
import Duration from "~/components/Duration"
import Table from "~/components/Table"
import axios from "axios"

import Button from "../Button"

export default function TableQueuedSupportCalls({
  supportTickets,
  accessToken,
}) {
  const history = useHistory()
  const handleStartRoom = (supportTicketId) => {
    axios
      .post(
        "/api/technician/createroom",
        { SupportTicketId: supportTicketId },
        {
          headers: {
            authorization: `Bearer ${accessToken}`,
          },
        }
      )
      .then(() => {
        history.push(`/technician/service-call/${supportTicketId}`)
      })
      .catch((error) => {
        console.log(error)
      })
  }
  return (
    <Table>
      <thead>
        <tr>
          <th>Navn</th>
          <th>Telefon</th>
          <th>Email</th>
          <th>Brand</th>
          <th>Tid i kø (min)</th>
          <th></th>
        </tr>
      </thead>
      <tbody>
        {supportTickets.map((ticket, index) => (
          <tr key={index}>
            <td>{ticket.customerName}</td>
            <td>{ticket.customerPhone}</td>
            <td>{ticket.customerEmail}</td>
            <td>{ticket.brand}</td>
            <td>
              <Duration since={new Date(ticket.createdAt)} format="m" />
            </td>
            <td>
              <Button
                size="sm"
                className="float-right"
                onClick={() => handleStartRoom(ticket.supportTicketId)}
              >
                Start
              </Button>
            </td>
          </tr>
        ))}
      </tbody>
    </Table>
  )
}
