using UnityEngine;
using Yg.Battle.GameSystems;
using Zenject;

namespace Yg.ZenjectInstallers
{
    public class BattleSceneInstaller : MonoInstaller
    {
        [CustomHeader("Settings")]
        [SerializeField] private BattleUnitSpawner _unitSpawner;

        public override void InstallBindings()
        {
            Container.Bind<BattleUnitSpawner>().FromInstance(_unitSpawner);
        }
    }
}
