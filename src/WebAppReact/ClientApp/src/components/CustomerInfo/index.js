import React, { useState, useCallback } from "react"
import { DateTime } from "luxon"
import axios from "axios"

import "./style.scss"
import Button from "~/components/Button"
import Spinner from "~/components/Spinner"

export default function CustomerInfo({ supportTicket, accessToken }) {
  const {
    customerName,
    customerEmail,
    customerPhone,
    brand,
    createdAt,
    supportTicketId,
    ossId,
  } = supportTicket

  const [ossCaseId, setOssId] = useState(ossId)
  const [isLoading, setIsLoading] = useState(false)

  const handleCreateOssCase = useCallback(() => {
    setIsLoading(true)

    axios
      .get(
        `api/Technician/requestossidfromoss?supportTicketId=${supportTicketId}`,
        {
          headers: {
            authorization: `Bearer ${accessToken}`,
          },
        }
      )
      .then((result) => {
        const { ossId } = result.data

        if (ossId) setOssId(ossId)
        setIsLoading(false)
      })
      .catch((error) => {
        console.log(error)
        setIsLoading(false)
      })
  }, [supportTicketId, accessToken])

  return (
    <table className="CustomerInfo">
      <tbody>
        <tr>
          <td>Navn</td>
          <td>{customerName}</td>
        </tr>
        <tr>
          <td>Email</td>
          <td>{customerEmail}</td>
        </tr>
        <tr>
          <td>Tlf.</td>
          <td>{customerPhone}</td>
        </tr>
        <tr>
          <td>Brand</td>
          <td>{brand}</td>
        </tr>
        <tr>
          <td>Oprettet</td>
          <td>
            {DateTime.fromISO(createdAt).toLocaleString(
              DateTime.TIME_24_WITH_SHORT_OFFSET
            )}
          </td>
        </tr>
        <tr>
          <td>SupportTicketId</td>
          <td>{supportTicketId}</td>
        </tr>
        <tr>
          {ossCaseId ? (
            <>
              <td>Servicenummer</td>
              <td>{ossCaseId}</td>
            </>
          ) : isLoading ? (
            <td>
              <Spinner />
            </td>
          ) : (
            <td>
              <Button size="sm" color="primary" onClick={handleCreateOssCase}>
                Create Oss Case
              </Button>
            </td>
          )}
        </tr>
      </tbody>
    </table>
  )
}
