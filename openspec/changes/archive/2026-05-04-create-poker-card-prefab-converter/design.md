## Context

La escena actual usa cartas UI con `Image + Outline + CardHoverFocus` y transformaciones concretas (tamaño aproximado `88x132`). El paquete `uVegas` ya incluye imágenes de rangos y palos por estilo (`Classic`, `Modern`, `Neo`) y sprites de front/back/joker. Se necesita un flujo de asignación automático por carta para evitar configuración manual al escalar a muchas cartas.

## Goals / Non-Goals

**Goals:**
- Crear un prefab de carta equivalente al actual para reemplazo directo.
- Implementar un conversor por componente que resuelva sprites a partir de `Suit`, `Rank` y estilo.
- Soportar toggle booleano para outline rojo o ninguno.
- Mantener compatibilidad con `CardHoverFocus` y layout actual.

**Non-Goals:**
- Rehacer sistema de juego o lógica de reparto.
- Crear un editor custom complejo.
- Reestructurar los assets del paquete `uVegas`.

## Decisions

1. Crear un componente dedicado (`PokerCardVisualConverter`) en `Assets/Scripts/`.
   - Rationale: encapsula la conversión visual sin mezclar con interacción (`CardHoverFocus`).
2. Resolver sprites por convención de nombres (`A,K,Q,J,10..2` y `Hearts/Diamonds/Clubs/Spades`) sobre carpetas por estilo.
   - Rationale: los assets ya siguen esa convención y evita tablas manuales extensas.
3. Aplicar imágenes sobre 3 `Image`: base, rank y suit (estructura igual a `uVegas` card prefab).
   - Rationale: permite un look idéntico al de cartas actuales y reutilizable para nuevos mazos.
4. Manejar outline con un bool `useRedOutline`; si es `false`, deshabilitar componente `Outline`.
   - Rationale: costo mínimo y comportamiento explícito de "rojo o ninguno".
5. Mantener `CardHoverFocus` en el prefab para no romper UX existente.
   - Rationale: evita divergencia entre cartas antiguas y nuevas.

## Risks / Trade-offs

- [Risk] Convenciones de nombre incompletas en alguna variante de estilo -> Mitigation: validación y fallback controlado (warning + no cambio de sprite faltante).
- [Risk] Referencias rotas al mover assets -> Mitigation: campos serializados de ruta base y método de refresh manual en editor.
- [Trade-off] Lookup por Resources no se usa para evitar cambios de pipeline; se prioriza referencia explícita/caché local.
