using System;
using System.Collections.Generic;
using Game.Scripts.Infrastructure.Services.Progress;
using Game.Scripts.Logic.GridLayout;
using UnityEngine;

namespace Game.Scripts.Data.Game
{
    [CreateAssetMenu(fileName = "GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [Header("Default Settings")]
        [SerializeField] private int _penCellByDefault = 6;
        [SerializeField] private List<ResourceData> _initialResources;

        [Space(2)]
        [Header("Grid Settings")]
        public GridCell CellPrefab;
        public int Width = 3;
        public int Height = 3;
        public float CellSize = 2;
        
        public int OpenCellByDefault => 
            Mathf.Clamp(_penCellByDefault, 0, Width * Height);

        public List<ResourceData> GetInitialResources() => 
            _initialResources;
    }

    [Serializable]
    public class ResourceData
    {
        public ResourceType ResourceType;
        public int Amount;
    }
}