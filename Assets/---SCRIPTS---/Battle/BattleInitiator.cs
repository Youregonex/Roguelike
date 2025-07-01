using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yg.Character;
using Yg.GameData;
using Zenject;

namespace Yg.Systems
{
    public class BattleInitiator : MonoBehaviour
    {
        private const string BATTLE_SCENE_NAME = "BattleScene";

        private PersistentData _persistentData;

        [Inject]
        private void Construct(PersistentData persistentData)
        {
            _persistentData = persistentData;
        }

        public void StartBattle(List<WarbandSlot> playerWarband, List<WarbandSlot> enemyWarband)
        {
            BattleTransitionData battleTransitionData = new(playerWarband, enemyWarband);
            _persistentData.SetBattleTransitionData(battleTransitionData);
            _persistentData.SaveData();
            SceneManager.LoadScene(BATTLE_SCENE_NAME);
        }
    }
}
