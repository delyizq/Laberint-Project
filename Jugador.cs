public class Jugador
{
    public string Nombre { get; set; }
    public int Vida { get; set; }
    public List<string> Fichas { get; private set; }
    public string FichaSeleccionada { get; set; }
    public int[] Posicion { get; set; } // [fila, columna]
    public int[] PosicionVictoria;
    public string Habilidad { get; set; }
    public int Enfriamiento { get; set; }
    public int TurnosDobleMovimiento { get; set; }
    public bool EscudoActivo { get; private set; } // Indica si el escudo está activo

    public Jugador(string nombre, string habilidad)
    {
        Nombre = nombre;
        Fichas = new List<string> { "Ficha1", "Ficha2", "Ficha3", "Ficha4", "Ficha5" };
        FichaSeleccionada = "";
        Habilidad = habilidad;
        Enfriamiento = 3; // Enfriamiento reducido a 1 turno
        TurnosDobleMovimiento = 0;
        EscudoActivo = false;
    }

    public void SeleccionarFicha(int indice)
    {
        if (indice >= 0 && indice < Fichas.Count)
        {
            FichaSeleccionada = Fichas[indice];
            Console.WriteLine($"{Nombre} ha seleccionado {FichaSeleccionada}");
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
            laberinto.ObtenerMapa()[nuevaFila, nuevaColumna] != 1)
        {
            Posicion[0] = nuevaFila;
            Posicion[1] = nuevaColumna;
            Console.WriteLine($"{Nombre} se ha movido a ({Posicion[0]}, {Posicion[1]})");
            VerificarTrampas(laberinto, nuevaFila, nuevaColumna);
            VerificarVictoria(PosicionVictoria);
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
            case 2: // Pinchos
                if (EscudoActivo)
                {
                    Console.WriteLine($"{Nombre} ha pisado una trampa de pinchos, pero el escudo lo protege. No pierde vida.");
                    EscudoActivo = false;
                }
                else
                {
                    Vida--;
                    Console.WriteLine($"{Nombre} ha pisado una trampa de pinchos y perdió 1 vida. Vida actual: {Vida}");
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