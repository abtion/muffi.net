import { useEffect, useState } from "react"
import { HubConnectionBuilder } from "@microsoft/signalr"

export default function useHub(path, onHubConnected, connectionOptions) {
  const [connection, setConnection] = useState(null)

  useEffect(() => {
    const connect = new HubConnectionBuilder()
      .withUrl(path, connectionOptions)
      .withAutomaticReconnect()
      .build()
    setConnection(connect)
  }, [path, setConnection, connectionOptions])

  useEffect(() => {
    if (!connection) return

    connection
      .start()
      .then(() => {
        onHubConnected(connection)
      })
      .catch((e) => console.log("Connection failed: ", e))

    return () => connection.stop()
  }, [connection, connectionOptions, onHubConnected])
}
