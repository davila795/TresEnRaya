using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TresEnRaya
{
    internal class Juego
    {
        Tablero tablero {  get; set; }
        Jugador jugador { get; set; }

        public Juego()
        {
            tablero = new Tablero();
            jugador = new Jugador();
            tablero.PrintBoard();
            Console.ReadLine();
        }
    }
}
