import { useEffect, useState } from "react"
import {
  HubConnection,
  HubConnectionBuilder,
  IHttpConnectionOptions,
} from "@microsoft/signalr"

export interface onHubConnected {
  (connection: HubConnection): void
}

export default function useHub(
  path: string,
  onHubConnected: onHubConnected,
  connectionOptions?: IHttpConnectionOptions
): void {
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
      // eslint-disable-next-line no-console
      .catch((error: Error) => console.error(error))

    return () => connection.stop()
  }, [connection, connectionOptions, onHubConnected])
}
