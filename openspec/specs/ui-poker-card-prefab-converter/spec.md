# ui-poker-card-prefab-converter Specification

## Purpose
TBD - created by archiving change create-poker-card-prefab-converter. Update Purpose after archive.
## Requirements
### Requirement: Carta Prefab Reutilizable Compatible Con Interacción Actual
El sistema SHALL proveer un prefab de carta UI que mantenga estructura y comportamiento compatibles con las cartas actuales para permitir reemplazo directo en escena y uso operativo durante la inicializacion de partida.

#### Scenario: Prefab usado en grupos existentes
- **WHEN** una instancia del nuevo prefab se coloca en `CardsLowerDeck` o `CardsInPlay`
- **THEN** la carta mantiene compatibilidad con `CardHoverFocus` y su layout base

#### Scenario: Prefab reutilizado por el manager en runtime
- **WHEN** el manager de mazo/mano reasigna cartas al entrar en Play y tras cada resolucion
- **THEN** las instancias existentes del nuevo prefab actualizan visuales por `Suit`/`Rank` y conservan la compatibilidad de interaccion y layout

### Requirement: Conversor Visual Por Tipo De Carta
El sistema SHALL permitir asignar `Suit` y `Rank` en una carta y aplicar automáticamente los sprites de rank/suit/base correspondientes.

#### Scenario: Asignación de palo y valor
- **WHEN** el usuario configura un `Suit` y `Rank` válidos
- **THEN** el conversor aplica los sprites correctos de palo y valor en la carta

### Requirement: Selección De Estilo De Baraja
El sistema SHALL soportar selección de estilo (`Classic`, `Modern`, `Neo`) para usar la variante de sprites adecuada.

#### Scenario: Cambio de estilo
- **WHEN** el estilo de carta cambia de `Classic` a `Modern` o `Neo`
- **THEN** la carta actualiza visuales usando sprites del estilo seleccionado

### Requirement: Toggle De Outline Rojo
El sistema SHALL exponer un booleano para aplicar outline rojo o no aplicar outline.

#### Scenario: Outline habilitado
- **WHEN** `useRedOutline` es `true`
- **THEN** la carta muestra outline rojo

#### Scenario: Outline deshabilitado
- **WHEN** `useRedOutline` es `false`
- **THEN** la carta no muestra outline

