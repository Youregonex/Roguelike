using UnityEngine;
using Yg.Battle.BattleUnits;

namespace Yg.GameData
{
    [System.Serializable]
    public class StatModifier
    {
        public EStat StatType;
        public EStatModification StatModificationType;
        public float Value;

        public void ModifyStat(BattleUnitCore target)
        {
            if (!target.TryGetUnitComponent(out BattleUnitStatsComponent statComponent))
                return;

            switch (StatModificationType)
            {
                case EStatModification.Add:

                    statComponent.IncreaseMaxStatValue(StatType, Value, true);
                    statComponent.IncreaseCurrentStatValue(StatType, Value, true);

                    break;

                case EStatModification.Substract:

                    statComponent.DecreaseMaxStatValue(StatType, Value, true);
                    statComponent.DecreaseCurrentStatValue(StatType, Value, true);

                    break;

                default:
                    Debug.LogError($"Couldn't modify stat {StatType}");
                    break;
            }
        }
    }

    public enum EStatModification
    {
        Add,
        Substract,
    }
}
