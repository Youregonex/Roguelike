using UnityEngine;
using Yg.Battle.BattleUnits;

namespace Yg.Battle
{
    public struct DamageStruct
    {
        public EUnitFaction UnitFaction;
        public BattleUnitCore Sender;
        public Vector2 OriginPosition;
        public EAttackType AttackType;
        public EDamageType DamageType;
        public float DamageAmount;
        public float KnockBackForce;

        public DamageStruct(
            EUnitFaction unitFaction,
            BattleUnitCore origin,
            Vector2 originPosition,
            EAttackType attackType,
            EDamageType damageType,
            float damageAmount,
            float knockBackForce)
        {
            UnitFaction = unitFaction;
            Sender = origin;
            OriginPosition = originPosition;
            AttackType = attackType;
            DamageType = damageType;
            DamageAmount = damageAmount;
            KnockBackForce = knockBackForce;
        }
    }
}
