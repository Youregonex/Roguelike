using UnityEngine;
using Yg.Battle;
using Yg.Battle.BattleUnits;

namespace Yg.GameData.Perks
{
    public abstract class Perk : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public EPerkApplicationEvent PerkApplicationEvent { get; protected set; }
        [field: SerializeField, TextArea(3,10)] public string PerkDescription { get; protected set; }
        [field: SerializeField] public Sprite PerkIcon { get; protected set; }

        public abstract void ApplyPerk(ref DamageStruct damageStruct);
    }
}
