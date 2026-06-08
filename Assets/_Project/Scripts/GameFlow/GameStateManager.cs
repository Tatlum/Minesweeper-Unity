using System;

namespace Minesweeper.GameFlow
{
    public class GameStateManager
    {
        public event Action<GameState> OnStateChanged;
        
        public GameState CurrentState { get; private set; }

        public GameStateManager()
        {
            ChangeState(GameState.Menu);
        }

        public void ChangeState(GameState newState)
        {
            CurrentState = newState;
            OnStateChanged?.Invoke(newState);
        }
    }
}
