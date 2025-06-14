using UnityEngine;
using Yg.SceneTransitions;
using Yg.UI.MainMenu;
using Zenject;

namespace Yg.ZenjectInstallers
{
    public class MainMenuSceneInstaller : MonoInstaller
    {
        [CustomHeader("Settings")]
        [SerializeField] private MainMenuUI _mainMenuUI;
        [SerializeField] private SceneTransitioner _sceneTransitioner;

        public override void InstallBindings()
        {
            InstallMainMenu();
            InstallSceneTransitioner();
        }

        private void InstallMainMenu()
        {
            Container.Bind<MainMenuUI>().FromInstance(_mainMenuUI).AsSingle();
        }

        private void InstallSceneTransitioner()
        {
            Container.Bind<SceneTransitioner>().FromInstance(_sceneTransitioner).AsSingle();
        }
    }
}
