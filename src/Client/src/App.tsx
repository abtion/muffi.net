import AuthOidcProvider from "~/components/AuthOidcProvider"
import AuthBarrier from "~/components/AuthBarrier"

// Visitors
import Home from "~/pages/Home"

// Authorized pages
import AuthorizedHome from "~/pages/AuthorizedHome"
import UserRoleAdmin from "~/pages/UserRoleAdmin"
import { BrowserRouter, Route, Routes } from "react-router-dom"

export default function App(): JSX.Element {
  return (
    <BrowserRouter>
      <Routes>
        <Route index element={<Home />} />
        <Route
          path="/admin/*"
          element={
            <AuthOidcProvider>
              <AuthBarrier>
                <Routes>
                  <Route path="roles" element={<UserRoleAdmin />} />
                  <Route index element={<AuthorizedHome />} />
                </Routes>
              </AuthBarrier>
            </AuthOidcProvider>
          }
        />
      </Routes>
    </BrowserRouter>
  )
}
