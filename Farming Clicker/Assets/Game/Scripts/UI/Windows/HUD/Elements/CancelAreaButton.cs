﻿using Game.Scripts.Logic;
using Game.Scripts.Logic.Production;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Scripts.UI.Windows.HUD.Elements
{
    public class CancelAreaButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private FarmController _farmController;
        
        
        public void Init(FarmController farmController)
        {
            _farmController = farmController;
            _farmController.ProductionAreaChoices += Show;
            _farmController.ProductionAreaCanceled += Hide;
            _farmController.ProductionAreaBuilt += Hide;
            
            _button.onClick.AddListener(OnCancelArea);
        }

        public void Hide() => 
            gameObject.SetActive(false);

        private void Show(ProductType productType) => 
            gameObject.SetActive(true);

        private void OnCancelArea() => 
            _farmController.CancelConstruction();

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnCancelArea);
            _farmController.ProductionAreaChoices -= Show;
            _farmController.ProductionAreaCanceled -= Hide;
            _farmController.ProductionAreaBuilt -= Hide;
        }
    }
}