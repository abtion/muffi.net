import getFirstName from "./getFirstName"

describe(getFirstName, () => {
  test("works as intended", () => {
    expect(getFirstName("Bob Johnson Augustus the third")).toBe("Bob")
    expect(getFirstName("Bob Johnson")).toBe("Bob")
    expect(getFirstName("Bob")).toBe("Bob")
    expect(getFirstName("")).toBe("")
  })
})
