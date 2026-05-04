## Why

Cards currently remain static unless the user interacts with them. We need a subtle continuous motion to keep the hand visually alive and readable, while preserving existing hover, selection, and transfer behaviors.

## What Changes

- Add a per-card idle loop animation that continuously applies a small Z rotation.
- Idle rotation SHALL oscillate slowly between a base range of `-5` and `+5` degrees.
- Each card SHALL randomize that range with a small per-card threshold offset of `+-2` degrees.
- Idle rotation SHALL remain active at all times, including while other card animations are running.
- Idle motion SHALL be additive to current card rotation so existing layout and interaction flows remain intact.

## Capabilities

### New Capabilities
- `ui-card-idle-loop-rotation`: Provides continuous per-card idle Z rotation with bounded random variation.

### Modified Capabilities
- `ui-card-hover-focus`: Integrates with always-on idle rotation so hover/selection animations coexist without disabling idle motion.

## Impact

- Affected area: card UI interaction/animation script (`CardHoverFocus`).
- Affected visual behavior: card rotation now has an always-on subtle oscillation.
- No backend, API, or dependency changes.
