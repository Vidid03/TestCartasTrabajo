## ADDED Requirements

### Requirement: Ordenar mano al pulsar SUIT
El sistema SHALL ordenar las cartas activas de la mano cuando el usuario pulse el boton `SUIT`.

#### Scenario: Trigger de ordenamiento
- **WHEN** el usuario pulsa el boton `SUIT`
- **THEN** el sistema ejecuta la rutina de ordenamiento sobre `CardsLowerDeck`

### Requirement: Orden descendente de izquierda a derecha
El sistema SHALL dejar la mano en orden descendente por `Rank`, con la carta de mayor valor en la izquierda y la de menor valor en la derecha.

#### Scenario: Resultado visual del ordenamiento
- **WHEN** termina la rutina de ordenamiento
- **THEN** el orden visual de las cartas en la mano va de mayor a menor de izquierda a derecha

### Requirement: Desempate determinista
El sistema SHALL aplicar una regla de desempate determinista cuando dos cartas tengan el mismo `Rank`.

#### Scenario: Cartas con mismo rank
- **WHEN** existen dos o mas cartas con el mismo `Rank`
- **THEN** el sistema aplica el mismo criterio de desempate en cada ejecucion

### Requirement: Orden fijo de palos para desempate
El sistema SHALL usar siempre el mismo orden de palos para desempatar cartas con igual `Rank`: `Spades > Hearts > Diamonds > Clubs`.

#### Scenario: Desempate por palo
- **WHEN** dos cartas tienen el mismo `Rank`
- **THEN** la carta con palo de mayor precedencia segun `Spades > Hearts > Diamonds > Clubs` queda mas a la izquierda
