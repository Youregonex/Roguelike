using UnityEngine;
using UnityEngine.UI;
using Yg.GameData;
using Yg.SceneTransitions;
using Zenject;

namespace Yg.UI.MainMenu
{
    public class MainMenuUI : MonoBehaviour
    {
        [CustomHeader("Settings")]
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _continueGameButton;
        [SerializeField] private Button _exitButton;

        private PersistentData _persistentData;
        private SceneTransitioner _sceneTransitioner;

        [Inject]
        private void Construct(PersistentData persistentData, SceneTransitioner sceneTransitioner)
        {
            _persistentData = persistentData;
            _sceneTransitioner = sceneTransitioner;
        }

        public void Initialize(bool saveFileExists)
        {
            SetupButtons();
            _continueGameButton.interactable = saveFileExists;
        }

        private void SetupButtons()
        {
            _newGameButton.onClick.AddListener(() =>
            {
                _persistentData.StartNewGame();
                _sceneTransitioner.StartTransition();
            });

            _continueGameButton.onClick.AddListener(() =>
            {
                _sceneTransitioner.StartTransition();
            });

            _exitButton.onClick.AddListener(() =>
            {
                Application.Quit();
            });
        }
    }
}
