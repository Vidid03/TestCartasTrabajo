## Why

The card layout now looks correct, but interacting with stacked cards is visually unclear because hovered cards remain partially occluded. Adding a smooth hover focus behavior improves readability and perceived polish while keeping the existing template structure.

## What Changes

- Add hover interaction for all playable/visible UI cards in both card groups.
- On pointer enter, hovered card SHALL:
  - render above other cards,
  - scale up slightly,
  - move slightly toward the configured reference point direction.
- On pointer exit, card SHALL smoothly return to its exact pre-hover state (position, scale, render order).
- Hover transitions SHALL be smooth (target duration ~0.15 seconds), not instant.
- Keep behavior compatible with existing curved/rotated card arrangement.

## Capabilities

### New Capabilities
- `ui-card-hover-focus`: Provides smooth card hover focus animation (raise, enlarge, move toward reference, restore) for stacked Unity UI cards.

### Modified Capabilities
- None.

## Impact

- Affected area: Unity UI card GameObjects and interaction scripts/components.
- No backend/API/dependency changes expected.
- Enables clearer card inspection and improved UX for stacked card presentation.
