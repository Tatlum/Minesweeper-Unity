using System;

namespace Minesweeper.Core
{
    public class BoardModel
    {
        public int Width { get; }
        public int Height { get; }
        public int MineCount { get; }

        private readonly CellModel[,] _grid;

        public BoardModel(int width, int height, int mineCount)
        {
            Width = width;
            Height = height;
            MineCount = ValidateMineCount(mineCount);
            _grid = new CellModel[width, height];
            
            Reset();
        }

        public CellModel GetCell(int x, int y)
        {
            return IsInBounds(x, y) ? _grid[x, y] : null;
        }

        private bool IsInBounds(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        private int ValidateMineCount(int mineCount)
        {
            return Math.Clamp(mineCount, 0, Width * Height - 1);
        }

        public void Reset()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    _grid[x, y] = new CellModel(x, y);
                }
            }
        }
    }
}
