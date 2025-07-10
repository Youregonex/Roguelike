using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using System.Linq;
using Yg.Battle.BattleUnits;
using Yg.GameData.Perks;

namespace Yg.GameData.Equipment
{
    public class EquipmentBuilder
    {
        private const float MAIN_STAT_MODIFIER = .5f;

        public EquipmentRarityTierStatsConfig _equipmentRarityTierStatsConfig;
        public EquipmentRarityWeightsConfig _equipmentRarityWeightConfig;
        public EquipmentTemplateConfig _equipmentTemplateConfig;

        private EquipmentNamingRules _namingRules;

        public EquipmentBuilder()
        {
            LoadNamingRules();

            _equipmentRarityTierStatsConfig = ResourceLoader.CONFIG_EquipmentRarityTierStatsConfig;
            _equipmentRarityWeightConfig = ResourceLoader.CONFIG_EquipmentRarityWeightConfig;
            _equipmentTemplateConfig = ResourceLoader.CONFIG_EquipmentTemplateConfig;
        }

        public EquipmentData BuildEquipment(int tier)
        {
            EquipmentData equipmentData = new();
            equipmentData.Rarity = RollRarity();
            equipmentData.Tier = tier;
            equipmentData.StatModifierList = GenerateStatModifiers(tier, equipmentData.Rarity);
            equipmentData.PerkIdList = GeneratePerks(tier, equipmentData.Rarity);
            equipmentData.Name = BuildName(equipmentData, tier);

            LogCreatedItem(equipmentData, tier);

            return equipmentData;
        }

        private void LogCreatedItem(EquipmentData equipmentData, int tier)
        {
            string log = $"<b>▶ Generated {equipmentData.Rarity} T{tier} Equipment</b>\n";
            log += $"<b>Name:</b> {equipmentData.Name}\n";
            log += $"<b>Rarity:</b> {equipmentData.Rarity}\n";
            log += $"<b>Tier:</b> {tier}\n";
            log += $"<b>Icon:</b> {equipmentData.IconPath}\n";

            log += $"\n<b>🔧 Stat Modifiers:</b>\n";
            foreach (var statMod in equipmentData.StatModifierList)
            {
                string valueText = $"{statMod.Value * 100}%";
                log += $"• {statMod.StatType} → {statMod.StatModificationType} {valueText: F1}\n";
            }

            log += $"\n<b>✨ Perks:</b>\n";
            if (equipmentData.PerkIdList.Count == 0)
            {
                log += "• None\n";
            }
            else
            {
                foreach (var perkId in equipmentData.PerkIdList)
                    log += $"• {_equipmentTemplateConfig.GetPerkSO(tier, perkId).Name}\n";
            }

            Debug.Log(log);
        }

        private string BuildName(EquipmentData equipmentData, int tier)
        {
            string rarity;
            string prefix;
            string item;
            string suffix;
            string creationPhrase;
            string statTitle;

            int randomIndex;

            // Rarity
            rarity = _namingRules.RarityNames[equipmentData.Rarity];

            // Prefix
            if(equipmentData.PerkIdList.Count == 0)
            {
                var prefixTagEntry = _namingRules.Prefixes.ElementAt(UnityEngine.Random.Range(0, _namingRules.Prefixes.Count));
                prefix = prefixTagEntry.Value[UnityEngine.Random.Range(0, prefixTagEntry.Value.Count)];
            }
            else
            {
                randomIndex = UnityEngine.Random.Range(0, equipmentData.PerkIdList.Count);
                string randomId = equipmentData.PerkIdList[randomIndex];
                PerkSO perkSO = _equipmentTemplateConfig.GetPerkSO(tier, randomId);
                randomIndex = UnityEngine.Random.Range(0, perkSO.Tags.Count);
                ETag tag = perkSO.Tags[randomIndex];
                randomIndex = UnityEngine.Random.Range(0, _namingRules.Prefixes[tag].Count);
                prefix = _namingRules.Prefixes[tag][randomIndex];
            }

            // Item
            randomIndex = UnityEngine.Random.Range(0, _namingRules.Items.Count);
            item = _namingRules.Items[randomIndex].Name;
            equipmentData.IconPath = _namingRules.Items[randomIndex].IconPath;

            // Suffix
            var suffixTagEntry = _namingRules.Suffixes.ElementAt(UnityEngine.Random.Range(0, _namingRules.Suffixes.Count));
            suffix = suffixTagEntry.Value[UnityEngine.Random.Range(0, suffixTagEntry.Value.Count)];

            // Creation Phrase
            creationPhrase = _namingRules.CreationPhrases[UnityEngine.Random.Range(0, _namingRules.CreationPhrases.Count)];

            // Stat Title (pick random stat key first)
            EStat mainStat = equipmentData.StatModifierList[0].StatType;
            randomIndex = UnityEngine.Random.Range(0, _namingRules.StatTitles[mainStat].Count);
            statTitle = _namingRules.StatTitles[mainStat][randomIndex];

            // Final name
            return $"{prefix} {item} {suffix} {creationPhrase} {statTitle}";
        }

