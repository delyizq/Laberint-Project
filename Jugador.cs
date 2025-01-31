public class Jugador
{
    public string Nombre { get; set; }
    public int Vida { get; set; }
    public List<string> Habilidades { get; private set; }
    public int[] Posicion { get; set; } // [fila, columna]
    public int[] PosicionVictoria;
    public string Habilidad { get; set; }
    public int Enfriamiento { get; set; }
    public int TurnosDobleMovimiento { get; set; }
    public bool EscudoActivo { get; private set; } // Indica si el escudo está activo
    public bool AtravesarPared = false;

    public Jugador(string nombre)
    {
        Nombre = nombre;
        Habilidades = new List<string> { "EscudoProtector", "DobleMovimiento", "", "Habilidad4", "Habilidad5" };
        Enfriamiento = 3; // Enfriamiento reducido a 1 turno
        TurnosDobleMovimiento = 0;
        EscudoActivo = false;
    }

    public void SeleccionarHabilidad(int indice)
    {
        if (indice >= 0 && indice < Habilidades.Count)
        {
            Habilidad = Habilidades[indice];
            Console.WriteLine($"{Nombre} ha seleccionado {Habilidad}");
        }
        else
        {
            Console.WriteLine("Índice de ficha no válido.");
        }
    }

    public void Mover(string tecla, Laberinto laberinto)
    {
        int nuevaFila = Posicion[0];
        int nuevaColumna = Posicion[1];

        switch (tecla.ToLower())
        {
            case "w": nuevaFila--; break; // Arriba
            case "s": nuevaFila++; break; // Abajo
            case "a": nuevaColumna--; break; // Izquierda
            case "d": nuevaColumna++; break; // Derecha
            default:
                Console.WriteLine("Tecla no válida.");
                return;
        }

        if (nuevaFila >= 0 && nuevaFila < laberinto.ObtenerMapa().GetLength(0) &&
            nuevaColumna >= 0 && nuevaColumna < laberinto.ObtenerMapa().GetLength(1) &&
            (AtravesarPared || laberinto.ObtenerMapa()[nuevaFila, nuevaColumna] != 1))

        {
            Posicion[0] = nuevaFila;
            Posicion[1] = nuevaColumna;
            Console.WriteLine($"{Nombre} se ha movido a ({Posicion[0]}, {Posicion[1]})");
            VerificarTrampas(laberinto, nuevaFila, nuevaColumna);
            VerificarVictoria(PosicionVictoria);
            AtravesarPared = false;
        }
        else
        {
            Console.WriteLine("Movimiento inválido.");
        }
    }

    private void VerificarTrampas(Laberinto laberinto, int fila, int columna)
    {
        int celda = laberinto.ObtenerMapa()[fila, columna];
        switch (celda)
        {
            case 2: // bombas
                if (EscudoActivo)
                {
                    Console.WriteLine($"{Nombre} ha pisado una trampa de bombas, pero el escudo lo protege. No pierde vida.");
                    EscudoActivo = false;
                }
                else
                {
                    Vida--;
                    Console.WriteLine($"{Nombre} ha pisado una trampa de bombas y perdió 1 vida. Vida actual: {Vida}");
                }
                laberinto.ReemplazarTrampa(fila, columna, 2);
                break;
            case 3: // Salud
                Vida++;
                Console.WriteLine($"{Nombre} ha encontrado una trampa de salud y ganó 1 vida. Vida actual: {Vida}");
                laberinto.ReemplazarTrampa(fila, columna, 3);
                break;
            case 4: // Teletransporte
                if (EscudoActivo)
                {
                    Console.WriteLine($"{Nombre} ha pisado una trampa de teletransporte, pero el escudo lo protege. No es teletransportado.");
                    EscudoActivo = false;
                }
                else
                {
                    int[] nuevaPosicion = laberinto.Teletransportar();
                    Posicion = nuevaPosicion;
                    Console.WriteLine($"{Nombre} ha sido teletransportado a la posición ({Posicion[0]}, {Posicion[1]}).");
                    laberinto.ReemplazarTrampa(fila, columna, 4);
                }
                break;
        }
        Thread.Sleep(1000);
    }

    public void UsarHabilidad(Laberinto laberinto)
    {
        if (Enfriamiento > 0)
        {
            Console.WriteLine($"{Nombre} la habilidad {Habilidad} está en enfriamiento por {Enfriamiento} turnos más.");
            return;
        }
        if (Habilidad == "EscudoProtector")
        {
            ActivarEscudo();
        }
        if (Habilidad == "DobleMovimiento")
        {
            ActivarDobleMovimiento();
        }
        if (Habilidad == "AtravesarPared")
        {
            ActivarAtravesarPared();
        }
        if (Habilidad == "Curación")
        {
            ActivarCuración();
        }
        if (Habilidad == "Teletransportación")
        {
            ActivarTeletransportación(laberinto);
        }
        Enfriamiento = 3; // Establecer enfriamiento
    }

    private void ActivarEscudo()
    {
        EscudoActivo = true;
        Console.WriteLine($"{Nombre} usa la habilidad Escudo Protector. Ignorará el daño durante un turno.");
    }

    private void ActivarDobleMovimiento()
    {
        Console.WriteLine($"{Nombre} usa la habilidad Doble Movimiento y podrá moverse dos veces en este turno.");
        TurnosDobleMovimiento = 2;
    }

    private void ActivarAtravesarPared()
    {
        Console.WriteLine($"{Nombre} usa la habilidad Atravsar Pared y podrá moverse a través de paredes durante un turno.");
        AtravesarPared = true;
    }
    
    private void ActivarCuración()
    {
        Console.WriteLine($"{Nombre} usa la habilidad Curación y recupera 2 de vida.");
        Vida += 2;
    }

    private void ActivarTeletransportación(Laberinto laberinto)
    {
        var mapa = laberinto.ObtenerMapa();
        int filas = mapa.GetLength(0);
        int columnas = mapa.GetLength(1);

        Random rnd = new Random();
        int x, y;
        do
         {
            x = rnd.Next(filas -3);
            y = rnd.Next(columnas -3);
        } while(mapa[x,y] != 0);

        Posicion[0] = x;
        Posicion[1] = y;
        Console.WriteLine($"{Nombre} usa la habilidad de Teletransportación y se teletransporta a la posición ({x}, {y}).");

    }
    public void ReducirEnfriamiento()
    {
        if (Enfriamiento > 0)
        {
            Enfriamiento--;
        }
    }

    private void VerificarVictoria(int[] posicionVictoria)
    {
        if (Posicion[0] == posicionVictoria[0] && Posicion[1] == posicionVictoria[1])
        {
            if (Vida > 3) // Suponiendo que la vida máxima es 6
            {
                Console.WriteLine($"{Nombre} ha ganado el juego!");
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine($"{Nombre} ha llegado a la posición de victoria, pero no tiene suficiente vida para ganar.");
            }
        }
    }
}