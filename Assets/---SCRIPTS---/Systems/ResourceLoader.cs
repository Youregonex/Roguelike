using UnityEngine;
using Yg.GameData.Configs;
using Yg.Character;
using System.Collections.Generic;
using System.Linq;
using Yg.GameData.Units;
using Yg.GameData.Perks;
using Yg.GameData.Equipment;

public static class ResourceLoader
{
    // Configs
    private static readonly string CONFIG_MAP_GENERATION_PATH = "Configs/MapGeneration/MainMapGenerationConfig";
    private static readonly string CONFIG_NOISE_TO_TILE_TYPE_PATH = "Configs/Tiles/MainNoiseToTileTypeConfig";
    private static readonly string CONFIG_TYPE_TO_TILE_PATH = "Configs/Tiles/MainTileTypeToTileConfig";
    private static readonly string CONFIG_CASTLE_POINT_OF_INTEREST_PATH = "Configs/PointsOfInterest/CastleConfig";
    private static readonly string CONFIG_RESOURCE_POINT_OF_INTEREST_PATH = "Configs/PointsOfInterest/ResourceConfig";
    private static readonly string CONFIG_VILLAGE_POINT_OF_INTEREST_PATH = "Configs/PointsOfInterest/VillageConfig";
    private static readonly string CONFIG_TOWN_POINT_OF_INTEREST_PATH = "Configs/PointsOfInterest/TownConfig";

    private static readonly string CONFIG_EQUIPMENT_RARITY_TIER_STATS_PATH = "Configs/Equipment/EquipmentRarityTierStatsConfig";
    private static readonly string CONFIG_EQUIPMENT_RARITY_WEIGHT_PATH = "Configs/Equipment/EquipmentRarityWeightConfig";
    private static readonly string CONFIG_EQUIPMENT_TEMPlATE_PATH = "Configs/Equipment/EquipmentTemplateConfig";

    // Prefabs
    private static readonly string PREFAB_PLAYER_CHARACTER_PATH = "Prefabs/Agents/PlayerCharacter";
    private static readonly string PREFAB_UNITS_PATH = "ScriptableObjects/Units";

    private static readonly string PERKS_PATH = "ScriptableObjects/Perks";

    // Configs
    public static DefaultMapGenerationConfigSO CONFIG_MapGeneration;
    public static NoiseToTileTypeConfigSO CONFIG_NoiseToTileType;
    public static TileTypeToTileConfigSO CONFIG_TileTypeToTile;
    public static CastlePointOfInterestConfigSO CONFIG_CastlePointOfInterest;
    public static ResourcePointOfInterestConfigSO CONFIG_ResourcePointOfInterest;
    public static VillagePointOfInterestConfigSO CONFIG_VillagePointOfInterest;
    public static TownPointOfInterestConfigSO CONFIG_TownPointOfInterest;

    public static EquipmentRarityTierStatsConfig CONFIG_EquipmentRarityTierStatsConfig;
    public static EquipmentRarityWeightsConfig CONFIG_EquipmentRarityWeightConfig;
    public static EquipmentTemplateConfig CONFIG_EquipmentTemplateConfig;

    // Prefabs
    public static PlayerCore PREFAB_PlayerCharacter;

    public static List<UnitDataSO> SO_UnitDataSOList;
    public static List<PerkSO> SO_PerkSOList;

    private static Dictionary<string, Sprite> _iconDictionary = new();

    static ResourceLoader()
    {
        LoadConfigs();
        LoadPrefabs();
        LoadScriptableObjects();
    }

    public static void Clear()
    {
        //SO_UnitDataSOList?.Clear();
        //SO_UnitDataSOList = null;

        //CONFIG_MapGeneration = null;
        //CONFIG_NoiseToTileType = null;
        //CONFIG_TileTypeToTile = null;
        //CONFIG_CastlePointOfInterest = null;
        //CONFIG_ResourcePointOfInterest = null;
        //CONFIG_VillagePointOfInterest = null;
        //CONFIG_TownPointOfInterest = null;

        //PREFAB_PlayerCharacter = null;
    }

    public static Sprite GetIconWithPath(string iconPath)
    {
        if (_iconDictionary.ContainsKey(iconPath))
            return _iconDictionary[iconPath];

        Sprite icon = Resources.Load<Sprite>(iconPath);

        if (icon == null)
        {
            Debug.LogError($"Couldn't find icon {iconPath}");
            return null;
        }

        _iconDictionary.Add(iconPath, icon);
        return icon;
    }

    public static UnitDataSO GetUnitDataSO(string prefabId)
    {
        return SO_UnitDataSOList.Where(e => e.PrefabId == prefabId).FirstOrDefault();
    }

    public static PerkSO GetPerkSO(string perkId)
    {
        PerkSO perkSO = SO_PerkSOList.Where(e => e.PerkId== perkId).FirstOrDefault();

        if (perkSO == null) Debug.LogError($"Couldn't find perk for id: {perkId}");
        return perkSO;
    }

    private static void LoadConfigs()
    {
        CONFIG_MapGeneration = Resources.Load<DefaultMapGenerationConfigSO>(CONFIG_MAP_GENERATION_PATH);
        CONFIG_NoiseToTileType = Resources.Load<NoiseToTileTypeConfigSO>(CONFIG_NOISE_TO_TILE_TYPE_PATH);
        CONFIG_TileTypeToTile = Resources.Load<TileTypeToTileConfigSO>(CONFIG_TYPE_TO_TILE_PATH);
        CONFIG_CastlePointOfInterest = Resources.Load<CastlePointOfInterestConfigSO>(CONFIG_CASTLE_POINT_OF_INTEREST_PATH);
        CONFIG_ResourcePointOfInterest = Resources.Load<ResourcePointOfInterestConfigSO>(CONFIG_RESOURCE_POINT_OF_INTEREST_PATH);
        CONFIG_VillagePointOfInterest = Resources.Load<VillagePointOfInterestConfigSO>(CONFIG_VILLAGE_POINT_OF_INTEREST_PATH);
        CONFIG_TownPointOfInterest = Resources.Load<TownPointOfInterestConfigSO>(CONFIG_TOWN_POINT_OF_INTEREST_PATH);

        CONFIG_EquipmentRarityTierStatsConfig = Resources.Load<EquipmentRarityTierStatsConfig>(CONFIG_EQUIPMENT_RARITY_TIER_STATS_PATH);
        CONFIG_EquipmentRarityWeightConfig = Resources.Load<EquipmentRarityWeightsConfig>(CONFIG_EQUIPMENT_RARITY_WEIGHT_PATH);
        CONFIG_EquipmentTemplateConfig = Resources.Load<EquipmentTemplateConfig>(CONFIG_EQUIPMENT_TEMPlATE_PATH);
    }

    private static void LoadPrefabs()
    {
        PREFAB_PlayerCharacter = Resources.Load<PlayerCore>(PREFAB_PLAYER_CHARACTER_PATH);
    }

    private static void LoadScriptableObjects()
    {
        SO_UnitDataSOList = new();
        SO_UnitDataSOList = Resources.LoadAll<UnitDataSO>(PREFAB_UNITS_PATH).ToList();
        SO_PerkSOList = new();
        SO_PerkSOList = Resources.LoadAll<PerkSO>(PERKS_PATH).ToList();
    }
}
