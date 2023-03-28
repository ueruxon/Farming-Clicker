using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Infrastructure.Services.AssetManagement;
using Game.Scripts.Infrastructure.Services.Progress;
using Game.Scripts.Logic.Tutorials.Tasks;
using Game.Scripts.UI.Services.Factory;
using Object = UnityEngine.Object;

namespace Game.Scripts.Logic.Tutorials
{
    public class TutorialController
    {
        public event Action<TutorialTaskData> TaskStarted;
        public event Action TutorialCompleted;

        private readonly IGameProgressService _progressService;
        private readonly FarmController _farmController;
        private readonly TutorialTaskProvider _provider;
        private readonly UIFactory _uiFactory;

        private List<TutorialTask> _allTasks;
        private Queue<TutorialTask> _taskQueue;

        public TutorialController(IGameProgressService progressService, IAssetProvider assetProvider,
            FarmController farmController, UIFactory uiFactory)
        {
            _progressService = progressService;
            _farmController = farmController;
            _uiFactory = uiFactory;
            _provider = new TutorialTaskProvider(assetProvider);
            
            _taskQueue = new Queue<TutorialTask>();
            _allTasks = new List<TutorialTask>();
        }

        public void Init()
        {
            var tasks = _provider.GetTutorialTasks()
                .OrderBy(x => x.TaskData.TaskNumber)
                .ToList();

            foreach (TutorialTask tutorialTask in tasks)
            {
                TutorialTask task = Object.Instantiate(tutorialTask);
                task.Init(_progressService, _farmController, _uiFactory);
                task.TaskStarted += OnTaskStarted;
                task.TaskCompleted += OnTaskCompleted;

                _allTasks.Add(task);
                _taskQueue.Enqueue(task);
            }
            
            SetNextTask();
        }

        private void OnTaskStarted(TutorialTaskData taskData) => 
            TaskStarted?.Invoke(taskData);

        private void OnTaskCompleted()
        {
            if (HasTask()) 
                SetNextTask();
            else
                TutorialCompleted?.Invoke();
        }

        private bool HasTask() =>
            _taskQueue.Count > 0;

        private void SetNextTask()
        {
            var task = _taskQueue.Dequeue();
            task.OnStart();
        }

        public void CleanUp()
        {
            foreach (TutorialTask task in _allTasks)
            {
                task.TaskStarted -= OnTaskStarted;
                task.TaskCompleted -= OnTaskCompleted;
            }
            
            _allTasks.Clear();
        }
    }
}