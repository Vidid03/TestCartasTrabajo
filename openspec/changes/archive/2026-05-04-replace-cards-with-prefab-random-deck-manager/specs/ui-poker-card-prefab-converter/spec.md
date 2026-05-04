## MODIFIED Requirements

### Requirement: Carta Prefab Reutilizable Compatible Con Interacción Actual
El sistema SHALL proveer un prefab de carta UI que mantenga estructura y comportamiento compatibles con las cartas actuales para permitir reemplazo directo en escena y uso operativo durante la inicializacion de partida.

#### Scenario: Prefab usado en grupos existentes
- **WHEN** una instancia del nuevo prefab se coloca en `CardsLowerDeck` o `CardsInPlay`
- **THEN** la carta mantiene compatibilidad con `CardHoverFocus` y su layout base

#### Scenario: Prefab reutilizado por el manager en runtime
- **WHEN** el manager de mazo/mano reasigna cartas al entrar en Play y tras cada resolucion
- **THEN** las instancias existentes del nuevo prefab actualizan visuales por `Suit`/`Rank` y conservan la compatibilidad de interaccion y layout
