using System;
using Game.Scripts.Data.Game;
using Game.Scripts.Infrastructure.Services.Progress;
using Game.Scripts.UI.Services.Factory;
using UnityEngine;

namespace Game.Scripts.Logic.Tutorials.Tasks
{
    public abstract class TutorialTask : ScriptableObject
    {
        public event Action<TutorialTaskData> TaskStarted;
        public event Action TaskCompleted;

        public TutorialTaskData TaskData;

        protected UpgradeRepository UpgradeRepository;
        protected ResourceRepository ResourceRepository;
        protected FarmController FarmController;
        protected UIFactory UIFactory;

        public virtual void Init(IGameProgressService progressService, FarmController farmController, UIFactory uiFactory)
        {
            UpgradeRepository = progressService.Progress.UpgradeRepository;
            ResourceRepository = progressService.Progress.ResourceRepository;
            FarmController = farmController;
            UIFactory = uiFactory;
        }

        public virtual void OnStart() => 
            TaskStarted?.Invoke(TaskData);

        public virtual void Execute() {}

        public virtual void OnComplete() => 
            TaskCompleted?.Invoke();
    }

    [Serializable]
    public class TutorialTaskData
    {
        [TextArea]
        public string Description;
        public int TaskNumber;
        public bool IsLastTask = false;
    }
}