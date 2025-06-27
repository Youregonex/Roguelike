using UnityEngine;

namespace Yg.Battle
{
    public struct DamageStruct
    {
        public Transform Origin;
        public float DamageAmount;
        public float KnockBackForce;

        public DamageStruct(Transform origin, float damageAmount, float knockBackForce)
        {
            Origin = origin;
            DamageAmount = damageAmount;
            KnockBackForce = knockBackForce;
        }
    }
}
