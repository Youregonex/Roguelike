using Yg.GameData;
using Zenject;

public class ProjectContextInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindPersistentData();
    }

    private void BindPersistentData()
    {
        Container.Bind<PersistentData>().AsSingle();
    }
}
