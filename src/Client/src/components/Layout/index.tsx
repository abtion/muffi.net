export default function Layout({ children }: LayoutProps): JSX.Element {
  return <div className="container py-8 max-w-screen-sm">{children}</div>
}

type LayoutProps = {
  children?: JSX.Element[]
}
