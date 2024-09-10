using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TresEnRaya
{
    internal class Program
    {
        static int numTablas = 0;
        static char[,] tablero;
        static int turno = 1;
        static char ficha = 'X';
        static bool finJuego = false;
        static int ganador = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("Di el tamañano del tablero: ");
            int.TryParse(Console.ReadLine(), out numTablas);
            //tableroNumeros = new int[numTablas, numTablas];
            tablero = new char[numTablas, numTablas];

            for (int i = 0; i < tablero.GetLength(0); i++)
            {
                for (int j = 0; j < tablero.GetLength(1); j++)
                {
                    tablero[i, j] = '*';
                }
            }

            PintarTablero();

            while (!finJuego)
            {
                Mover();
                Console.Clear();
                PintarTablero();
                finJuego = ComprobarFinalJuego();
                if (!finJuego)
                    CambiarTurno();
            }

            if (ganador > 0)
            {
                Console.WriteLine($"JUGADOR {ganador} GANA!");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("EMPATE!");
                Console.ReadKey();
            }

        }

        static void PintarTablero()
        {
            for (int i = 0; i < tablero.GetLength(0); i++)
            {
                Console.Write(i + 1);

                for (int j = 0; j < tablero.GetLength(1); j++)
                {
                    Console.Write(tablero[i, j] + "|");
                }
                Console.WriteLine("  \n------------------------");
            }

            for (int i = 1; i <= numTablas; i++)
            {
                Console.Write($" {i}");
            }
        }

        static bool ComprobarFinalJuego()
        {
            bool victoria = ComprobarVictoria();

            if (victoria)
            {
                ganador = turno;
                return victoria;
            }
            else
                return ComprobarEmpate();
        }

        //static bool ComprobarVictoria()
        //{
        //    for (int i = 0; i < numTablas; i++)
        //    {
        //        bool condicion = true;
        //        char temp1 = tablero[i, 0];
        //        for (int j = 0; j < numTablas; j++)
        //        {
        //            char temp2 = tablero[0, j];

        //            char posicionTablero = tablero[i, j];
        //            if (temp1 != posicionTablero || temp1 == '*')
        //                condicion = false;
        //        }
        //        if (condicion)
        //            return true;
        //    }
        //    return false;
        //}

        static bool ComprobarVictoria()
        {
            bool condicion = true;

            // Check horizontal win condition
            for (int i = 0; i < numTablas; i++)
            {
                char temp1 = tablero[i, 0];
                for (int j = 0; j < numTablas; j++)
                {
                    char posicionTablero = tablero[i, j];
                    if (temp1 != posicionTablero || temp1 == '*')
                        condicion = false;
                }
                if (condicion)
                    return true;
            }

            // Check vertical win condition
            for (int j = 0; j < numTablas; j++)
            {
                condicion = true;
                char temp1 = tablero[0, j];
                for (int i = 0; i < numTablas; i++)
                {
                    char posicionTablero = tablero[i, j];
                    if (temp1 != posicionTablero || temp1 == '*')
                        condicion = false;
                }
                if (condicion)
                    return true;
            }

            // Check diagonal win condition
            char tempDiagonal = tablero[0, 0];
            for (int i = 1; i < numTablas; i++)
            {
                condicion = true;
                if (tempDiagonal != tablero[i, i] || tempDiagonal == '*')
                {
                    condicion = false;
                }
            }
            if (condicion)
                return true;



            int posFinal = numTablas - 1;
            char tempFinal = tablero[0, posFinal];

            if (tempFinal != '*')
            {
                for (int i = 1; i < numTablas; i++)
                {
                    condicion = true;

                    if (tempFinal != tablero[i, posFinal - i])
                    {
                        condicion = false;
                    }
                }
                if (condicion)
                    return true;
            }

            return false;
        }

        static bool ComprobarEmpate()
        {
            for (int i = 0; i < tablero.GetLength(0); i++)
            {
                for (int j = 0; j < tablero.GetLength(1); j++)
                {
                    if (tablero[i, j] != 'X' && tablero[i, j] != 'O')
                        return false;
                }
            }
            return true;
        }

        static void Mover()
        {
            bool movimientoCorrecto = false;
            //Maquina simbolo O (jugador 2)
            //Jugador simbolo X (jugador 1)
            Random rnd = new Random();
            int casilla1 = 0;
            int casilla2 = 0;

            do
            {
                Console.Write($"Jugador {turno} selecciona la casilla mediante dos posiciones x,y: ");

                if (turno == 1)
                {
                    Console.Write($"Jugador {turno} selecciona la primera posición: ");
                    string inputCasilla1 = Console.ReadLine();

                    Console.Write($"Jugador {turno} selecciona la primera posición: ");
                    string inputCasilla2 = Console.ReadLine();

                    if (int.TryParse(inputCasilla1, out casilla1) && int.TryParse(inputCasilla2, out casilla2))
                    {
                        movimientoCorrecto = ComprobarMovimiento(casilla1, casilla2);
                    }

                    if (!movimientoCorrecto)
                        Console.WriteLine("Por favor, selecciona una casilla válida");

                }
                else if (turno == 2)
                {
                    casilla1 = rnd.Next(1, (numTablas * numTablas));
                    casilla2 = rnd.Next(1, (numTablas * numTablas));

                    movimientoCorrecto = ComprobarMovimiento(casilla1, casilla2);

                    if (!movimientoCorrecto)
                        Console.WriteLine("Por favor, selecciona una casilla válida");
                }

            } while (!movimientoCorrecto);

            if (turno == 1)
            {
                tablero[casilla1 - 1, casilla2 - 1] = ficha;
            }
            else
            {
                tablero[casilla1 - 1, casilla2 - 1] = ficha;
            }
        }

        static bool ComprobarMovimiento(int posicion1, int posicion2)
        {
            if (posicion1 < 1 || posicion1 > tablero.GetLength(0) || posicion2 < 1 || posicion2 > tablero.GetLength(1))
            {
                return false;
            }

            return tablero[posicion1 - 1, posicion2 - 1] != 'X' && tablero[posicion1 - 1, posicion2 - 1] != 'O';
        }

        static void CambiarTurno()
        {
            if (turno == 1)
            {
                ficha = 'O';
                turno = 2;
            }
            else
            {
                ficha = 'X';
                turno = 1;
            }
        }
    }
}
