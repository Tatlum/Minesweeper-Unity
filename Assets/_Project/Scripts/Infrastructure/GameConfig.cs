using UnityEngine;

namespace Minesweeper.Infrastructure
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Minesweeper/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [Header("Board Settings")]
        [SerializeField] private int _width = 16;
        [SerializeField] private int _height = 16;
        [SerializeField] private int _mineCount = 40;

        public int Width => _width;
        public int Height => _height;
        public int MineCount => _mineCount;
    }
}
