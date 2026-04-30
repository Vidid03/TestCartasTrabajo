## Why

The current hover behavior improves readability, but cards cannot be explicitly marked for user intent. A selection layer is needed so hovered cards can be locked for interaction planning while preserving visual clarity.

## What Changes

- Add click-based card selection toggle for UI cards.
- Selecting a hovered card SHALL change its outline color from black to red.
- While selected, a card SHALL remain in hover visual state even when the pointer exits.
- While selected, hover enter/exit transitions SHALL be ignored for that card.
- Hover-exit reset logic triggered by other cards SHALL NOT reset cards that are currently selected.
- Clicking a selected card again SHALL deselect it, restore black outline, and allow hover-exit behavior again.
- Support multi-select within the same group.
- Enforce group-exclusive selection:
  - If at least one selected card belongs to `CardsInPlay`, only cards in `CardsInPlay` can be newly selected.
  - If at least one selected card belongs to `CardsLowerDeck`, only cards in `CardsLowerDeck` can be newly selected.

## Capabilities

### New Capabilities
- `ui-card-selection-lock`: Enables click toggle selection with red/black outline state, hover-lock while selected, and group-exclusive multi-selection rules for card UI groups.

### Modified Capabilities
- `ui-card-hover-focus`: Integrates hover transitions with selection lock state so selected cards do not process hover-exit/hover-enter in normal mode.

## Impact

- Affected area: card UI interaction scripts (`CardHoverFocus` and/or companion selection manager).
- Affected visual state: outline color and layered state control for selected cards.
- No backend/API changes.
