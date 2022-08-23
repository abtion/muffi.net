// import React, { useCallback, useState } from "react"
// import { render } from "@testing-library/react"
// import userEvent from "@testing-library/user-event"
// import Dialog from "."

// function DialogTest() {
//   const [isOpen, setOpen] = useState(false)

//   function open() {
//     setOpen(true)
//   }

//   function close() {
//     setOpen(false)
//   }

//   return (
//     <>
//       <input type="text" value="my input" autoFocus />
//       <button onClick={close}>Show dialog</button>
//       <Dialog isOpen={isOpen} onClose={close}>
//         <Dialog.Header title="my title">my header</Dialog.Header>
//         <Dialog.Content>my content</Dialog.Content>
//         <Dialog.Footer>
//           my footer
//           <button onClick={close}>Cancel</button>
//         </Dialog.Footer>
//       </Dialog>
//     </>
//   )
// }

// function setup() {
//   return render(<DialogTest />)
// }

// TODO figure out how to implement these tests (or drop them)
//      `jest` and/or `js-dom` do not implement proper <modal> element behavior, so this can't be tested at the moment

test(`a closed Dialog is initially hidden`, () => {
  // const { getByRole } = setup();
  // expect(getByRole(HTMLDialogElement)).toBeInTheDocument();
})

// test(`can close modal Dialog using the isOpen prop`, () => {})

// test(`can close modal Dialog using "x" icon`, () => {})

// test(`can close modal Dialog using ESC button (on keyboard)`, () => {})

// test(`can restore focus to last active element after closing Dialog`, () => {})