        private void LoadNamingRules()
        {
            TextAsset jsonAsset = Resources.Load<TextAsset>("Configs/Equipment/EquipmentNamingRules");
            if (jsonAsset == null)
            {
                Debug.LogError("EquipmentNamingRules.json not found!");
                return;
            }

            var settings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                Error = (sender, args) =>
                {
                    Debug.LogError($"JSON Error: {args.ErrorContext.Error.Message}");
                    args.ErrorContext.Handled = true;
                }
            };

            _namingRules = JsonConvert.DeserializeObject<EquipmentNamingRules>(jsonAsset.text, settings);
        }

        private EEquipmentRarity RollRarity()
        {
            float totalWeight = 0;

            foreach (var entry in _equipmentRarityWeightConfig.RarityWeightList)
                totalWeight += entry.Weight;

            float roll = UnityEngine.Random.Range(0f, totalWeight);
            float current = 0;

            foreach (var entry in _equipmentRarityWeightConfig.RarityWeightList)
            {
                current += entry.Weight;
                if (roll < current)
                    return entry.Rarity;
            }

            Debug.LogWarning("Rarity roll failed, defaulting to Forgotten.");
            return EEquipmentRarity.Forgotten;
        }

        private List<StatModifier> GenerateStatModifiers(int tier, EEquipmentRarity rarity)
        {
            List<StatModifier> statModifierList = new();
            List<EStat> availableStats = ((EStat[])System.Enum.GetValues(typeof(EStat))).ToList();

            EquipmentRarityTierStats rarityTierStats = GetEquipmentRarityTierStats(tier, rarity);

            int totalStatModifiersAmount = UnityEngine.Random.Range(rarityTierStats.AmountOfStatsMin, rarityTierStats.AmountOfStatsMax);
            int randomIndex;
            EStat stat;
            bool main;

            for (int i = 0; i < totalStatModifiersAmount && availableStats.Count > 0; i++)
            {
                main = i == 0;

                randomIndex = UnityEngine.Random.Range(0, availableStats.Count);
                stat = availableStats[randomIndex];
                availableStats.RemoveAt(randomIndex);

                StatModifier statModifier = CreateStatModifier(stat, rarityTierStats, main);
                statModifierList.Add(statModifier);
            }


            return statModifierList;
        }

        private StatModifier CreateStatModifier(EStat statType, EquipmentRarityTierStats erc, bool main)
        {
            StatModifier statModifier = new();
            statModifier.StatModificationType = EStatModification.Add;
            statModifier.StatType = statType;
            statModifier.Value = UnityEngine.Random.Range(erc.StatValueChangeMin, erc.StatValueChangeMax);

            if (main) statModifier.Value *= 1 + MAIN_STAT_MODIFIER;

            return statModifier;
        }

        private List<string> GeneratePerks(int tier, EEquipmentRarity rarity)
        {
            List<string> perkIdList = new();
            EquipmentRarityTierStats rarityTierStats = GetEquipmentRarityTierStats(tier, rarity);
            int totalPerks = UnityEngine.Random.Range(rarityTierStats.PerksAmountMin, rarityTierStats.PerksAmountMax);

            if (totalPerks == 0) return perkIdList;
            var template = _equipmentTemplateConfig.EquipmentTemplateList.Where(e => e.Tier == tier).FirstOrDefault();
            List<PerkSO> availablePerks = new(template.PerkPool);

            int randomIndex;
            for (int i = 0; i < totalPerks && availablePerks.Count > 0; i++)
            {
                randomIndex = UnityEngine.Random.Range(0, availablePerks.Count);
                perkIdList.Add(availablePerks[randomIndex].PerkId);
                availablePerks.RemoveAt(randomIndex);
            }

            return perkIdList;
        }

        private EquipmentRarityTierStats GetEquipmentRarityTierStats(int tier, EEquipmentRarity rarity)
        {
            var result = _equipmentRarityTierStatsConfig.EquipmentRarityTierStatsList
                .Where(e => (e.Tier == tier) && (e.EquipmentRarity == rarity))
                .FirstOrDefault();

            if(result == null) Debug.LogError($"Missing EquipmentRarityTierStats for Tier {tier} and Rarity {rarity}");

            return result;
        }

        [Serializable]
        public class EquipmentItemName
        {
            public string Name;
            public string IconPath;
        }

        [Serializable]
        public class EquipmentNamingRules
        {
            public Dictionary<EEquipmentRarity, string> RarityNames;
            public Dictionary<ETag, List<string>> Prefixes;
            public List<EquipmentItemName> Items;
            public Dictionary<ETag, List<string>> Suffixes;
            public List<string> CreationPhrases;
            public Dictionary<EStat, List<string>> StatTitles;
        }
    }
}