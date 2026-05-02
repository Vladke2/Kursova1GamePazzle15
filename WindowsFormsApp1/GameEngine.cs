using System;
using System.Collections.Generic;
using System.Drawing;

namespace Puzzle15
{
    public class GameEngine
    {
        private int[,] board;
        private int size;
        private int emptyRow;
        private int emptyCol;

        public int Moves { get; private set; }

        // Наша "відеокамера" для запису ходів
        public List<Point> History { get; private set; }

        public GameEngine(int size)
        {
            this.size = size;
            board = new int[size, size];
            History = new List<Point>();
            StartNewGame();
        }

        public void StartNewGame()
        {
            Moves = 0;
            History.Clear();
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
            // Оптимальна кількість заплутувань (щоб авторозв'язок не тривав вічність)
            int shuffleCount = size * size * 5;

            for (int i = 0; i < shuffleCount; i++)
            {
                List<Point> validMoves = new List<Point>();
                if (emptyRow > 0) validMoves.Add(new Point(emptyRow - 1, emptyCol));
                if (emptyRow < size - 1) validMoves.Add(new Point(emptyRow + 1, emptyCol));
                if (emptyCol > 0) validMoves.Add(new Point(emptyRow, emptyCol - 1));
                if (emptyCol < size - 1) validMoves.Add(new Point(emptyRow, emptyCol + 1));

                Point move = validMoves[rand.Next(validMoves.Count)];

                // ЗАПИСУЄМО ПОЗИЦІЮ ПУСТОЇ КЛІТИНКИ ДО ХОДУ
                History.Add(new Point(emptyRow, emptyCol));

                board[emptyRow, emptyCol] = board[move.X, move.Y];
                board[move.X, move.Y] = 0;
                emptyRow = move.X;
                emptyCol = move.Y;
            }
        }

        public void LoadState(int savedMoves, List<int> flatBoard)
        {
            Moves = savedMoves;
            History.Clear(); // При завантаженні історія очищується
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
                // ЗАПИСУЄМО ХІД ГРАВЦЯ В ІСТОРІЮ
                History.Add(new Point(emptyRow, emptyCol));

                board[emptyRow, emptyCol] = board[r, c];
                board[r, c] = 0;
                emptyRow = r;
                emptyCol = c;
                Moves++;
                return true;
            }
            return false;
        }

        // --- МЕТОД ДЛЯ АВТОПІЛОТА (Тихо відмотує час назад) ---
        public void ReverseMove(int r, int c)
        {
            if (r < 0 || r >= size || c < 0 || c >= size) return;
            board[emptyRow, emptyCol] = board[r, c];
            board[r, c] = 0;
            emptyRow = r;
            emptyCol = c;
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