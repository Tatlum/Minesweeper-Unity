using System.Collections.Generic;

namespace Minesweeper.Core
{
    public class BoardTraversalService
    {
        private static readonly (int X, int Y)[] NeighborOffsets =
        {
            (-1, -1), (0, -1), (1, -1),
            (-1,  0),          (1,  0),
            (-1,  1), (0,  1), (1,  1)
        };
        
        public void CalculateAdjacentMines(BoardModel board)
        {
            for (int x = 0; x < board.Width; x++)
            {
                for (int y = 0; y < board.Height; y++)
                {
                    var cell = board.GetCell(x, y);

                    if (cell.IsMine)
                        continue;

                    int count = 0;

                    foreach (var (dx, dy) in NeighborOffsets)
                    {
                        var neighbor = board.GetCell(x + dx, y + dy);

                        if (neighbor != null && neighbor.IsMine)
                            count++;
                    }

                    cell.AdjacentMines = count;
                }
            }
        }

        public void RevealArea(BoardModel board, int startX, int startY)
        {
            var queue = new Queue<CellModel>();

            queue.Enqueue(board.GetCell(startX, startY));

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (current.IsRevealed || current.IsFlagged)
                    continue;

                current.IsRevealed = true;

                if (current.IsMine || current.AdjacentMines > 0)
                    continue;

                foreach (var (dx, dy) in NeighborOffsets)
                {
                    var neighbor = board.GetCell(current.X + dx, current.Y + dy);

                    if (neighbor != null &&
                        !neighbor.IsRevealed &&
                        !neighbor.IsFlagged)
                    {
                        queue.Enqueue(neighbor);
                    }
                }
            }
        }
        
        public void RevealAllBoard(BoardModel board)
        {
            for (int x = 0; x < board.Width; x++)
            {
                for (int y = 0; y < board.Height; y++)
                {
                    var cell = board.GetCell(x, y);
                    cell.IsFlagged = false;
                    cell.IsRevealed = true;
                }
            }
        }
    }
}
