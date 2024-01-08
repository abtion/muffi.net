import classNames from "classnames"
import { Link } from "react-router-dom"

export default function NavLink(props: NavLinkProps): JSX.Element {
  const usedProps = {
    ...props,
    className: classNames(
      "py-2 px-2 md:py-4 text-gray-700 hover:text-gray-900 block",
      props.className,
    ),
  }
  return <Link {...usedProps} />
}

type NavLinkProps = {
  to: string
  tag?: JSX.IntrinsicElements
  className?: string
  children: string
}
