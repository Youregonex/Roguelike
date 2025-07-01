using UnityEngine;
using Yg.Battle;
using Yg.Battle.GameSystems;
using Yg.GameData;
using Zenject;

namespace Yg.EntryPoint
{
    public class BattleSceneEntryPoint : MonoBehaviour
    {
        private PersistentData _persistentData;
        private BattleUnitSpawner _battleUnitSpawner;
        private EnemySquadPlacer _enemySquadPlacer;

        [Inject]
        private void Construct(
            PersistentData persistentData,
            BattleUnitSpawner battleUnitSpawner,
            EnemySquadPlacer enemySquadPlacer)
        {
            _battleUnitSpawner = battleUnitSpawner;
            _persistentData = persistentData;
            _enemySquadPlacer = enemySquadPlacer;
        }

        private void Awake()
        {
            InitializeScene();
        }

        private void InitializeScene()
        {
            _battleUnitSpawner.Initialize(_persistentData);
            _enemySquadPlacer.Initialize();
        }
    }
}
