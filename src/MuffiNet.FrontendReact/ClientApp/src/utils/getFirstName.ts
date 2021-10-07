export default function getFirstName(fullName: string): string {
  return fullName.split(/\s/)[0]
}
