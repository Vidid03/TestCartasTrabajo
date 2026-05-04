## ADDED Requirements

### Requirement: Cards Have Continuous Idle Rotation
The system SHALL continuously apply a subtle looping Z rotation to each card while it is active.

#### Scenario: Idle rotation always running
- **WHEN** a card is active in the UI
- **THEN** it continuously rotates in a slow loop without requiring pointer interaction

### Requirement: Idle Rotation Uses Bounded Base Range With Random Threshold
The system SHALL animate each card around a nominal range of `-5` to `+5` degrees and include a per-card random threshold offset up to `+-2` degrees.

#### Scenario: Per-card randomized idle bounds
- **WHEN** two or more cards are visible
- **THEN** each card uses its own slightly different idle bounds derived from nominal `[-5, +5]` plus random threshold variation within `+-2`

### Requirement: Idle Rotation Coexists With Other Card Animations
The system SHALL keep idle rotation active while other card animations are applied.

#### Scenario: Hover and selection while idle runs
- **WHEN** hover, selection, or transfer animations run on a card
- **THEN** idle rotation still applies concurrently instead of being disabled
