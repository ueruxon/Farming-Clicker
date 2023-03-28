using Game.Scripts.Common;
using Game.Scripts.Logic;
using Game.Scripts.Logic.Cameras;
using Game.Scripts.Logic.GridLayout;
using Game.Scripts.Logic.Tutorials;
using Game.Scripts.Logic.Upgrades;
using Game.Scripts.UI.Services.Factory;
using UnityEngine;

namespace Game.Scripts.Infrastructure.Setup.Game
{
    public class SceneInitializer
    {
        private readonly UIFactory _uiFactory;
        private readonly FarmController _farmController;
        private readonly UpgradesHandler _upgradesHandler;
        private readonly TutorialController _tutorialController;
        private readonly CameraController _cameraController;
        private readonly GridSystem _gridSystem;

        public SceneInitializer(
            UIFactory uiFactory,
            FarmController farmController,
            UpgradesHandler upgradesHandler, 
            TutorialController tutorialController,
            CameraController cameraController,
            GridSystem gridSystem)
        {
            _uiFactory = uiFactory;
            _farmController = farmController;
            _upgradesHandler = upgradesHandler;
            _tutorialController = tutorialController;
            _cameraController = cameraController;
            _gridSystem = gridSystem;
        }

        public void SetupScene(Transform gridContainer)
        {
            InitGameplaySystems(gridContainer);
            InitUI();
            InitGameWorld();
            InitTutorial();
        }

        private void InitGameplaySystems(Transform gridContainer)
        {
            _gridSystem.Init(gridContainer);
            _upgradesHandler.Init();
            _farmController.Init();
            _cameraController.Init();
        }

        private void InitUI()
        {
            _uiFactory.CreateUIRoot();
            _uiFactory.CreateHUD();
            _uiFactory.CreateShop();
            _uiFactory.CreateSelectProductArea();
            _uiFactory.CreateTutorialWindow();
        }
        
        private void InitGameWorld() => 
            _farmController.CreateFarm();

        private void InitTutorial() => 
            _tutorialController.Init();

        public void CleanUp()
        {
            _farmController.CleanUp();
            _cameraController.CleanUp();
            _uiFactory.CleanUp();
            _tutorialController.CleanUp();
        }
    }
}