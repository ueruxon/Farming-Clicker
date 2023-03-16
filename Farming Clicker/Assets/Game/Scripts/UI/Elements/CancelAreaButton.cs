﻿using Game.Scripts.Logic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Elements
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
            
            _button.onClick.AddListener(OnCancelArea);
        }

        public void Show() => 
            gameObject.SetActive(true);

        public void Hide() => 
            gameObject.SetActive(false);

        private void OnProductionAreaChoices() => 
            Show();

        private void OnCancelArea() => 
            _farmController.CancelConstruction();

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnCancelArea);
            _farmController.ProductionAreaChoices -= Show;
            _farmController.ProductionAreaCanceled -= Hide;
        }
    }
}