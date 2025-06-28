using UnityEngine;
using Yg.Battle;

namespace Yg.GameData.Units
{
    [CreateAssetMenu(fileName = "RangedUnitData", menuName = "Data/RangedUnitData")]
    public class RangedUnitDataSO : UnitDataSO
    {
        [field: Space(10f)]

        [field: SerializeField] public Projectile ProjectilePrefab { get; private set; }
        [field: SerializeField] public float ProjectileSpeed { get; private set; }
    }
}
