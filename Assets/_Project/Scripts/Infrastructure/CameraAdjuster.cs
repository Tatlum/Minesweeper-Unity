using UnityEngine;

namespace Minesweeper.Infrastructure
{
    public class CameraAdjuster
    {
        private const float _padding = 1.5f;

        public void AdjustByBoardSize(Camera camera, Vector2 boardScreenSize)
        {
            var verticalSize = boardScreenSize.y / 2f + _padding;
            var horizontalSize = (boardScreenSize.x / 2f + _padding) / camera.aspect;

            camera.orthographicSize = Mathf.Max(verticalSize, horizontalSize);
            camera.transform.position = new Vector3(0, 0, -10);
        }
    }
}
