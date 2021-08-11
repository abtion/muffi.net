import React from "react"
import { useHistory } from "react-router-dom"
import Duration from "~/components/Duration"
import Table from "~/components/Table"
import Button from "../Button"

export default function TableOngoingSupportCalls({ supportTickets }) {
  const history = useHistory()
  const handleGoToRoom = (supportTicketId) => {
    if (window.confirm("Er du sikker på at du vil gå til dette rum?")) {
      history.push(`/technician/service-call/${supportTicketId}`)
    }
  }
  const upperDurationLimit = 60 * 60 * 1000 // 1 hour

  return (
    <Table>
      <thead>
        <tr>
          <th>Navn</th>
          <th>Telefon</th>
          <th>Email</th>
          <th>Brand</th>
          <th>Opkaldstid (min)</th>
          <th>Tekniker</th>
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
              {ticket.callStartedAt ? (
                <Duration
                  since={new Date(ticket.callStartedAt)}
                  format="m"
                  upperLimit={upperDurationLimit}
                />
              ) : (
                ""
              )}
            </td>
            <td>{ticket.technicianFullName}</td>
            <td>
              <Button
                size="sm"
                className="float-right"
                onClick={() => handleGoToRoom(ticket.supportTicketId)}
              >
                Gå til kald
              </Button>
            </td>
          </tr>
        ))}
      </tbody>
    </Table>
  )
}
