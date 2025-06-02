using System.Collections.Generic;
using UnityEngine;
using Yg.GameConfigs;
using Zenject;

namespace Yg.MapGeneration
{
    public class PointOfInterestPlacer : MonoBehaviour
    {
        private Dictionary<Vector2Int, BasePointOfInterest> _pointOfInterestDictionary = new();

        private DefaultMapGenerationConfigSO _defaultMapGenerationConfigSO;

        private TileGameObjectPlacer _tileGameObjectPlacer;
        private Tileplacer _tilePlacer;
        private MapAssembler _mapAssembler;

        [Inject]
        private void Construct(TileGameObjectPlacer tileGameObjectPlacer, Tileplacer tileplacer, MapAssembler mapAssembler)
        {
            _tileGameObjectPlacer = tileGameObjectPlacer;
            _tilePlacer = tileplacer;
            _mapAssembler = mapAssembler;
        }

        public void Initialize()
        {
            _defaultMapGenerationConfigSO = ConfigLoader.MapGenerationConfig;
        }

        public void PlacePointsOfInterest()
        {
            PlaceCastlePoints();
            PlaceResourcePoints();
            PlaceVillagePoints();
            PlaceTownPoints();
        }

        private void PlaceCastlePoints()
        {
            Vector2Int pointPosition = FindPositionForPointOfInterest();
            CastlePointOfInterestConfigSO castlePointOfInterestConfigSO = ConfigLoader.CastlePointOfInterestConfig;

            CastlePoint castlePoint = new(castlePointOfInterestConfigSO, pointPosition);

            for (int x = -castlePoint.CastleAreaWidth / 2; x <= castlePoint.CastleAreaWidth / 2; x++)
                for (int y = -castlePoint.CastleAreaHeight / 2; y <= castlePoint.CastleAreaHeight / 2; y++)
                {
                    Vector2Int castleAreaOffset = new(x, y);

                    if (!_mapAssembler.WithinBounds(pointPosition + castleAreaOffset))
                        continue;

                    CreatePointOfInterest(pointPosition + castleAreaOffset, castlePoint);
                }
        }

        private void PlaceResourcePoints()
        {
            PlacePoints(
                ConfigLoader.ResourcePointOfInterestConfig,
                config => new ResourcePoint((ResourcePointOfInterestConfigSO)config)
            );
        }

        private void PlaceVillagePoints()
        {
            PlacePoints(
                ConfigLoader.VillagePointOfInterestConfig,
                config => new VillagePoint((VillagePointOfInterestConfigSO)config)
            );
        }

        private void PlaceTownPoints()
        {
            PlacePoints(
                ConfigLoader.TownPointOfInterestConfig,
                config => new TownPoint((TownPointOfInterestConfigSO)config)
            );
        }

        private void PlacePoints<T>(BasePointOfInterestConfigSO config, System.Func<BasePointOfInterestConfigSO, T> pointFactory)
            where T : BasePointOfInterest
        {
            int pointsAmount = UnityEngine.Random.Range(config.PointsAmountMin, config.PointsAmountMax + 1);

            for (int x = 0; x < pointsAmount; x++)
            {
                Vector2Int position = FindPositionForPointOfInterest();
                T point = pointFactory(config);
                CreatePointOfInterest(position, point);
            }
        }

        private Vector2Int FindPositionForPointOfInterest()
        {
            int xPosition = UnityEngine.Random.Range(0, _defaultMapGenerationConfigSO.MapWidth);
            int yPosition = UnityEngine.Random.Range(0, _defaultMapGenerationConfigSO.MapHeight);
            Vector2Int pointPosition = new(xPosition, yPosition);

            int count = 30;
            while(_pointOfInterestDictionary.ContainsKey(pointPosition))
            {
                xPosition = UnityEngine.Random.Range(0, _defaultMapGenerationConfigSO.MapWidth);
                yPosition = UnityEngine.Random.Range(0, _defaultMapGenerationConfigSO.MapHeight);
                pointPosition = new(xPosition, yPosition);

                count--;

                if(count < 0)
                {
                    Debug.LogError("Couldn't fine place for PointOfInterest");
                    return Vector2Int.zero;
                }
            }

            return pointPosition;
        }

        private void CreatePointOfInterest(Vector2Int position, BasePointOfInterest pointOfInterest)
        {
            _tileGameObjectPlacer.AssignPointOfInterestToTileAtPosition(position, pointOfInterest);
            _tilePlacer.PlacePointOfInterestTile(position, pointOfInterest.PointTile);
            _pointOfInterestDictionary.Add(position, pointOfInterest);
        }
    }
}