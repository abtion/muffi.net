import React from "react"
import Layout from "~/components/Layout"
import ExampleContainer from "~/components/ExampleContainer"

export default function Home() {
  return (
    <Layout>
      <h1 className="text-2xl mb-3">Title</h1>

      <ExampleContainer connectionOptions={undefined} accessToken={undefined} />
    </Layout>
  )
}
