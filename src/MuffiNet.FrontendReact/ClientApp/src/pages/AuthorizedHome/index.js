import React, { useMemo } from "react"

import AuthorizedLayout from "~/components/AuthorizedLayout"
import ExampleContainer from "~/components/ExampleContainer"
import { HttpTransportType } from "@microsoft/signalr"

export default function AuthorizedHome({ accessToken }) {
  const connectionOptions = useMemo(
    () => ({
      accessTokenFactory: () => accessToken,
      transport: HttpTransportType.LongPolling,
    }),
    [accessToken]
  )

  return (
    <AuthorizedLayout>
      <div className="AuthorizedHome container mt-5">
        <ExampleContainer
          connectionOptions={connectionOptions}
          accessToken={accessToken}
        />
      </div>
    </AuthorizedLayout>
  )
}
