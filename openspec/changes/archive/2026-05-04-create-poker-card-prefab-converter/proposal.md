## Why

Ahora que ya tenemos los assets de una baraja completa en `Assets/uVegas/Images`, hace falta un prefab de carta reutilizable igual al visual actual y un conversor para asignar automáticamente tipo/valor/color sin montaje manual por carta.

## What Changes

- Crear un prefab de carta UI equivalente a las cartas actuales (misma estructura visual y compatibilidad con interacción existente).
- Añadir un conversor de carta para asignar `suit` y `rank` desde inspector y aplicar sprites automáticamente.
- Permitir selección de estilo de baraja (`Classic`, `Modern`, `Neo`) usando las imágenes de `Assets/uVegas/Images/Cards/<Style>/`.
- Añadir una opción booleana para habilitar/deshabilitar outline rojo en la carta (si está desactivado, no aplicar outline).
- Mantener compatibilidad con `CardHoverFocus` para hover/selección/idle ya existentes.

## Capabilities

### New Capabilities
- `ui-poker-card-prefab-converter`: Provee un prefab de carta y un componente conversor que aplica automáticamente imágenes según palo/valor/estilo, con toggle de outline rojo.

### Modified Capabilities
- None.

## Impact

- Scripts Unity UI en `Assets/Scripts/` para el conversor.
- Nuevo prefab de carta en `Assets/Prefabs/` reutilizable por `CardsLowerDeck` y `CardsInPlay`.
- Uso directo de sprites existentes en `Assets/uVegas/Images/Cards/`.
- Sin cambios de backend ni dependencias externas.
