## Why

The blackjack project needs a reusable Unity UI layout scaffold based on the provided reference so gameplay can be integrated quickly without waiting on final art. Creating a default-style template now allows iteration on flow, spacing, and card placement while keeping visuals intentionally placeholder.

## What Changes

- Create a Unity UI template screen based on `References/Template.png` using default white buttons and default/basic image components only (no custom art).
- Add top controls: back button (upper-left) and settings button (upper-right).
- Add bottom controls: three buttons for trash, upload, and suit.
- Add center text elements: `999,999`, adjacent `999,999`, and `three of a kind`.
- Add card placeholders with curved composition:
  - 5 cards in the main in-play row/arc.
  - ~10 cards in a lower row/arc, matching the slightly circular fan-like arrangement in the reference.
- Keep implementation template-only (layout and placeholders), without gameplay logic.

## Capabilities

### New Capabilities
- `blackjack-ui-template-layout`: Builds a default Unity UI screen template for blackjack with required buttons, text counters, status text, and curved card placeholder groups derived from the reference image.

### Modified Capabilities
- None.

## Impact

- Affected area: Unity scene UI hierarchy and RectTransform layout data.
- No API or backend changes.
- No external dependencies required.
- Enables subsequent implementation tasks for wiring gameplay state and interactions to an already-structured UI.
