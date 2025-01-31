class Program
{
    static void Main()
    {
        int jugadorActual = 1;
        int cont = 0;
        Laberinto laberinto = new Laberinto(17, 17);
        Jugador jugador1 = new Jugador ("Jugador 1") { Posicion = new int[] { 1, 1 }, Vida = 6, PosicionVictoria = new int[] { laberinto.ObtenerMapa().GetLength(0) - 2, laberinto.ObtenerMapa().GetLength(1) - 2 } };
        Jugador jugador2 = new Jugador("Jugador 2") { Posicion = new int[] { laberinto.ObtenerMapa().GetLength(0) - 2, laberinto.ObtenerMapa().GetLength(1) - 2 }, Vida = 6, PosicionVictoria = new int[] { 1, 1 } };

        laberinto.MostrarMapa(jugador1, jugador2);
        SeleccionarHabilidades(jugador1, jugador2);
        Console.Clear();
        laberinto.MostrarMapa(jugador1, jugador2);
        Console.WriteLine("Jugador 1 usa las teclas W/A/S/D.");
        Console.WriteLine("Jugador 2 usa las flechas ↑/←/↓/→.");

        while (true)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(intercept: true);
                if (jugadorActual == 1)
                {
                    ProcesarMovimientoJugador1(key, jugador1, jugador2, laberinto, ref jugadorActual, ref cont);
                }
                else if (jugadorActual == 2)
                {
                    ProcesarMovimientoJugador2(key, jugador1, jugador2, laberinto, ref jugadorActual, ref cont);
                }
            }
        }
    }

    private static void SeleccionarHabilidades(Jugador jugador1, Jugador jugador2)
    {
        Console.WriteLine("Selecciona una habilidad para Jugador 1 (0-4):");
        int seleccion1 = int.Parse(Console.ReadLine());
        jugador1.SeleccionarHabilidad(seleccion1);
        Console.WriteLine("Selecciona una habilidad para Jugador 2 (0-4):");
        int seleccion2 = int.Parse(Console.ReadLine());
        jugador2.SeleccionarHabilidad(seleccion2);
    }

    private static void ProcesarMovimientoJugador1(ConsoleKeyInfo key, Jugador jugador1, Jugador jugador2, Laberinto laberinto, ref int jugadorActual, ref int cont)
    {
        if (key.Key == ConsoleKey.W || key.Key == ConsoleKey.A || key.Key == ConsoleKey.S || key.Key == ConsoleKey.D)
        {
            string ?movimiento = key.Key switch
            {
                ConsoleKey.W => "w",
                ConsoleKey.A => "a",
                ConsoleKey.S => "s",
                ConsoleKey.D => "d",
                _ => null
            };
            if (movimiento != null)
            {
                jugador1.Mover(movimiento, laberinto);
                if (jugador1.TurnosDobleMovimiento > 0 && cont < 1)
                {
                    cont += 1;
                    jugador1.TurnosDobleMovimiento--;
                    Console.WriteLine($"{jugador1.Nombre} tiene {jugador1.TurnosDobleMovimiento} turnos de doble movimiento restantes.");
                }
                else
                {
                    cont = 0;
                    jugadorActual = 2; // Cambiar al jugador 2
                    jugador1.ReducirEnfriamiento();
                }
                laberinto.MostrarMapa(jugador1, jugador2);
            }
        }
        else if (key.Key == ConsoleKey.B)
        {
            jugador1.UsarHabilidad(laberinto);
            laberinto.MostrarMapa(jugador1, jugador2);
        }
    }

    private static void ProcesarMovimientoJugador2(ConsoleKeyInfo key, Jugador jugador1, Jugador jugador2, Laberinto laberinto, ref int jugadorActual, ref int cont)
    {
        if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.RightArrow)
        {
            string ?movimiento = key.Key switch
            {
                ConsoleKey.UpArrow => "w",
                ConsoleKey.LeftArrow => "a",
                ConsoleKey.DownArrow => "s",
                ConsoleKey.RightArrow => "d",
                _ => null
            };
            if (movimiento != null)
            {
                jugador2.Mover(movimiento, laberinto);
                if (jugador2.TurnosDobleMovimiento > 0 && cont < 1)
                {
                    cont += 1;
                    jugador2.TurnosDobleMovimiento--;
                    Console.WriteLine($"{jugador2.Nombre} tiene {jugador2.TurnosDobleMovimiento} turnos de doble movimiento restantes.");
                }
                else
                {
                    cont = 0;
                    jugadorActual = 1; // Cambiar al jugador 1
                    jugador2.ReducirEnfriamiento();
                }
                laberinto.MostrarMapa(jugador1, jugador2);
            }
        }
        else if (key.Key == ConsoleKey.B)
        {
            jugador2.UsarHabilidad(laberinto);
        }
    }
}