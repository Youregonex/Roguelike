using Yg.GameData;
using Yg.Systems;
using Zenject;

namespace Yg.ZenjectInstallers
{
    public class ProjectContextInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PersistentData>().AsSingle();
            Container.Bind<ColorPicker>().AsSingle();
        }
    }
}
