using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TresEnRaya
{
    internal class Tablero
    {
        private char[,] board;

        public Tablero()
        {
            board = new char[3, 3];
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    board[row, col] = ' ';
                }
            }

        }
        public void PrintBoard()
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    Console.Write(board[row, col]);
                    if (col < 2)
                    {
                        Console.Write(" | ");
                    }
                }
                Console.WriteLine();
                if (row < 2)
                {
                    Console.WriteLine("---------");
                }
            }
        }
    }
}
