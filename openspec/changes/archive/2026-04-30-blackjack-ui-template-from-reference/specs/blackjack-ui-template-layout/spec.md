## ADDED Requirements

### Requirement: Template Screen Uses Default Unity UI Placeholders
The system SHALL create a Unity UI screen template using only default/basic UI components and default visual styling, without adding custom art assets.

#### Scenario: Create placeholder-only UI
- **WHEN** the template is generated
- **THEN** all requested elements use default Unity UI visuals (basic white buttons/images and default text style)

### Requirement: Top Corner Navigation Buttons
The system SHALL place exactly two top-corner buttons: a back button in the upper-left and a settings button in the upper-right.

#### Scenario: Top buttons are present and positioned
- **WHEN** the user opens the template scene
- **THEN** a back button is visible in the top-left corner and a settings button is visible in the top-right corner

### Requirement: Bottom Action Buttons
The system SHALL place exactly three lower buttons corresponding to trash, upload, and suit actions.

#### Scenario: Bottom action controls exist
- **WHEN** the user views the lower UI area
- **THEN** three distinct buttons for trash, upload, and suit are visible in the bottom region

### Requirement: Center Value and Hand Text
The system SHALL display center text placeholders containing `999,999`, adjacent `999,999`, and `three of a kind`.

#### Scenario: Center text placeholders are rendered
- **WHEN** the template is displayed
- **THEN** the two numeric placeholders and the hand-name placeholder text are visible near the center as specified

### Requirement: In-Play Card Group Uses Slight Arc
The system SHALL create a group of 5 in-play card placeholders arranged in a slight circular/fan pattern rather than a straight row.

#### Scenario: Five in-play cards appear in curved formation
- **WHEN** the user inspects the main card area
- **THEN** exactly 5 card placeholders are visible with slight arc-like offsets and/or rotations

### Requirement: Lower Card Group Uses Slight Arc
The system SHALL create a lower group of approximately 10 card placeholders arranged in a slight circular/fan pattern similar to the reference composition.

#### Scenario: Lower card placeholders appear in curved formation
- **WHEN** the user inspects the lower card area
- **THEN** about 10 card placeholders are visible in a slight arc/fan arrangement
