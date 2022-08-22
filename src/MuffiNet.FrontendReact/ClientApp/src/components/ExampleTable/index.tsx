import React from "react"
import Table from "~/components/Table"
import Button from "~/components/Button"
import { ExampleEntity } from "~/types/ExampleEntity"
import Size from "~/const/size"

export default function ExampleTable({
  entities,
  onRemove,
}: ExampleTableProps): JSX.Element {
  const handleClick = (
    event: React.MouseEvent<HTMLElement>,
    id: string | number
  ) => {
    event.preventDefault()
    onRemove(id)
  }

  return (
    <Table>
      <thead>
        <tr>
          <th>Id</th>
          <th>Name</th>
          <th>Description</th>
          <th>Email</th>
          <th>Phone</th>
          <th></th>
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
            <td>
              <Button
                size={Size.Small}
                onClick={(e) => handleClick(e, entity.id)}
              >
                Remove
              </Button>
            </td>
          </tr>
        ))}
      </tbody>
    </Table>
  )
}

type ExampleTableProps = {
  entities: ExampleEntity[]
  onRemove: (id: string | number) => void
}
