## Context

Cards already support hover animation, top-layer priority on hover, and deterministic base ordering. The new requirement introduces persistent selection with visual lock semantics and cross-card constraints by group.

## Goals / Non-Goals

**Goals:**
- Toggle selection on click for hovered cards.
- Red outline when selected, black outline when not selected.
- Keep selected cards visually in hover-locked state.
- Prevent normal hover transitions while selected.
- Allow multiple selected cards in the same group.
- Disallow new selections from the other group while one group has active selections.

**Non-Goals:**
- Gameplay action execution on selected cards.
- Networking/saving selected state outside current scene runtime.
- Changing existing card arc layout rules.

## Decisions

1. Extend `CardHoverFocus` to also implement click selection (`IPointerClickHandler`).
   - Rationale: Selection is tightly coupled with hover state and outline visuals.
2. Keep per-group selection registry using static runtime state keyed by parent group transform.
   - Rationale: Efficient enforcement of same-group selection rule.
3. Represent selection visually via `Outline.effectColor` switch black/red.
   - Rationale: Matches existing outline setup and user requirement.
4. On select, force and retain hover target transform state (position/scale/top layer).
   - Rationale: Selected cards must remain in hover-like state even without pointer.
5. On deselect, immediately transition to default hover-exit path and restore black outline.
   - Rationale: Keeps behavior predictable and reversible.

## Risks / Trade-offs

- [Risk] Fast pointer/click sequences can desync hover and selection state -> Mitigation: single source of truth flags (`_isHovered`, `_isSelected`) and guarded transition paths.
- [Risk] Group lock cleanup when last selected card is deselected -> Mitigation: clear active-group lock whenever selected count reaches zero.
- [Trade-off] Added state complexity in one component -> Mitigation: isolate selection utility methods and explicit state transitions.
