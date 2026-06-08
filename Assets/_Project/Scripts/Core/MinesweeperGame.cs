using System;

namespace Minesweeper.Core
{
    public class MinesweeperGame
    {
        public event Action OnGameWon;
        public event Action OnGameLost;
        
        public event Action OnBoardChanged;
        
        public bool IsFirstMoveDone { get; private set; }

        private readonly BoardModel _boardModel;
        private readonly MineGenerationService _mineGenerator;
        private readonly BoardTraversalService _traversal;
        
        public MinesweeperGame(
            BoardModel boardModel, 
            MineGenerationService mineGenService, 
            BoardTraversalService traversalService)
        {
            _boardModel = boardModel;
            _mineGenerator = mineGenService;
            _traversal = traversalService;
        }

        public void RevealCell(int x, int y)
        {
            var cell = _boardModel.GetCell(x, y);
            
            if (cell == null || cell.IsRevealed || cell.IsFlagged)
            {
                return;
            }

            if (!IsFirstMoveDone)
            {
                IsFirstMoveDone = true;
                _mineGenerator.GenerateMines(_boardModel, x, y);
                _traversal.CalculateAdjacentMines(_boardModel);
            }

            if (cell.IsMine)
            {
                _traversal.RevealAllBoard(_boardModel);
                OnBoardChanged?.Invoke();
                OnGameLost?.Invoke();
                return;
            }

            _traversal.RevealArea(_boardModel, x, y);

            if (CheckWinCondition())
            {
                _traversal.RevealAllBoard(_boardModel);
                OnBoardChanged?.Invoke();
                OnGameWon?.Invoke();
                return;
            }
            
            OnBoardChanged?.Invoke();
        }

        public void ToggleFlag(int x, int y)
        {
            var cell = _boardModel.GetCell(x, y);
            
            if (cell == null || cell.IsRevealed)
            {
                return;
            }

            cell.IsFlagged = !cell.IsFlagged;
            OnBoardChanged?.Invoke();
        }

        private bool CheckWinCondition()
        {
            for (int x = 0; x < _boardModel.Width; x++)
            {
                for (int y = 0; y < _boardModel.Height; y++)
                {
                    var cell = _boardModel.GetCell(x, y);

                    if (!cell.IsMine && !cell.IsRevealed)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void RestartGame()
        {
            IsFirstMoveDone = false;
            _boardModel.Reset(); 
            OnBoardChanged?.Invoke();
        }
    }
}
