public class Laberinto
{
    private int[,] mapa;
    private int filas;
    private int columnas;
    private int PINCHOS = 2;
    private int SALUD = 3;
    private int TELETRANSPORTE = 4;

    public Laberinto(int filas, int columnas)
    {
        this.filas = filas;
        this.columnas = columnas;
        mapa = new int[filas, columnas];
        InicializarMapa();
        GenerarLaberinto(1, 1);
        ColocarTrampas(PINCHOS, 5); // trampas de pinchos
        ColocarTrampas(SALUD, 3); // trampas de salud
        ColocarTrampas(TELETRANSPORTE, 2); // trampas de teletransporte
    }

    private void InicializarMapa()
    {
        for (int i = 0; i < filas; i++)
        {
            for (int j = 0; j < columnas; j++)
            {
                mapa[i, j] = 1; // Pared
            }
        }
    }

    private void GenerarLaberinto(int x, int y)
    {
        int[] dx = { -2, 2, 0, 0 };
        int[] dy = { 0, 0, -2, 2 };
        Random rnd = new Random();
        List<int> indices = new List<int> { 0, 1, 2, 3 };
        for (int i = indices.Count - 1; i > 0; i--)
        {
            int j = rnd.Next(i + 1);
            int temp = indices[i];
            indices[i] = indices[j];
            indices[j] = temp;
        }
        foreach (int i in indices)
        {
            int nx = x + dx[i];
            int ny = y + dy[i];
            if (nx > 0 && nx < filas && ny > 0 && ny < columnas && mapa[nx, ny] == 1)
            {
                mapa[nx, ny] = 0; // Hacer el espacio libre
                mapa[x + dx[i] / 2, y + dy[i] / 2] = 0; // Hacer el camino entre celdas
                GenerarLaberinto(nx, ny);
            }
        }
    }

    private void ColocarTrampas(int tipoTrampa, int cantidad)
    {
        Random rnd = new Random();
        int colocadas = 0;
        while (colocadas < cantidad)
        {
            int x = rnd.Next(filas);
            int y = rnd.Next(columnas);
            if (mapa[x, y] == 0) // Solo colocar trampas en espacios vacíos
            {
                mapa[x, y] = tipoTrampa;
                colocadas++;
            }
        }
    }

    public void MostrarMapa(Jugador jugador1, Jugador jugador2)
    {
        Console.Clear();
        for (int i = 0; i < filas; i++)
        {
            for (int j = 0; j < columnas; j++)
            {
                if (jugador1.Posicion[0] == i && jugador1.Posicion[1] == j)
                    
                    Console.Write("😃"); // Jugador 1
                else if (jugador2.Posicion[0] == i && jugador2.Posicion[1] == j)
                   
                    Console.Write("😠"); // Jugador 2
                else
                {
                    switch (mapa[i, j])
                    {
                        case 1:
                            
                            Console.Write("⬛"); // Pared
                            break;
                        case 0:
                           
                            Console.Write("🟫"); // Camino
                            break;
                        case 2:
                            
                            Console.Write("💣"); // Pinchos
                            break;
                        case 3:
                            
                            Console.Write("🍎"); // Salud
                            break;
                        case 4:
                            
                            Console.Write("⭐"); // Teletransporte
                            break;
                    }
                }
            }
            Console.WriteLine();
        }
        MostrarEstadoJugadores(jugador1, jugador2);
    }

    private void MostrarEstadoJugadores(Jugador jugador1, Jugador jugador2)
    {
        Console.WriteLine($"Vida Jugador 1: {jugador1.Vida} | Habilidad: {jugador1.Habilidad} (Enfriamiento: {jugador1.Enfriamiento})");
        Console.WriteLine($"Vida Jugador 2: {jugador2.Vida} | Habilidad: {jugador2.Habilidad} (Enfriamiento: {jugador2.Enfriamiento})");
    }

    public int[,] ObtenerMapa()
    {
        return mapa;
    }

    public void ReemplazarTrampa(int fila, int columna, int tipoTrampa)
    {
        mapa[fila, columna] = 0; // Remover la trampa
        ColocarTrampaEnPosicionAleatoria(tipoTrampa); // Colocar una nueva trampa en una posición aleatoria
    }

    private void ColocarTrampaEnPosicionAleatoria(int tipoTrampa)
    {
        Random rnd = new Random();
        int x, y;
        do
        {
            x = rnd.Next(filas);
            y = rnd.Next(columnas);
        } while (mapa[x, y] != 0);
        mapa[x, y] = tipoTrampa; // Colocar la nueva trampa
    }

    public int[] Teletransportar()
    {
        Random rnd = new Random();
        int x, y;
        do
        {
            x = rnd.Next(filas);
            y = rnd.Next(columnas);
        } while (mapa[x, y] != 0);
        return new int[] { x, y }; // Retornar la nueva posición
    }
}