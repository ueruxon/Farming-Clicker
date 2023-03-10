using Game.Scripts.Logic.GridLayout;
using UnityEngine;

namespace Game.Scripts.Data
{
    [CreateAssetMenu(fileName = "GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [Header("Default Settings")]
        public int OpenCellByDefault = 6;
        
        [Space(2)]
        [Header("Grid Settings")]
        public GridCell CellPrefab;
        public int Width = 3;
        public int Height = 3;
        public float CellSize = 2;
    }
}