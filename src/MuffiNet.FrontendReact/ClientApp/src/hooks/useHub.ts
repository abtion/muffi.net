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
  const [connection, setConnection] = useState<HubConnection | undefined>(undefined)

  useEffect(() => {
    const builder = new HubConnectionBuilder()
    connectionOptions ? builder.withUrl(path, connectionOptions) : builder.withUrl(path)

    const connection = builder
      .withAutomaticReconnect()
      .build()

    setConnection(connection)
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

    return () => { connection.stop() }
  }, [connection, connectionOptions, onHubConnected])
}
