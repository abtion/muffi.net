// Mock the request issued by the react app to get the client configuration parameters.

// @ts-expect-error: We are overriding window.fetch on purpose to make authentication work.
// TODO: Only override fetch for the authentication URL
window.fetch = () => {
  return Promise.resolve({
    ok: true,
    json: () =>
      Promise.resolve({
        /* eslint-disable camelcase */
        authority: "https://localhost:5001",
        client_id: "MuffiNet.FrontendReact",
        redirect_uri: "https://localhost:5001/authentication/login-callback",
        post_logout_redirect_uri:
          "https://localhost:5001/authentication/logout-callback",
        response_type: "id_token token",
        scope: "MuffiNet.FrontendReactAPI openid profile",
      }),
  })
}
