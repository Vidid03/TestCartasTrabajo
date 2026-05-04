## 1. Idle Rotation Configuration

- [x] 1.1 Add serialized idle configuration fields for nominal min/max Z, random threshold, and loop duration.
- [x] 1.2 Add per-card runtime randomization state (range offsets, phase, runtime duration).

## 2. Always-On Loop Behavior

- [x] 2.1 Implement continuous idle loop Z rotation update per card.
- [x] 2.2 Ensure each card oscillates around nominal `[-5, +5]` with per-card `+-2` threshold variation.
- [x] 2.3 Ensure idle loop remains active while hover/selection/transfer animations occur.

## 3. Integration With Existing Card Flow

- [x] 3.1 Integrate idle rotation with existing base snapshot/refresh flow so layout rotations are preserved.
- [x] 3.2 Validate behavior in both `CardsLowerDeck` and `CardsInPlay` groups.
