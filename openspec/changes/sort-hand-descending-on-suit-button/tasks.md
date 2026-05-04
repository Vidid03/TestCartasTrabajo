## 1. Integrar accion de ordenamiento en UI

- [x] 1.1 Localizar el handler actual del boton `SUIT` y conectar la accion de ordenamiento de mano
- [x] 1.2 Bloquear la accion cuando `IsInteractionLocked` este activo

## 2. Implementar ordenamiento descendente de mano

- [x] 2.1 Leer cartas activas de `CardsLowerDeck` usando su `Rank` actual
- [x] 2.2 Implementar comparador por `Rank` descendente con desempate por `Suit` usando orden fijo `Spades > Hearts > Diamonds > Clubs`
- [x] 2.3 Reordenar siblings de la mano segun el resultado y reaplicar relayout del arco

## 3. Validar comportamiento funcional

- [ ] 3.1 Verificar que al pulsar `SUIT` la mano queda de mayor a menor de izquierda a derecha
- [ ] 3.2 Verificar consistencia del desempate con cartas de mismo `Rank` respetando `Spades > Hearts > Diamonds > Clubs`
- [ ] 3.3 Verificar que el arco de mano permanece centrado y con espaciado uniforme tras ordenar
- [ ] 3.4 Verificar que durante lock de interaccion la accion de `SUIT` no altera el orden
