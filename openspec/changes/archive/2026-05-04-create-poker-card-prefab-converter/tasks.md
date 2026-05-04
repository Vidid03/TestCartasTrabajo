## 1. Conversor De Visuales De Carta

- [x] 1.1 Crear `PokerCardVisualConverter` con campos serializados para `Suit`, `Rank`, estilo, referencias `Image` y control de outline.
- [x] 1.2 Implementar resolución de nombre/ruta de sprite para rank y suit según estilo (`Classic`, `Modern`, `Neo`).
- [x] 1.3 Aplicar actualización visual automática en `OnValidate` y método público de refresh en runtime.

## 2. Prefab De Carta Compatible

- [x] 2.1 Crear un prefab nuevo equivalente a carta actual (estructura UI + `CardHoverFocus` + `Outline`).
- [x] 2.2 Integrar `PokerCardVisualConverter` en el prefab y vincular sus referencias (`baseImage`, `rankImage`, `suitImage`, `outline`).
- [x] 2.3 Configurar tamaño base y parámetros para reemplazo directo en los grupos actuales.

## 3. Validación De Uso

- [x] 3.1 Probar en `CardsLowerDeck` y `CardsInPlay` que el conversor aplica sprites correctos por palo/valor/estilo.
- [x] 3.2 Verificar que el booleano de outline activa rojo o lo deshabilita sin romper hover/selección.
