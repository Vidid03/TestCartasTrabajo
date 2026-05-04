## Why

Actualmente al pulsar el boton `SUIT` no existe un ordenamiento consistente de la mano por valor de carta. Ordenar visualmente de mayor a menor mejora lectura, decision de jugada y consistencia del flujo de UI.

## What Changes

- Añadir comportamiento al boton `SUIT` para ordenar las cartas de la mano en orden descendente por valor.
- Garantizar que el orden visual final quede de izquierda a derecha con valor mas alto a la izquierda y mas bajo a la derecha.
- Reaplicar layout de mano despues del ordenamiento para mantener el arco, centrado y espaciado configurado.
- Definir criterio determinista de desempate cuando dos cartas tengan el mismo `Rank`, usando orden fijo de palos: `Spades > Hearts > Diamonds > Clubs`.

## Capabilities

### New Capabilities
- `hand-sort-descending-on-suit-action`: Ordena la mano por `Rank` descendente al pulsar el boton `SUIT` y actualiza el layout visual de forma estable.

### Modified Capabilities
- `poker-hand-random-deck-initialization`: Se extiende para incluir reordenamiento manual de la mano por accion de UI sin romper reutilizacion de instancias ni layout.

## Impact

- Codigo afectado en `Assets/Scripts/` en el flujo de boton `SUIT`, manager de mano/mazo y rutina de relayout.
- Sin cambios en APIs externas; impacto limitado a logica de UI y orden de siblings en la mano.
- Se reutilizan las mismas instancias de carta; no hay instanciacion adicional.
