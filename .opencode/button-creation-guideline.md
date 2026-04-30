# Pauta global para crear botones

Cuando el usuario pida crear un boton, seguir siempre este flujo:

1. Usar Unity UI.
2. Revisar la escena para ver cuantos canvases hay.
3. Si hay mas de un canvas, preguntar cual quiere usar.
4. Si no hay canvas, preguntar si quiere crear uno nuevo y, si la respuesta es si, pedir el nombre del canvas.
5. Preguntar siempre por:
   - tamaño del boton
   - posicion del boton
   - color por defecto
   - color al pasar el cursor
   - texto del boton
6. En cada pregunta, dar tambien una serie corta de opciones sugeridas para responder rapido, manteniendo la posibilidad de escribir un valor propio.
7. Si solo hay un canvas, usarlo sin preguntar a menos que el usuario pida otro.
8. No inventar valores si el usuario no los da.
9. Mantener el flujo corto y directo, haciendo solo las preguntas necesarias antes de crear el boton.
