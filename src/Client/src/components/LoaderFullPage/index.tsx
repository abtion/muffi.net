import Loader from "../Loader"
import "./style.scss"

export default function LoaderFullPage({
  text,
}: {
  text?: string
}): JSX.Element {
  return (
    <div className="LoaderFullPage">
      <div className="LoaderFullPage__loader">
        <Loader text={text} />
      </div>
    </div>
  )
}
