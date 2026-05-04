## Context

La mano ya se inicializa con cartas aleatorias y reutiliza instancias UI, pero no tiene un accionador explicito para ordenar por valor al pulsar `SUIT`. El cambio debe integrarse con el flujo existente de seleccion/jugada y con el layout en arco, sin crear ni destruir cartas.

## Goals / Non-Goals

**Goals:**
- Ejecutar ordenamiento de la mano al pulsar el boton `SUIT`.
- Ordenar de izquierda a derecha en valor descendente (`Ace/King/.../Two`).
- Mantener el arco centrado y con espaciado uniforme tras ordenar.
- Definir desempate estable para cartas con mismo `Rank`.

**Non-Goals:**
- Cambiar reglas de mazo/reparto o bloqueo de interaccion.
- Introducir animaciones complejas de ordenamiento.
- Reordenar automaticamente en otras acciones no relacionadas con `SUIT`.

## Decisions

- Reutilizar el boton `SUIT` como trigger de ordenamiento de mano.
  - **Rationale:** evita nueva UI y encaja con el flujo actual.
  - **Alternatives considered:** boton nuevo de sort; descartado por ruido de interfaz.

- Ordenar por `Rank` descendente y usar `Suit` como desempate determinista con precedencia fija `Spades > Hearts > Diamonds > Clubs`.
  - **Rationale:** garantiza orden estable y reproducible.
  - **Alternatives considered:** desempate por orden actual de siblings; descartado por resultados inconsistentes.

- Aplicar orden solo sobre cartas activas en `CardsLowerDeck` y luego ejecutar relayout centralizado.
  - **Rationale:** evita afectar cartas inactivas/reserva y conserva arco/centrado/espaciado definidos.
  - **Alternatives considered:** mover transform por posicion absoluta sin relayout; descartado por romper mantenimiento del layout.

## Risks / Trade-offs

- [Orden de `Rank` no alineado con reglas de UI esperadas] -> Mitigacion: mapear `Rank` explicitamente en comparador y probar casos borde (`Ace`, `Two`).
- [Conflicto con estado de seleccion actual] -> Mitigacion: preservar componentes y solo modificar sibling order + relayout.
- [Ordenamiento invocado durante lock] -> Mitigacion: ignorar accion si `IsInteractionLocked` esta activo.
