using System.Collections.Generic;
using UnityEngine;
using Yg.Battle;
using Yg.Battle.BattleUnits;
using Yg.GameData.Perks;

namespace Yg.GameData.Units
{
    [CreateAssetMenu(fileName = "UnitData", menuName = "Data/UnitData")]
    public class UnitDataSO : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public BattleUnitCore Prefab { get; private set; }
        [field: SerializeField] public string PrefabId { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }

        [field: Space(10f)]
        [field: SerializeField] public int DefaultSquadSize { get; private set; }
        [field: SerializeField] public List<StatData> UnitStatDataList { get; private set; }
        [field: SerializeField] public EAttackType AttackType { get; private set; }
        [field: SerializeField] public EDamageType DamageType { get; private set; }

        [field: Space(10f)]

        [field: SerializeField] public List<PerkSO> PerkSOList { get; private set; }
        [field: SerializeField] public List<SpellSO> SpellSOList { get; private set; }

        private void OnValidate()
        {
            if (Prefab is not null)
                PrefabId = Prefab.GetComponent<UniqueId>().Id;
            else
                PrefabId = string.Empty;
        }
    }
}
