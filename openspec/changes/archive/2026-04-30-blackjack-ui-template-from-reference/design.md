## Context

The project needs a Unity UI-only scaffold that mirrors selected elements from `References/Template.png` for a blackjack-style card screen. The request is intentionally placeholder-focused: default Unity visuals only, no custom sprites, no gameplay logic, and no art production. The generated layout must include specific controls and text labels plus two card groups arranged in a slight arc/fan shape.

## Goals / Non-Goals

**Goals:**
- Build a reusable UI hierarchy in Unity UI (`Canvas` + `RectTransform`-based) that places only the requested elements.
- Keep all visuals at Unity defaults (white/basic buttons, default image/text look), while matching relative placement from the reference.
- Represent card zones with placeholders: 5 in-play cards and ~10 lower cards, each arranged with slight circular curvature.
- Structure objects so later gameplay wiring can bind interactions and card data without rebuilding layout.

**Non-Goals:**
- Implement blackjack game rules, state management, or networking.
- Add final iconography, polished art, custom fonts, VFX, or non-default styling.
- Recreate every element present in the reference image.

## Decisions

1. Use Unity UI hierarchy with anchor-driven layout.
   - Rationale: Anchor-based `RectTransform` positioning preserves corner/center alignment across resolutions and is the fastest path for template scaffolding.
   - Alternative considered: Absolute pixel placement only; rejected because it scales poorly on aspect changes.

2. Use default UI components only (`Button`, `Image`, `Text` or `TMP_Text` default settings available in project).
   - Rationale: Matches requirement to avoid art and keep placeholders basic.
   - Alternative considered: Custom sprite packs for icons/cards; rejected as explicitly out of scope.

3. Create two dedicated container objects for card groups with scripted/manual transforms in arc patterns.
   - Rationale: A parent-centered fan/arc is easier to tune than independent absolute placements and keeps card layout maintainable.
   - Alternative considered: Horizontal layout groups; rejected because they produce linear rows and do not capture the slight circular shape.

4. Keep semantic object naming (`BtnBack`, `BtnSettings`, `BtnTrash`, `BtnUpload`, `BtnSuit`, `TxtBankrollA`, `TxtBankrollB`, `TxtHandName`, `CardsInPlay`, `CardsLowerDeck`).
   - Rationale: Enables straightforward binding from future scripts and inspector references.
   - Alternative considered: Auto-generated names; rejected due to poor maintainability.

## Risks / Trade-offs

- [Risk] Reference-driven spacing may look different on unusual aspect ratios. -> Mitigation: Use normalized anchors and pivot-consistent offsets; verify in at least 16:9 and tall mobile preview.
- [Risk] Curved card feel may depend on exact per-card rotation/offset values. -> Mitigation: Store cards under parent containers and tune with small incremental angle/position steps.
- [Trade-off] Default visuals are intentionally plain and may feel incomplete. -> Mitigation: Treat this as a layout baseline and schedule a later art pass.
