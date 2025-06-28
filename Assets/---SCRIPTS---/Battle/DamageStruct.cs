using UnityEngine;

namespace Yg.Battle
{
    public struct DamageStruct
    {
        public EUnitFaction UnitFaction;
        public Vector2 Origin;
        public EAttackType AttackType;
        public EDamageType DamageType;
        public float DamageAmount;
        public float KnockBackForce;

        public DamageStruct(
            EUnitFaction unitFaction,
            Vector2 origin,
            EAttackType attackType,
            EDamageType damageType,
            float damageAmount,
            float knockBackForce)
        {
            UnitFaction = unitFaction;
            Origin = origin;
            AttackType = attackType;
            DamageType = damageType;
            DamageAmount = damageAmount;
            KnockBackForce = knockBackForce;
        }
    }
}
