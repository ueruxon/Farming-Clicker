using Game.Scripts.Logic.GridLayout;
using UnityEngine;

namespace Game.Scripts.Data
{
    [CreateAssetMenu(fileName = "GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [Header("Grid Settings")]
        [Space(2)]
        public GridCell CellPrefab;
        public int Width = 3;
        public int Height = 3;
        public float CellSize = 2;
    }
}