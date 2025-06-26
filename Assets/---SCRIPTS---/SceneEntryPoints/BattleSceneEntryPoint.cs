using UnityEngine;
using Yg.Battle.GameSystems;
using Zenject;

namespace Yg.EntryPoint
{
    public class BattleSceneEntryPoint : MonoBehaviour
    {
        private BattleUnitSpawner _battleUnitSpawner;

        [Inject]
        private void Construct(BattleUnitSpawner battleUnitSpawner)
        {
            _battleUnitSpawner = battleUnitSpawner;
        }

        private void Awake()
        {
            InitializeScene();
        }

        private void InitializeScene()
        {
            _battleUnitSpawner.Initialize();
        }
    }
}
