using UnityEngine;
using Yg.GameData;
using Yg.UI.MainMenu;
using Zenject;

namespace Yg.EntryPoint
{
    public class MainMenuSceneEntryPoint : MonoBehaviour
    {
        private PersistentData _persistentData;
        private MainMenuUI _mainMenu;

        [Inject]
        private void Construct(PersistentData persistentData, MainMenuUI mainMenu)
        {
            _persistentData = persistentData;
            _mainMenu = mainMenu;
        }

        private void Awake()
        {
            InitializeScene();
        }

        private void InitializeScene()
        {
            InitializeMainMenu(_persistentData.SaveFileExists());
        }

        private void InitializeMainMenu(bool saveFileExists)
        {
            _mainMenu.Initialize(saveFileExists);
        }
    }
}
