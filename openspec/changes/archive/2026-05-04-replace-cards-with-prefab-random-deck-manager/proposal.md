## Why

La escena aún depende de cartas antiguas y de una inicialización que puede dejar cartas jugadas al iniciar, lo que rompe el flujo esperado de partida. Necesitamos una inicialización única y determinista que use el nuevo prefab con reutilización de instancias, prepare solo la mano inicial desde un mazo real sin duplicados y gestione el ciclo de cartas jugadas.

## What Changes

- Reemplazar en escena las cartas actuales por el nuevo prefab de carta compatible con el sistema visual existente.
- Introducir una clase manager que construya y gestione un mazo estándar de 52 cartas (4 palos x todos los rangos) sin repetición.
- Al entrar en Play, poblar únicamente las cartas de la mano con cartas aleatorias extraídas del mazo, reutilizando siempre las mismas instancias de carta.
- Garantizar que el contenedor/zona de cartas jugadas inicie vacío y no reciba cartas en la inicialización.
- Mantener el estado del mazo para que una carta ya asignada a la mano no pueda volver a salir mientras no se reconstruya el mazo.
- Cuando haya cartas en juego, bloquear toda interacción durante 5 segundos y, al finalizar, limpiar jugadas y devolver esas mismas instancias a la mano con nuevas cartas del mazo.

## Capabilities

### New Capabilities
- `poker-hand-random-deck-initialization`: Inicializa mano desde mazo aleatorio sin duplicados, sin instanciación en runtime, y fuerza que la zona de jugadas arranque vacía.

### Modified Capabilities
- `ui-poker-card-prefab-converter`: Se amplía el uso del prefab para cubrir reemplazo operativo en escena y actualización dinámica de visuales al reciclar cartas.

## Impact

- Código afectado en `Assets/Scripts/` para crear el manager de mazo/mano/ciclo de jugadas y enlazarlo con los contenedores UI de cartas.
- Escena principal afectada para reemplazar referencias de cartas antiguas por el nuevo prefab y configurar un conjunto fijo de instancias reutilizables.
- Dependencia funcional con `PokerCardVisualConverter`/tipos de carta para mapear suit/rank a visuales.
- Dependencia funcional con scripts de interacción para bloqueo temporal de input durante la resolución de jugadas.
- Flujo de partida más consistente: mano inicial lista, zona de jugadas vacía, resolución automática de jugada y redeal sin crear ni destruir cartas.
