import React, { useContext } from "react"
import { Link } from "react-router-dom"

import Button from "~/components/Button"
import Layout from "~/components/Layout"
import ThemeContext from "~/contexts/ThemeContext"

export default function Home() {
  const { logo, prefix } = useContext(ThemeContext)

  return (
    <Layout>
      <img className="mb-8 w-1/3 mx-auto" src={logo} alt="" />
      <div className="aspect-w-16 aspect-h-9 mb-10">
        <iframe
          title="My title"
          src="https://www.youtube.com/embed/atsIcDoiZYA?rel=0"
          allowFullScreen
        ></iframe>
      </div>

      <h1 className="text-2xl mb-3">Altid lige ved hånden</h1>
      <p className="text-lg mb-4">
        Få rådgivning af nogle af Danmarks dygtigste, autoriserede teknikere
        inden for IT- og mobiltelefoni. Vores uddannede teknikere sidder klar
        til at hjælpe dig, og du er blot få klik fra at få den rådgivning, du
        behøver!
      </p>
      <p className="text-lg mb-4">
        Vi anbefaler så vidt muligt ikke at benytte den enhed, det drejer sig
        om. Hav venligst dit serienummer eller IMEI nummer klar, hvis du
        alligevel benytter enheden.
      </p>
      <Link to={`${prefix}/sign-up`}>
        <Button color="success">Start Live Service</Button>
      </Link>
    </Layout>
  )
}
