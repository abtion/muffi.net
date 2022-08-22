import classNames from "classnames";
import React, { useEffect, useRef } from "react";
import "./index.scss";
import Header from "./Header";
import Footer from "./Footer";
import Content from "./Content";

const closeIcon = (
  <svg width="14" height="14" viewBox="0 0 14 14" fill="none" xmlns="http://www.w3.org/2000/svg">
    <path d="M1 13L13 1M1 1L13 13" stroke="#9CA3AF" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
  </svg>
)

type DialogProps = Omit<React.HTMLAttributes<HTMLDialogElement>, "onClick"> & {
  isOpen: boolean;
  onClose: () => void;
}

function Dialog({ children, isOpen, onClose, className, ...rest }: DialogProps): JSX.Element {
  const dialog = useRef<HTMLDialogElement>(null);

  const lastActiveElement = useRef<HTMLElement | null>(null);

  // Show/hide the dialog when `osOpen` is changed, restore focus on close:

  useEffect(() => {
    if (isOpen) {
      lastActiveElement.current = isHTMLElement(document.activeElement)
        ? document.activeElement
        : null;

      dialog.current!.showModal();
    } else {
      dialog.current?.close();

      lastActiveElement.current?.focus();
    }
  }, [isOpen]);

  // Handle `cancel` event: (allows pressing ESC to close)

  useEffect(() => {
    function onCancel(event: Event) {
      event.preventDefault();
      onClose();
    }

    dialog.current!.addEventListener("cancel", onCancel);

    return () => {
      dialog.current!.removeEventListener("cancel", onCancel);
    }
  }, [onClose]);

  return (
    <dialog ref={dialog} className={classNames(className, "Dialog")} {...rest}>
      <button title="Close" onClick={onClose} className="Dialog__Close">{closeIcon}</button>
      {children}
    </dialog>
  );
}

const isHTMLElement = (el: Element | null): el is HTMLElement => el instanceof HTMLElement;

export default Object.assign(
  Dialog,
  {
    Header,
    Footer,
    Content
  }
);
