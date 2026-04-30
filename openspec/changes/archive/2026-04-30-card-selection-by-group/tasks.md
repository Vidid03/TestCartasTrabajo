## 1. Selection State Model

- [x] 1.1 Add selected-state flag and outline references to card interaction component.
- [x] 1.2 Add runtime group-lock tracking to enforce single active selection group.

## 2. Click Toggle Behavior

- [x] 2.1 Implement click handling on hovered cards to toggle selection on/off.
- [x] 2.2 Block cross-group selection attempts while another group has selected cards.

## 3. Visual And Hover Lock Integration

- [x] 3.1 On selection, switch outline color black->red and keep card in hover-locked state.
- [x] 3.2 While selected, bypass normal hover enter/exit transitions.
- [x] 3.3 On deselection, switch outline red->black and restore normal hover-exit behavior.

## 4. Group Consistency And Validation

- [x] 4.1 Ensure selected cards can be multiple within same group.
- [x] 4.2 Ensure group lock is released when last selected card is cleared.
- [x] 4.3 Validate fast interaction edge cases (rapid hover/click transitions) for stable layering and state.
