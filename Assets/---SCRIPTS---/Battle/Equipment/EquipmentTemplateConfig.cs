using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yg.Battle;
using Yg.GameData.Perks;

namespace Yg.GameData.Equipment
{
    [CreateAssetMenu(fileName = "EquipmentTemplateConfig", menuName = "Configs/Equipment/EquipmentTemplateConfig")]
    public class EquipmentTemplateConfig : ScriptableObject
    {
        [field: SerializeField] public List<EquipmentTemplate> EquipmentTemplateList { get; private set; }

        public PerkSO GetPerkSO(int tier, string perkId)
        {
            List<PerkSO> perkPool = EquipmentTemplateList.Where(e => e.Tier == tier).FirstOrDefault().PerkPool;
            var perk = perkPool.Where(e => e.PerkId == perkId).FirstOrDefault();

            if (perk == null) Debug.LogWarning($"Couldn't find {perkId} of {tier} tier in template!");
            return perk;
        }
    }

    [System.Serializable]
    public class EquipmentTemplate
    {
        [field: SerializeField] public int Tier { get; private set; }
        [field: SerializeField] public List<PerkSO> PerkPool { get; private set; }
        [field: SerializeField] public List<SpellSO> SpellPool { get; private set; }
    }
}
