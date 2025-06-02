using UnityEngine;

namespace Yg.GameConfigs
{
    public static class ConfigLoader
    {
        private static readonly string MAP_GENERATION_CONFIG_PATH = "Configs/MapGeneration/MainMapGenerationConfig";
        private static readonly string NOISE_TO_TILE_TYPE_CONFIG_PATH = "Configs/Tiles/MainNoiseToTileTypeConfig";
        private static readonly string TILE_TYPE_TO_TILE_CONFIG_PATH = "Configs/Tiles/MainTileTypeToTileConfig";

        private static readonly string CASTLE_POINT_OF_INTEREST_CONFIG_PATH = "Configs/PointsOfInterest/CastleConfig";
        private static readonly string RESOURCE_POINT_OF_INTEREST_CONFIG_PATH = "Configs/PointsOfInterest/ResourceConfig";
        private static readonly string VILLAGE_POINT_OF_INTEREST_CONFIG_PATH = "Configs/PointsOfInterest/VillageConfig";
        private static readonly string TOWN_POINT_OF_INTEREST_CONFIG_PATH = "Configs/PointsOfInterest/TownConfig";

        // Map generation config
        public static DefaultMapGenerationConfigSO MapGenerationConfig;
        public static NoiseToTileTypeConfigSO NoiseToTileTypeConfig;
        public static TileTypeToTileConfigSO TileTypeToTileConfig;

        // Point of interest configs
        public static CastlePointOfInterestConfigSO CastlePointOfInterestConfig;
        public static ResourcePointOfInterestConfigSO ResourcePointOfInterestConfig;
        public static VillagePointOfInterestConfigSO VillagePointOfInterestConfig;
        public static TownPointOfInterestConfigSO TownPointOfInterestConfig;

        static ConfigLoader()
        {
            LoadConfigs();
        }

        private static void LoadConfigs()
        {
            MapGenerationConfig = Resources.Load<DefaultMapGenerationConfigSO>(MAP_GENERATION_CONFIG_PATH);
            NoiseToTileTypeConfig = Resources.Load<NoiseToTileTypeConfigSO>(NOISE_TO_TILE_TYPE_CONFIG_PATH);
            TileTypeToTileConfig = Resources.Load<TileTypeToTileConfigSO>(TILE_TYPE_TO_TILE_CONFIG_PATH);

            CastlePointOfInterestConfig = Resources.Load<CastlePointOfInterestConfigSO>(CASTLE_POINT_OF_INTEREST_CONFIG_PATH);
            ResourcePointOfInterestConfig = Resources.Load<ResourcePointOfInterestConfigSO>(RESOURCE_POINT_OF_INTEREST_CONFIG_PATH);
            VillagePointOfInterestConfig = Resources.Load<VillagePointOfInterestConfigSO>(VILLAGE_POINT_OF_INTEREST_CONFIG_PATH);
            TownPointOfInterestConfig = Resources.Load<TownPointOfInterestConfigSO>(TOWN_POINT_OF_INTEREST_CONFIG_PATH);
        }
    }
}
