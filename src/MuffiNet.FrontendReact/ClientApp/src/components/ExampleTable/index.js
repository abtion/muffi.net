import React from "react"
import Table from "~/components/Table"

export default function ExampleTable({ entities }) {
  return (
    <Table>
      <thead>
        <tr>
          <th>Id</th>
          <th>Name</th>
          <th>Description</th>
          <th>Email</th>
          <th>Phone</th>
        </tr>
      </thead>
      <tbody>
        {entities.map((entity, index) => (
          <tr key={index}>
            <td>{entity.id}</td>
            <td>{entity.name}</td>
            <td>{entity.description}</td>
            <td>{entity.email}</td>
            <td>{entity.phone}</td>
          </tr>
        ))}
      </tbody>
    </Table>
  )
}
