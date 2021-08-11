import React, { useContext, useState } from "react"
import { useHistory } from "react-router-dom"
import axios from "axios"

import Layout from "~/components/Layout"
import ThemeContext from "~/contexts/ThemeContext"

import "./style.scss"
import Button from "~/components/Button"
import Input from "~/components/Input"
import Select from "~/components/Select"

export default function SignUp() {
  const history = useHistory()
  const [formData, setFormData] = useState({
    CustomerName: "",
    CustomerEmail: "",
    CustomerPhone: "",
    Brand: "",
    RecordMeConsent: false,
    PrivacyConsent: false,
  })
  const { logo, prefix } = useContext(ThemeContext)

  const handleSubmit = (e) => {
    e.preventDefault()
    createSupportTicket()
  }

  const handleInput = ({ currentTarget: { type, name, value, checked } }) => {
    if (name === "CustomerPhone" || name === "CustomerEmail") {
      value = value.replace(/ /g, "")
    }

    if (type === "checkbox") {
      value = checked
    }

    setFormData({
      ...formData,
      [name]: value,
    })
  }

  const createSupportTicket = () => {
    return axios
      .post("/api/signup", formData)
      .then((response) => {
        const { supportTicketId } = response.data

        history.push(`${prefix}/waiting-room/${supportTicketId}`)
      })
      .catch((error) => {
        console.log(error)
      })
  }

  return (
    <Layout>
      <img className="mb-8 w-1/3 mx-auto" src={logo} alt="" />
      <p className="text-lg mb-5">
        Udfyld dine oplysninger, så vores tekniker er klar til at hjælpe dig.
      </p>
      <form onSubmit={handleSubmit}>
        <div className="mb-5">
          <label className="block mb-1" htmlFor="nameInput">
            Navn
          </label>
          <Input
            id="nameInput"
            name="CustomerName"
            type="text"
            value={formData.CustomerName}
            onChange={handleInput}
            required
          />
        </div>
        <div className="mb-5">
          <label className="block mb-1" htmlFor="emailInput">
            E-mail
          </label>
          <Input
            id="emailInput"
            name="CustomerEmail"
            type="email"
            inputMode="email"
            value={formData.CustomerEmail}
            onChange={handleInput}
            required
          />
        </div>
        <div className="mb-5">
          <label className="block mb-1" htmlFor="phoneInput">
            Telefon
          </label>
          <Input
            id="phoneInput"
            name="CustomerPhone"
            type="text"
            inputMode="tel"
            value={formData.CustomerPhone}
            onChange={handleInput}
            required
            pattern="^[+]?[0-9]{7,15}$"
          />
        </div>
        <div className="mb-5">
          <label className="block mb-1" htmlFor="brandSelect">
            Vælg din enhed
          </label>
          <Select
            id="brandSelect"
            name="Brand"
            value={formData.Brand}
            onChange={handleInput}
            required
          >
            <option value="">Vælg mærke</option>
            <option value="apple">Apple</option>
            <option value="Andet mærke">Andet mærke</option>
          </Select>
        </div>

        <div className="mb-4">
          <div className="mb-2 flex items-start">
            <input
              required
              id="recordConsent"
              name="RecordMeConsent"
              type="checkbox"
              checked={formData.RecordMeConsent}
              onChange={handleInput}
              className="mr-2 mt-1"
            />
            <label htmlFor="recordConsent">
              Jeg accepterer, at Live opkaldet bliver optaget og gemt, så længe
              sagen er aktiv.
            </label>
          </div>
          <div className="mb-1 flex items-start">
            <input
              required
              id="privacyPolicyConsent"
              name="PrivacyConsent"
              type="checkbox"
              checked={formData.PrivacyConsent}
              onChange={handleInput}
              className="mr-2 mt-1"
            />
            <label htmlFor="privacyPolicyConsent">
              Jeg accepterer Care1's{" "}
              <a
                href="https://care1.dk/om-care1/privatlivspolitik/"
                target="_blank"
                rel="noreferrer"
              >
                privatlivspolitik
              </a>
              .
            </label>
          </div>
        </div>
        <div>
          <Button color="success">Videre</Button>
        </div>
      </form>
    </Layout>
  )
}
