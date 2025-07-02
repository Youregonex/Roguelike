using System.Collections.Generic;
using Yg.Character;
using Yg.GameData.Units;

namespace Yg.Systems
{
    public class UnitSelectionGenerator
    {
        public List<WarbandSlot> GenerateRandomUnitChoiceList(int choicesAmount)
        {
            List<UnitDataSO> unitDataSOList = ResourceLoader.SO_UnitDataSOList;
            List<WarbandSlot> choices = new();

            int randomUnitIndex;

            for (int i = 0; i < choicesAmount; i++)
            {
                randomUnitIndex = UnityEngine.Random.Range(0, unitDataSOList.Count);
                UnitDataSO unitDataSO = unitDataSOList[randomUnitIndex];

                WarbandSlot warbandSlot = new(unitDataSO, unitDataSO.DefaultSquadSize);
                choices.Add(warbandSlot);
            }

            return choices;
        }
    }
}
