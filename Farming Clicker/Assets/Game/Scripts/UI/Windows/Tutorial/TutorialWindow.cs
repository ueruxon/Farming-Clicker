using Game.Scripts.Logic.Tutorials;
using Game.Scripts.Logic.Tutorials.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Scripts.UI.Windows.Tutorial
{
    public class TutorialWindow : MonoBehaviour
    {
        [SerializeField] private TMP_Text _tutorialText;
        [SerializeField] private GameObject _completeContent;
        [SerializeField] private Button _closeButton;
        
        private TutorialController _tutorialController;

        [Inject]
        public void Construct(TutorialController tutorialController)
        {
            _tutorialController = tutorialController;
            _tutorialController.TaskStarted += OnNewTaskStarted;
            _tutorialController.TutorialCompleted += OnTutorialCompleted;

            _closeButton.onClick.AddListener(() => Destroy(gameObject));
            _completeContent.gameObject.SetActive(false);
        }

        private void OnNewTaskStarted(TutorialTaskData taskData)
        {
            _tutorialText.SetText(taskData.Description);
        }

        private void OnTutorialCompleted()
        {
            _tutorialText.gameObject.SetActive(false);
            _completeContent.gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            _tutorialController.TaskStarted -= OnNewTaskStarted;
            _tutorialController.TutorialCompleted -= OnTutorialCompleted;
        }
    }
}