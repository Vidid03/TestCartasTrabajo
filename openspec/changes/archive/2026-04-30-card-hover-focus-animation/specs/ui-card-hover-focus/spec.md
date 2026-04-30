## ADDED Requirements

### Requirement: Hovered Card Is Visually Prioritized
The system SHALL place the hovered card above overlapping sibling cards while the pointer is over it.

#### Scenario: Pointer enters a stacked card
- **WHEN** the pointer enters a card with hover focus behavior
- **THEN** that card renders above the other cards in its visual group

### Requirement: Hover Applies Smooth Scale And Directional Lift
The system SHALL smoothly animate the hovered card over approximately 0.15 seconds to a slightly larger scale and a slight offset toward the configured reference point.

#### Scenario: Hover transition in
- **WHEN** the pointer remains over a card
- **THEN** the card scales up slightly and moves slightly toward the reference direction using smooth interpolation over ~0.15 seconds

### Requirement: Card Fully Restores On Hover Exit
The system SHALL smoothly restore the card to its exact pre-hover state when the pointer exits.

#### Scenario: Hover transition out
- **WHEN** the pointer exits the card
- **THEN** the card returns to its original position, scale, and render order over ~0.15 seconds

### Requirement: Behavior Works With Existing Curved Layout
The system SHALL operate without breaking the existing curved/overlapped card arrangement.

#### Scenario: Existing layout remains intact
- **WHEN** hover behavior is enabled on cards in `CardsInPlay` and `CardsLowerDeck`
- **THEN** cards keep their configured base positions/rotations outside active hover states
