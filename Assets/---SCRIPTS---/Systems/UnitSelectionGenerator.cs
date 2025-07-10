using System.Collections.Generic;
using Yg.GameData.Units;

namespace Yg.Systems
{
    public class UnitSelectionGenerator
    {
        public List<UnitDataSO> GenerateRandomUnitChoiceList(int choicesAmount)
        {
            List<UnitDataSO> unitDataSOList = ResourceLoader.SO_UnitDataSOList;
            List<UnitDataSO> choices = new();

            int randomUnitIndex;

            for (int i = 0; i < choicesAmount; i++)
            {
                randomUnitIndex = UnityEngine.Random.Range(0, unitDataSOList.Count);
                UnitDataSO unitDataSO = unitDataSOList[randomUnitIndex];
                choices.Add(unitDataSO);
            }

            return choices;
        }
    }
}
