using System;
using System.Collections.Generic;

namespace Puzzle15
{
    public class GameEngine
    {
        private int[,] board;
        private int size;
        private int emptyRow;
        private int emptyCol;

        public int Moves { get; private set; }

        public GameEngine(int size)
        {
            this.size = size;
            board = new int[size, size];
            StartNewGame();
        }

        public void StartNewGame()
        {
            Moves = 0;
            int counter = 1;
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    board[i, j] = counter++;

            board[size - 1, size - 1] = 0;
            emptyRow = size - 1;
            emptyCol = size - 1;
            Shuffle();
        }

        private void Shuffle()
        {
            Random rand = new Random();
            for (int i = 0; i < size * size * 10; i++)
            {
                int moveType = rand.Next(4);
                int r = emptyRow + (moveType == 0 ? -1 : moveType == 1 ? 1 : 0);
                int c = emptyCol + (moveType == 2 ? -1 : moveType == 3 ? 1 : 0);

                if (r >= 0 && r < size && c >= 0 && c < size)
                {
                    board[emptyRow, emptyCol] = board[r, c];
                    board[r, c] = 0;
                    emptyRow = r;
                    emptyCol = c;
                }
            }
        }

        // --- МЕТОД ДЛЯ ЗАВАНТАЖЕННЯ ЗБЕРЕЖЕНОЇ ГРИ ---
        public void LoadState(int savedMoves, List<int> flatBoard)
        {
            Moves = savedMoves;
            int idx = 0;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    board[i, j] = flatBoard[idx++];
                    if (board[i, j] == 0)
                    {
                        emptyRow = i;
                        emptyCol = j;
                    }
                }
            }
        }

        // --- МЕТОД ДЛЯ ЕКСПОРТУ ПОЛЯ В ОДНОВИМІРНИЙ МАСИВ (ДЛЯ ЗБЕРЕЖЕННЯ) ---
        public List<int> GetFlatBoard()
        {
            List<int> flat = new List<int>();
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    flat.Add(board[i, j]);
            return flat;
        }

        public bool TryMove(int r, int c)
        {
            if (r < 0 || r >= size || c < 0 || c >= size) return false;

            if (Math.Abs(emptyRow - r) + Math.Abs(emptyCol - c) == 1)
            {
                board[emptyRow, emptyCol] = board[r, c];
                board[r, c] = 0;
                emptyRow = r;
                emptyCol = c;
                Moves++;
                return true;
            }
            return false;
        }

        public int GetValue(int r, int c) => board[r, c];
        public int Size => size;

        public bool CheckWin()
        {
            int counter = 1;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i == size - 1 && j == size - 1) return board[i, j] == 0;
                    if (board[i, j] != counter++) return false;
                }
            }
            return true;
        }
    }
}