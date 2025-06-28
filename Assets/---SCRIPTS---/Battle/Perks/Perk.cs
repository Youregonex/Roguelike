using UnityEngine;
using Yg.Battle;
using Yg.Battle.BattleUnits;

namespace Yg.GameData.Perks
{
    public abstract class Perk : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public EPerkApplicationEvent PerkApplicationEvent { get; protected set; }

        public abstract void ApplyPerk(ref DamageStruct damageStruct);
    }
}
