using Yg.Battle.BattleUnits;

namespace Yg.Battle
{
    public struct DamageStruct
    {
        public EUnitFaction UnitFaction;
        public BattleUnitCore Origin;
        public EAttackType AttackType;
        public EDamageType DamageType;
        public float DamageAmount;
        public float KnockBackForce;

        public DamageStruct(
            EUnitFaction unitFaction,
            BattleUnitCore origin,
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
