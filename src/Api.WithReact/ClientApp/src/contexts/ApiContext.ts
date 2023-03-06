import axios, { AxiosInstance } from "axios"
import { createContext } from "react"

export default createContext<AxiosInstance>(axios)
