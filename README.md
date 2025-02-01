# Crazy Maze
Mi primer proyecto de programación.

## Descripción:
Es un juego de laberinto para dos jugadores donde cada uno debe moverse estratégicamente a través de un mapa generado de forma aleatoria, esquivando trampas y utilizando una habilidad especial para alcanzar la victoria. El primer jugador en llegar a la casilla inicial de su oponente será el vencedor.

## Requisitos:
- .NET SDK 8.0.401 para compilar y ejecutar el código en C#.
- Consola de comandos para ejecutar el programa.

## Para jugar:
1. Ejecute el programa en la consola.
2. Se muestra el laberinto con los jugadores y las trampas.
3. Para comenzar la partida cada jugador debe seleccionar una habilidad.
4. Los jugadores se moverán por turnos presionando las siguientes teclas:
 Jugador 1: W (arriba), A (izquierda), S (abajo), D (derecha)
 Jugador 2: ↑ (arriba), ← (izquierda), ↓ (abajo), → (derecha)
5. Se deben evitar las trampas como bombas y estrellas mágicas, y aprovechar las manzanas saludables.
6. El ganador será el primero en llegar a la casilla de inicio de su oponente.

## Elementos del mapa:
- ⬛Pared (no se puede atravesar).
- 🟫Camino (los jugadores se desplazan a través de él).
- 💣Bomba (reduce vida al pisarla).
- 🍎Manzana (incrementa la vida).
- ⭐Estrella mágica (te teletransporta a una casilla random del laberinto).
- 😃 Jugador1.
- 😠 Jugadoe2.

## Habilidades de los jugadores:
Para que inicie la partida cada jugador debe seleccionar una habilidad de las siguientes:
1. Escudo protector: evita el daño al caer sobre una trampa.
2. Doble movimiento: permite caminar dos veces seguidas.
3. Atravesar pared: podrás caminar por las paredes.
4. Curación: incrementará tu vida.
5. Teletransportación: te moverá a un punto aleatorio del mapa.
Para activar tu habilidad deberás presionar la tecla B.

## Reglas:
- Los jugadores se mueven por turnos y si se mueven en dirección a una pared perderán el turno.
- Las habilidades poseen un tiempo de enfriamiento para usarse.
- Si un jugador pierde toda su vida, su oponente ganará automáticamente.











