## 1. Preparar escena y referencias

- [x] 1.1 Identificar en la escena objetivo los contenedores de mano y cartas jugadas (`CardsLowerDeck` y `CardsInPlay` o equivalentes)
- [x] 1.2 Reemplazar cartas antiguas de la escena por el nuevo prefab y dejar un conjunto fijo de instancias reutilizables en jerarquia
- [x] 1.3 Exponer en Inspector del manager referencias a cartas reutilizables, contenedor de mano, contenedor de jugadas y cantidad inicial de cartas en mano

## 2. Implementar manager de mazo y reparto

- [x] 2.1 Crear clase `PokerHandDeckManager` en `Assets/Scripts/` como orquestador unico de inicializacion
- [x] 2.2 Implementar construccion de mazo estandar (52 cartas: 4 `Suit` x todos los `Rank`)
- [x] 2.3 Implementar extraccion aleatoria sin reposicion para que cada carta repartida se elimine del mazo
- [x] 2.4 Implementar inicializacion de Play que limpie jugadas y reparta solo a mano
- [x] 2.5 Reasignar `Suit`/`Rank` sobre las mismas instancias de carta sin instanciacion/destruccion en runtime

## 3. Resolver jugadas con bloqueo temporal

- [x] 3.1 Detectar cuando haya cartas en juego y activar estado de bloqueo global de interaccion
- [x] 3.2 Implementar espera de 5 segundos durante el bloqueo sin permitir interacciones
- [x] 3.3 Al finalizar el bloqueo, limpiar zona de jugadas y devolver esas instancias a mano con nuevas cartas del mazo

## 4. Validar comportamiento funcional

- [x] 4.1 Verificar en Play que no exista ninguna carta en jugadas al inicio
- [x] 4.2 Verificar que la mano inicial tenga la cantidad configurada y todas las cartas sean unicas
- [x] 4.3 Verificar que durante los 5 segundos no se pueda interactuar con nada
- [x] 4.4 Verificar que las cartas jugadas desaparezcan y vuelvan a mano como nuevas sin crear instancias nuevas
