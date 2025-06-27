using UnityEngine;

namespace Yg.Battle
{
    public struct DamageStruct
    {
        public EUnitFaction UnitFaction;
        public Vector2 Origin;
        public float DamageAmount;
        public float KnockBackForce;

        public DamageStruct(EUnitFaction unitFaction, Vector2 origin, float damageAmount, float knockBackForce)
        {
            UnitFaction = unitFaction;
            Origin = origin;
            DamageAmount = damageAmount;
            KnockBackForce = knockBackForce;
        }
    }
}
