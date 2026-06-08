using System;
using System.Collections.Generic;

namespace Minesweeper.Core
{
    public class MineGenerationService
    {
        private readonly Random _random;

        public MineGenerationService(Random random)
        {
            _random = random;
        }

        public void GenerateMines(BoardModel board, int startX, int startY)
        {
            var availableCells = new List<(int x, int y)>(
                board.Width * board.Height - 1);

            for (int x = 0; x < board.Width; x++)
            {
                for (int y = 0; y < board.Height; y++)
                {
                    if (x == startX && y == startY)
                    {
                        continue;
                    }

                    availableCells.Add((x, y));
                }
            }

            Shuffle(availableCells);

            for (int i = 0; i < board.MineCount; i++)
            {
                var (x, y) = availableCells[i];
                board.GetCell(x, y).IsMine = true;
            }
        }

        private void Shuffle(List<(int x, int y)> cells)
        {
            for (int i = cells.Count - 1; i > 0; i--)
            {
                int j = _random.Next(i + 1);

                (cells[i], cells[j]) = (cells[j], cells[i]);
            }
        }
    }
}
