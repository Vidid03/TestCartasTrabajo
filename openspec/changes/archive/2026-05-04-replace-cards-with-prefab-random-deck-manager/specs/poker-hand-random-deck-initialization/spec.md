## ADDED Requirements

### Requirement: Inicializacion de mazo estandar sin duplicados
El sistema SHALL construir un mazo de poker de 52 cartas compuesto por todos los `Rank` disponibles para cada uno de los 4 `Suit`, sin cartas repetidas.

#### Scenario: Mazo completo al iniciar
- **WHEN** el manager inicializa el mazo al entrar en Play
- **THEN** el mazo contiene exactamente 52 combinaciones unicas de `Suit` y `Rank`

### Requirement: Reparto aleatorio solo a la mano
El sistema SHALL repartir de forma aleatoria un numero configurable de cartas unicamente en la zona de mano al inicio de la partida.

#### Scenario: Mano inicial poblada
- **WHEN** se ejecuta la inicializacion de partida
- **THEN** solo el contenedor de mano recibe cartas asignadas sobre instancias reutilizadas

### Requirement: Zona de jugadas vacia al inicio
El sistema SHALL garantizar que la zona de cartas jugadas inicie vacia y sin cartas asignadas durante la inicializacion.

#### Scenario: Inicio sin cartas jugadas
- **WHEN** la escena entra en Play y corre la inicializacion
- **THEN** el contenedor de cartas jugadas queda vacio

### Requirement: Extraccion sin reposicion
El sistema SHALL retirar del mazo cada carta asignada a la mano para impedir que vuelva a aparecer mientras el mazo no se reconstruya.

#### Scenario: No repeticion tras repartir
- **WHEN** se reparten cartas consecutivamente desde el mazo inicial
- **THEN** ninguna combinacion de `Suit` y `Rank` se repite en el reparto

### Requirement: Reutilizacion de instancias de carta
El sistema SHALL reutilizar siempre las mismas instancias de carta para los estados de mano y jugadas, sin instanciar ni destruir cartas durante la partida.

#### Scenario: Cartas persistentes en runtime
- **WHEN** inicia la partida y se realizan ciclos de jugar y renovar mano
- **THEN** los mismos objetos de carta cambian de contenedor y visuales sin creacion de nuevas instancias

### Requirement: Bloqueo de interaccion durante resolucion
El sistema SHALL desactivar toda interaccion durante 5 segundos cuando existan cartas en la zona de jugadas.

#### Scenario: Lock al jugar cartas
- **WHEN** una o mas cartas pasan al contenedor de jugadas
- **THEN** ninguna interaccion de usuario queda habilitada durante 5 segundos

### Requirement: Limpieza de jugadas y redeal reutilizando cartas
El sistema SHALL, tras el bloqueo de 5 segundos, vaciar la zona de jugadas y devolver esas mismas instancias a la mano reasignandoles nuevas cartas del mazo sin reposicion.

#### Scenario: Fin de resolucion y nueva mano parcial
- **WHEN** termina el periodo de bloqueo tras una jugada
- **THEN** las cartas jugadas desaparecen de la zona de jugadas y reaparecen en mano con nuevas combinaciones no repetidas
