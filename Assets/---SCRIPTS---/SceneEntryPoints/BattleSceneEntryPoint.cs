using UnityEngine;
using Yg.Battle;
using Yg.Battle.GameSystems;
using Yg.GameData;
using Yg.UI;
using Zenject;

namespace Yg.EntryPoint
{
    public class BattleSceneEntryPoint : MonoBehaviour
    {
        private PersistentData _persistentData;
        private BattleUnitSpawner _battleUnitSpawner;
        private BattleSquadPlacer _battleSquadPlacer;
        private SquadPlacementUI _squadPlacementUI;

        [Inject]
        private void Construct(
            PersistentData persistentData,
            BattleUnitSpawner battleUnitSpawner,
            BattleSquadPlacer enemySquadPlacer,
            SquadPlacementUI squadPlacementUI)
        {
            _battleUnitSpawner = battleUnitSpawner;
            _persistentData = persistentData;
            _battleSquadPlacer = enemySquadPlacer;
            _squadPlacementUI = squadPlacementUI;
        }

        private void Awake()
        {
            InitializeScene();
        }

        private void InitializeScene()
        {
            _battleUnitSpawner.Initialize(_persistentData);
            _battleSquadPlacer.Initialize();
            _squadPlacementUI.Initialize();
        }
    }
}
