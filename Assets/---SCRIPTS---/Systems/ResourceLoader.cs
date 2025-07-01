using UnityEngine;
using Yg.GameData.Configs;
using Yg.Character;
using System.Collections.Generic;
using System.Linq;
using Yg.GameData.Units;

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

    // Prefabs
    private static readonly string PREFAB_PLAYER_CHARACTER_PATH = "Prefabs/Agents/PlayerCharacter";
    private static readonly string PREFAB_UNITS_PATH = "ScriptableObjects/Units";


    // Configs
    public static DefaultMapGenerationConfigSO CONFIG_MapGeneration;
    public static NoiseToTileTypeConfigSO CONFIG_NoiseToTileType;
    public static TileTypeToTileConfigSO CONFIG_TileTypeToTile;
    public static CastlePointOfInterestConfigSO CONFIG_CastlePointOfInterest;
    public static ResourcePointOfInterestConfigSO CONFIG_ResourcePointOfInterest;
    public static VillagePointOfInterestConfigSO CONFIG_VillagePointOfInterest;
    public static TownPointOfInterestConfigSO CONFIG_TownPointOfInterest;

    // Prefabs
    public static PlayerCore PREFAB_PlayerCharacter;

    public static List<UnitDataSO> SO_UnitDataSOList;


    static ResourceLoader()
    {
        LoadConfigs();
        LoadPrefabs();
    }

    public static UnitDataSO GetUnitDataSO(string prefabId)
    {
        return SO_UnitDataSOList.Where(e => e.PrefabId == prefabId).FirstOrDefault();
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
    }

    private static void LoadPrefabs()
    {
        PREFAB_PlayerCharacter = Resources.Load<PlayerCore>(PREFAB_PLAYER_CHARACTER_PATH);

        SO_UnitDataSOList = new();
        SO_UnitDataSOList = Resources.LoadAll<UnitDataSO>(PREFAB_UNITS_PATH).ToList();
    }
}
