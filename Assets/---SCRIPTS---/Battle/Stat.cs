
namespace Yg.Battle.BattleUnits
{
    public class Stat
    {
        public EStat StatType { get; private set; }
        public float MaxValue { get; private set; }
        public bool IgnoreMaxValue { get; private set; }
        public float CurrentValue { get; private set; }

        public Stat(EStat statType, float maxValue, bool ignoreMaxValue)
        {
            StatType = statType;
            MaxValue = maxValue;
            IgnoreMaxValue = ignoreMaxValue;
            CurrentValue = MaxValue;
        }

        public void IncreaseMaxValue(float amount, bool percentage)
        {
            if (amount < 0) return;

            if (percentage) MaxValue += MaxValue * amount;
            else MaxValue += amount;
        }

        public void DecreaseMaxValue(float amount, bool percentage)
        {
            if (amount < 0) return;

            if (percentage) MaxValue -= MaxValue * amount;
            else MaxValue -= amount;

            if (MaxValue < 0) MaxValue = 0;
        }

        public void IncreaseCurrentValue(float amount, bool percentage)
        {
            if (amount < 0) return;

            if (percentage) CurrentValue += CurrentValue * amount;
            else CurrentValue += amount;

            if (CurrentValue > MaxValue && IgnoreMaxValue) CurrentValue = MaxValue;
        }

        public void DecreaseCurrentValue(float amount, bool percentage)
        {
            if (amount < 0) return;

            if (percentage) CurrentValue -= CurrentValue * amount;
            else CurrentValue -= amount;

            if (CurrentValue < 0) CurrentValue = 0;
        }
    }
}