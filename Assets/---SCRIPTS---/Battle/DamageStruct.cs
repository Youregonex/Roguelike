using UnityEngine;

namespace Yg.Battle
{
    public struct DamageStruct
    {
        public Transform Origin;
        public float DamageAmount;

        public DamageStruct(Transform origin, float damageAmount)
        {
            Origin = origin;
            DamageAmount = damageAmount;
        }
    }
}
