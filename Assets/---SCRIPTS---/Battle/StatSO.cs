using UnityEngine;

namespace Yg.Battle.BattleUnits
{
    [System.Serializable]
    public class StatData
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public EStat StatType { get; private set; }
        [field: SerializeField] public float MaxValue { get; private set; }
        [field: SerializeField] public bool IgnoreMaxValue { get; private set; }
    }
}
