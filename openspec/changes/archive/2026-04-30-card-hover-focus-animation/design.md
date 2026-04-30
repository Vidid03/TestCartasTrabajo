## Context

Cards are displayed in overlapping arcs (`CardsInPlay` and `CardsLowerDeck`) and already use directional rotations toward a shared reference. Users need hover feedback that temporarily prioritizes one card visually without permanently disturbing the arranged fan/stack.

## Goals / Non-Goals

**Goals:**
- Add smooth hover in/out animation for cards (~0.15s).
- Bring hovered card visually on top while hovered.
- Apply subtle scale-up and slight directional offset toward reference.
- Restore exact original state on hover exit.

**Non-Goals:**
- Rebuilding card layout system.
- Gameplay/card rule logic changes.
- New art or style assets.

## Decisions

1. Use a per-card Unity component implementing pointer events (`IPointerEnterHandler`/`IPointerExitHandler`).
   - Rationale: Local, reusable, explicit control per card.
2. Cache base state on start (anchored position, local scale, sibling index) and restore from cache on exit.
   - Rationale: Guarantees exact restoration.
3. Animate with time-based interpolation over configurable duration (default `0.15f`).
   - Rationale: Deterministic smoothness independent of framerate spikes.
4. Compute hover offset direction using vector from card to `Reference` in the same UI space.
   - Rationale: Matches existing directional visual language.

## Risks / Trade-offs

- [Risk] Rapid pointer movement can interrupt transitions -> Mitigation: restart animation from current interpolated state.
- [Risk] Multiple cards hovered quickly can cause unexpected stacking if indexes are not restored -> Mitigation: preserve and restore original sibling index per card.
- [Trade-off] Slight per-frame interpolation cost on hovered cards -> Mitigation: animation runs only during short hover transitions.
