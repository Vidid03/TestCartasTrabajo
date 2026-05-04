## ADDED Requirements

### Requirement: Relayout estable tras orden manual
El sistema SHALL reaplicar el layout de mano en arco despues de ordenar por accion de UI, manteniendo centrado y espaciado uniforme.

#### Scenario: Arco consistente tras ordenar
- **WHEN** la mano se ordena desde el boton `SUIT`
- **THEN** las cartas permanecen en arco centrado y sin deriva lateral

### Requirement: Ordenamiento no permitido durante lock
El sistema SHALL ignorar la accion de ordenamiento de mano mientras el lock de interaccion este activo.

#### Scenario: Pulsacion durante lock
- **WHEN** el usuario pulsa `SUIT` durante el periodo de bloqueo
- **THEN** la mano no cambia de orden hasta que termine el lock
