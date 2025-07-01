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
        [field: SerializeField] public string UnitName { get; private set; }
        [field: SerializeField] public BattleUnitCore UnitPrefab { get; private set; }
        [field: SerializeField] public string PrefabId { get; private set; }
        [field: SerializeField] public Sprite UnitIcon { get; private set; }
        [field: SerializeField] public int DefaultSquadSize { get; private set; }

        [field: Space(10f)]

        [field: SerializeField] public float MoveSpeed { get; private set; }
        [field: SerializeField] public float MaxHealth { get; private set; }
        [field: SerializeField] public EAttackType AttackType { get; private set; }
        [field: SerializeField] public EDamageType DamageType { get; private set; }
        [field: SerializeField] public float AttackRange { get; private set; }
        [field: SerializeField] public float AttackCooldownMin { get; private set; }
        [field: SerializeField] public float AttackCooldownMax { get; private set; }
        [field: SerializeField] public float AttackDamageMin { get; private set; }
        [field: SerializeField] public float AttackDamageMax { get; private set; }
        [field: SerializeField] public float KnockBackForce { get; private set; }

        [field: Space(10f)]

        [field: SerializeField] public List<Perk> PerkList { get; private set; }
        [field: SerializeField] public List<SpellSO> SpellSOList { get; private set; }

        private void OnValidate()
        {
            if (UnitPrefab is not null)
                PrefabId = UnitPrefab.GetComponent<UniqueId>().Id;
            else
                PrefabId = string.Empty;
        }
    }
}
