using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        Console.WriteLine("Bienvenido a la simulación de la carrera");
        Console.WriteLine("Número de jugadores (mínimo 2):");
        int numPlayers = int.Parse(Console.ReadLine());

        if (numPlayers < 2)
        {
            Console.WriteLine("El número mínimo de jugadores es 2.");
            return;
        }

        Console.WriteLine("Iniciando la carrera...\n");

        List<Task<(int playerId, int position)>> tasks = new List<Task<(int, int)>>();

        for (int i = 1; i <= numPlayers; i++)
        {
            int playerId = i;
            var task = Task.Factory.StartNew<(int, int)>(() =>
            {
                int position = 0;
                Random random = new Random();
                while (position < 100)
                {
                    position += random.Next(1, 10);
                    Console.WriteLine($"Jugador {playerId} avanzó a la posición {position}");
                    Thread.Sleep(random.Next(100, 300));
                }
                return (playerId, position);
            }, TaskCreationOptions.AttachedToParent);
            tasks.Add(task);
        }

        Task.Factory.ContinueWhenAny(tasks.ToArray(), winnerTask =>
        {
            var result = winnerTask.Result;
            Console.WriteLine($"\nJugador {result.playerId} ganó la carrera alcanzando la posición {result.position}!");
        });

        Task.WhenAll(tasks).ContinueWith(_ =>
        {
            Console.WriteLine("\nLa carrera ha terminado.");
        }).Wait();
    }
}
