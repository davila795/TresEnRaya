using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TresEnRaya
{
    internal class Program
    {
        static char[] tablero = {
            '1', '2', '3',
            '4', '5', '6',
            '7', '8', '9'
        };
        static List<List<char>> lineasGanadoras = new List<List<char>> {
            new List<char> { '1', '2', '3' },
            new List<char> { '4', '5', '6' },
            new List<char> { '7', '8', '9' },
            new List<char> { '1', '4', '7' },
            new List<char> { '2', '5', '8' },
            new List<char> { '3', '6', '9' },
            new List<char> { '1', '5', '9' },
            new List<char> { '3', '5', '7' }
        };
        static int turno = 1;
        static char ficha = 'X';
        static bool finJuego = false;
        static int ganador = 0;

        static void Main(string[] args)
        {
            PintarTablero();

            while (!finJuego)
            {
                Mover();
                PintarTablero();
                finJuego = ComprobarFinalJuego();
                if (!finJuego)
                    CambiarTurno();
            }

            if (ganador > 0)
                Console.WriteLine($"JUGADOR {ganador} GANA!");
            else
                Console.WriteLine("EMPATE!");

            Console.WriteLine("Juego terminado. Presiona ENTER para salir...");
            Console.ReadLine();
        }

        static void PintarTablero()
        {
            Console.Clear();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.Write(tablero[i * 3 + j] + "|");
                }
                Console.WriteLine("\n------");
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

        static bool ComprobarVictoria()
        {
            //  Fila 1
            if (tablero[0] == tablero[1] && tablero[1] == tablero[2])
                return true;
            //  Fila 2
            if (tablero[3] == tablero[4] && tablero[4] == tablero[5])
                return true;
            //  Fila 3
            if (tablero[6] == tablero[7] && tablero[7] == tablero[8])
                return true;
            //  Columna 1
            if (tablero[0] == tablero[3] && tablero[3] == tablero[6])
                return true;
            //  Columna 2
            if (tablero[1] == tablero[4] && tablero[4] == tablero[7])
                return true;
            //  Columna 3
            if (tablero[2] == tablero[5] && tablero[5] == tablero[8])
                return true;
            //Diagonal 1
            if (tablero[0] == tablero[4] && tablero[4] == tablero[8])
                return true;
            //Diagonal 2
            if (tablero[2] == tablero[4] && tablero[4] == tablero[6])
                return true;

            return false;
        }

        static bool ComprobarEmpate()
        {
            for (int i = 0; i < tablero.Length; i++)
            {
                if (tablero[i] != 'X' && tablero[i] != 'O')
                    return false;
            }
            return true;
        }

        static void Mover()
        {
            if (turno == 1)
                MoverJugador();
            else
                MoverMaquina();
        }

        static void MoverJugador()
        {
            bool movimientoCorrecto = false;
            int casilla;

            do
            {
                Console.Write($"Jugador {turno} selecciona una casilla: ");
                string inputCasilla = Console.ReadLine();

                if (int.TryParse(inputCasilla, out casilla))
                    movimientoCorrecto = ComprobarMovimiento(casilla);

                if (!movimientoCorrecto)
                    Console.WriteLine("Por favor, selecciona una casilla valida");

            } while (!movimientoCorrecto);

            char posicion = casilla.ToString()[0];
            marcarPosicionDeLineasGanadoras(posicion);
            tablero[casilla - 1] = ficha;
        }

        static void MoverMaquina()
        {
            char jugada = BuscarJugada();
            int index = Array.IndexOf(tablero, jugada);
            tablero[index] = ficha;
        }

        static char BuscarJugada()
        {
            //  buscar jugada ganadora
            foreach (List<char> linea in lineasGanadoras)
            {
                if (linea.Count == 1 && !linea.Contains('X'))
                {
                    char temp = linea[0];
                    eliminarPosicionDeLineasGanadoras(temp);
                    return temp;
                }
            }
            //  buscar bloquear
            foreach (List<char> linea in lineasGanadoras)
            {
                if (linea.Count == 3 && linea.Count(x => x == 'X') == 2)
                {
                    char temp = linea.Find(e => e != 'X');
                    eliminarPosicionDeLineasGanadoras(temp);
                    return temp;
                }
            }
            //  buscar jugada adyacente
            Random rdm = new Random();
            foreach (List<char> linea in lineasGanadoras)
            {
                if (linea.Count < 3 && !linea.Contains('X'))
                {
                    int index = rdm.Next(linea.Count);
                    char temp = linea[index];
                    eliminarPosicionDeLineasGanadoras(temp);
                    return temp;
                }
            }
            //  jugada por default
            List<char> lineaDisponible = lineasGanadoras.First(ComprobarLineaDisponible);
            char casillaDisponible = lineaDisponible.First(CasillaNoEsX);
            eliminarPosicionDeLineasGanadoras(casillaDisponible);
            return casillaDisponible;
        }

        static void eliminarPosicionDeLineasGanadoras(char posicion)
        {
            foreach (List<char> linea in lineasGanadoras)
            {
                if (linea.Contains(posicion))
                    linea.Remove(posicion);
            }
        }

        static void marcarPosicionDeLineasGanadoras(char posicion)
        {
            foreach (List<char> linea in lineasGanadoras)
            {
                if (linea.Contains(posicion))
                {
                    int index = linea.IndexOf(posicion);
                    linea[index] = 'X';
                }
            }
        }

        static bool ComprobarLineaDisponible(List<char> linea)
        {
            bool hayCasillaDisponible = linea.Exists(CasillaNoEsX);
            return hayCasillaDisponible;
        }

        static bool CasillaNoEsX(char casilla)
        {
            return (casilla != 'X');
        }

        static bool ComprobarMovimiento(int casilla)
        {
            bool movimientoValido = casilla > 0
                && casilla <= tablero.Length
                && tablero[casilla - 1] != 'X'
                && tablero[casilla - 1] != 'O';

            return movimientoValido;
        }

        static void CambiarTurno()
        {
            if (turno == 1)
            {
                ficha = 'O';
                turno++;
            }
            else
            {
                ficha = 'X';
                turno--;
            }
        }
    }
}
