## ADDED Requirements

### Requirement: Click Toggles Card Selection
The system SHALL toggle selection state when the user clicks a card that is currently hovered.

#### Scenario: Select hovered card
- **WHEN** the pointer is over a card and the user clicks it
- **THEN** the card enters selected state

#### Scenario: Deselect selected card
- **WHEN** the user clicks a selected card
- **THEN** the card exits selected state

### Requirement: Selected Cards Use Red Outline
The system SHALL use red outline for selected cards and black outline for unselected cards.

#### Scenario: Selection visual on
- **WHEN** a card becomes selected
- **THEN** its outline color becomes red

#### Scenario: Selection visual off
- **WHEN** a card becomes deselected
- **THEN** its outline color becomes black

### Requirement: Selection Locks Hover State
The system SHALL keep selected cards in hover-style visual state and bypass normal hover enter/exit transitions while selected.

#### Scenario: Pointer exits selected card
- **WHEN** pointer exits a selected card
- **THEN** the card remains in hover-locked visual state

#### Scenario: Pointer re-enters selected card
- **WHEN** pointer re-enters a selected card
- **THEN** no additional hover transition is applied

#### Scenario: Other card exits hover
- **WHEN** another non-selected card completes hover-exit/reset behavior
- **THEN** selected cards are not reset and remain in their selected hover-locked visual state

### Requirement: Deselect Restores Normal Hover Flow
The system SHALL restore normal hover transition behavior after deselection.

#### Scenario: Deselect selected card then pointer outside
- **WHEN** a selected card is deselected while pointer is not over it
- **THEN** hover-exit behavior is applied and the card returns to default state

### Requirement: Multi-Select Is Restricted To One Group
The system SHALL allow multi-selection only within a single card group at a time.

#### Scenario: Group lock in play cards
- **WHEN** one or more cards in `CardsInPlay` are selected
- **THEN** selecting cards in `CardsLowerDeck` is blocked

#### Scenario: Group lock in lower cards
- **WHEN** one or more cards in `CardsLowerDeck` are selected
- **THEN** selecting cards in `CardsInPlay` is blocked

#### Scenario: Group lock cleared
- **WHEN** all selected cards are deselected
- **THEN** cards from either group can be selected again
