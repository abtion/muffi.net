import React, { useState } from "react"

import Button from "~/components/Button"
import Input from "~/components/Input"
import Size from "~/const/size"
import Variant from "~/const/variant"

export default function ExampleForm({
  onSubmit,
}: ExampleFormProps): JSX.Element {
  const [formData, setFormData] = useState({
    Name: "",
    Description: "",
    Email: "",
    Phone: "",
  })

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>): void => {
    e.preventDefault()
    onSubmit(formData)
  }

  const handleInput = ({
    currentTarget: { name, value },
  }: React.ChangeEvent<HTMLInputElement>): void => {
    if (name === "Phone" || name === "Email") {
      value = value.replace(/ /g, "")
    }

    setFormData({
      ...formData,
      [name]: value,
    })
  }

  return (
    <form onSubmit={handleSubmit}>
      <div className="mb-5">
        <label className="block mb-1" htmlFor="nameInput">
          Name
        </label>
        <Input
          size={Size.Medium}
          variant={Variant.Neutral}
          className="w-full block"
          id="nameInput"
          name="Name"
          type="text"
          value={formData.Name}
          onChange={handleInput}
        />
      </div>
      <div className="mb-5">
        <label className="block mb-1" htmlFor="descriptionInput">
          Description
        </label>
        <Input
          size={Size.Medium}
          variant={Variant.Neutral}
          className="w-full block"
          id="descriptionInput"
          name="Description"
          type="text"
          value={formData.Description}
          onChange={handleInput}
          required
        />
      </div>
      <div className="mb-5">
        <label className="block mb-1" htmlFor="emailInput">
          E-mail
        </label>
        <Input
          size={Size.Medium}
          variant={Variant.Neutral}
          className="w-full block"
          id="emailInput"
          name="Email"
          type="email"
          inputMode="email"
          value={formData.Email}
          onChange={handleInput}
          required
        />
      </div>
      <div className="mb-5">
        <label className="block mb-1" htmlFor="phoneInput">
          Phone
        </label>
        <Input
          size={Size.Medium}
          variant={Variant.Neutral}
          className="w-full block"
          id="phoneInput"
          name="Phone"
          type="text"
          inputMode="tel"
          value={formData.Phone}
          onChange={handleInput}
          required
          pattern="^[+]?[0-9]{7,15}$"
        />
      </div>
      <div>
        <Button variant={Variant.Primary} size={Size.Large}>
          Submit
        </Button>
      </div>
    </form>
  )
}

type ExampleFormProps = {
  onSubmit: (formdata: ExampleFormData) => void
}

export type ExampleFormData = {
  Name: string
  Description: string
  Email: string
  Phone: string
}
