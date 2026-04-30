## 1. Hover Focus Component

- [x] 1.1 Create a reusable UI card hover component handling pointer enter/exit events.
- [x] 1.2 Store each card's base state (position, scale, sibling index) for exact restoration.

## 2. Smooth Animation Behavior

- [x] 2.1 Implement smooth hover-in interpolation (~0.15s) for scale-up and directional lift toward `Reference`.
- [x] 2.2 Implement smooth hover-out interpolation (~0.15s) back to cached base state.
- [x] 2.3 Ensure hover interruptions (rapid enter/exit) continue smoothly from current animated state.

## 3. Visual Stacking Priority

- [x] 3.1 Raise hovered card to top rendering priority in its group while hovered.
- [x] 3.2 Restore original sibling order for that card when hover ends.

## 4. Scene Integration And Validation

- [x] 4.1 Attach/configure the hover component on cards in `CardsInPlay` and `CardsLowerDeck`.
- [x] 4.2 Validate that base curved layout remains unchanged when not hovered.
- [x] 4.3 Validate behavior: top layering, subtle scale, directional lift, smooth 0.15s in/out restore.
