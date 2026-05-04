## Context

La escena de juego contiene cartas antiguas y un flujo de inicio que no separa claramente la inicialización de mano y jugadas. Ya existe un prefab de carta con conversión visual por `Suit` y `Rank`, pero falta un punto único de orquestación para: (1) reemplazar cartas antiguas por el nuevo prefab en edición, (2) construir un mazo estándar sin repetición, (3) poblar solo la mano al iniciar Play reutilizando instancias, y (4) resolver cartas jugadas con bloqueo temporal de interacción y redeal automático.

## Goals / Non-Goals

**Goals:**
- Introducir un manager responsable del ciclo de mazo, reparto inicial y resolución de jugadas.
- Garantizar mano inicial aleatoria sin cartas repetidas.
- Garantizar que la zona de cartas jugadas inicie vacía.
- Reutilizar siempre las mismas instancias de cartas (sin instanciación/destrucción en runtime).
- Bloquear interacción 5 segundos cuando existan cartas jugadas y luego reciclarlas a mano con nuevas cartas.
- Mantener compatibilidad con el sistema visual actual del prefab (conversor `Suit`/`Rank`).

**Non-Goals:**
- Implementar reglas completas de poker (turnos, apuestas, evaluación de manos).
- Persistir estado del mazo entre partidas o escenas.
- Añadir red/servidor o sincronización multijugador.

## Decisions

- Crear `PokerHandDeckManager` (nombre final a validar con convención de proyecto) en `Assets/Scripts/` como único orquestador de inicialización.
  - **Rationale:** centraliza responsabilidades y evita lógica duplicada en componentes visuales.
  - **Alternatives considered:**
    - Repartir lógica entre varios scripts de UI: descartado por mayor acoplamiento.
    - Generar cartas en `Awake` de cada carta: descartado porque pierde control global del mazo.

- Representar mazo con una colección en memoria de combinaciones `Suit x Rank` y usar extracción aleatoria sin reposición.
  - **Rationale:** asegura unicidad por diseño y facilita pruebas de no duplicación.
  - **Alternatives considered:**
    - Random por reintentos hasta no repetido: descartado por peor rendimiento y complejidad cuando el mazo se agota.

- En el inicio, limpiar contenedor de jugadas y poblar exclusivamente el contenedor de mano con N cartas configurables.
  - **Rationale:** cumple requisito funcional explícito de arranque sin cartas jugadas.
  - **Alternatives considered:**
    - Mantener cartas jugadas ocultas pero instanciadas: descartado porque conserva estado innecesario y puede reintroducir errores visuales/lógicos.

- Usar un conjunto fijo de instancias de carta existentes en escena y reasignar `Suit`/`Rank` sobre esas mismas instancias cuando cambian de estado (mano/juego).
  - **Rationale:** cumple el requisito de no instanciar en runtime, reduce garbage y mantiene continuidad visual.
  - **Alternatives considered:**
    - Instanciar y destruir por ronda: descartado por restricción explícita del cambio.

- Detectar estado de jugada (cartas presentes en contenedor de juego), bloquear interacción global por 5 segundos y luego reciclar cartas jugadas a mano con nuevas combinaciones del mazo.
  - **Rationale:** garantiza una ventana de resolución sin acciones concurrentes y reinicio limpio de turno usando instancias existentes.
  - **Alternatives considered:**
    - Bloquear solo cartas jugadas: descartado porque permite acciones paralelas no deseadas en otros elementos interactivos.
    - Esperar input manual para finalizar jugada: descartado por requerimiento de espera fija de 5 segundos.

## Risks / Trade-offs

- [Referencias de escena mal configuradas en el manager] -> Mitigación: validaciones en `Awake/Start` con errores claros y fallback seguro sin inicializar parcialmente.
- [Desalineación entre enums de `Suit/Rank` y recursos visuales] -> Mitigación: usar tipos existentes de `uVegas` y pruebas manuales de las 52 combinaciones.
- [Diferencias de layout al reemplazar cartas por prefab] -> Mitigación: respetar parent/layout groups existentes y probar en Play con distintos tamaños de mano.
- [Bloqueo de interacción demasiado agresivo] -> Mitigación: encapsular el lock en un estado de manager con duración serializada (default 5) y un indicador visual opcional.
- [Reasignación incorrecta al reciclar cartas jugadas] -> Mitigación: secuencia explícita mover-a-mano -> extraer-del-mazo -> aplicar-visuales, con validación de cartas disponibles.
- [Cambio funcional visible en escenas no objetivo] -> Mitigación: encapsular activación del manager en escena objetivo y dejar parámetros serializados por escena.
