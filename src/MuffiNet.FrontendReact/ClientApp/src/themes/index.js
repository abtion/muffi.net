import appleLogo from "./AppleLogo.png"
import care1Logo from "./Care1Logo.png"
import codanLogo from "./CodanLogo.png"
import gjensidigeLogo from "./GjensidigeLogo.png"
import samsungLogo from "./SamsungLogo.png"
import telenorLogo from "./TelenorLogo.png"
import teliaLogo from "./TeliaLogo.png"

export const defaultTheme = {
  logo: care1Logo,
  prefix: "/care1",
}

export default [
  defaultTheme,
  {
    logo: appleLogo,
    prefix: "/apple",
  },
  {
    logo: codanLogo,
    prefix: "/codan",
  },
  {
    logo: gjensidigeLogo,
    prefix: "/gjensidige",
  },
  {
    logo: samsungLogo,
    prefix: "/samsung",
  },
  {
    logo: telenorLogo,
    prefix: "/telenor",
  },
  {
    logo: teliaLogo,
    prefix: "/telia",
  },
]
