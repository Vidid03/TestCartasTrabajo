## Context

Cards already animate position and scale for hover/selection, and card layout scripts set base Z rotations for fan arcs. The new requirement adds a continuous idle sway that must remain active even when those existing animations run.

## Goals / Non-Goals

**Goals:**
- Add continuous per-card idle loop rotation in Z.
- Keep nominal idle range centered around `[-5, +5]` degrees.
- Apply per-card random threshold offsets up to `+-2` degrees.
- Keep idle behavior active concurrently with hover/selection/transfer animations.

**Non-Goals:**
- Replacing existing card layout arc logic.
- Adding timeline/Animator assets.
- Changing card gameplay logic.

## Decisions

1. Extend `CardHoverFocus` with idle rotation support.
   - Rationale: It is already attached to the card UI elements and centralizes card interaction animation behavior.
2. Apply idle rotation in `LateUpdate` as an additive overlay.
   - Rationale: Running after `Update` helps preserve coexistence with other scripts that animate cards in `Update`.
3. Use per-card randomized loop parameters (phase, duration, min/max offsets within threshold).
   - Rationale: Avoids synchronized movement and satisfies the requirement for individual variation.
4. Keep configuration serialized (nominal min/max, threshold, duration).
   - Rationale: Designers can tune intensity and cadence without code changes.
5. Re-baseline from observed card rotation before applying the new idle offset.
   - Rationale: Preserves additive behavior when other animations/scripts update card rotation.

## Risks / Trade-offs

- [Risk] Another script may overwrite Z rotation after idle is applied -> Mitigation: apply in `LateUpdate` and re-baseline each frame.
- [Risk] Overly strong rotation jitter can make cards look unstable -> Mitigation: bounded threshold and clamped minimum span.
- [Trade-off] Small per-frame rotation cost on all cards -> Mitigation: lightweight math-only update path.
