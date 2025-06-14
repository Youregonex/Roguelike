using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using Yg.GameConfigs;
using Yg.SaveLoad;
using Zenject;

namespace Yg.MapGeneration
{
    public class PointOfInterestPlacer : MonoBehaviour, ISaveable
    {
        private List<BasePointOfInterest> _pointOfInterestList = new();

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
            _defaultMapGenerationConfigSO = ResourceLoader.CONFIG_MapGeneration;
        }

        public void PlacePointsOfInterest()
        {
            if(_pointOfInterestList != null && _pointOfInterestList.Count > 0)
            {
                LoadPointsOfInterest();
                return;
            }

            PlaceCastlePoints();
            PlaceResourcePoints();
            PlaceVillagePoints();
            PlaceTownPoints();
        }

        public object CaptureState()
        {
            return _pointOfInterestList;
        }

        public void RestoreState(object data)
        {
            var pointOfInterestSaveDataList = data as List<BasePointOfInterest>
                ?? JsonConvert.DeserializeObject<List<BasePointOfInterest>>(JsonConvert.SerializeObject(data));

            if (pointOfInterestSaveDataList == null)
            {
                Debug.LogError("Data is null!");
                return;
            }

            _pointOfInterestList = pointOfInterestSaveDataList;
        }

        private void LoadPointsOfInterest()
        {
            for (int i = 0; i < _pointOfInterestList.Count; i++)
                CreatePointOfInterest(_pointOfInterestList[i], true);
        }

        private void PlaceCastlePoints()
        {
            CastlePointOfInterestConfigSO castlePointOfInterestConfigSO = ResourceLoader.CONFIG_CastlePointOfInterest;

            CastlePoint castlePoint;
            Vector2Int castleOrigin = FindPositionForPointOfInterest();

            for (int x = -castlePointOfInterestConfigSO.CastleAreaWidth / 2; x <= castlePointOfInterestConfigSO.CastleAreaWidth / 2; x++)
                for (int y = -castlePointOfInterestConfigSO.CastleAreaHeight / 2; y <= castlePointOfInterestConfigSO.CastleAreaHeight / 2; y++)
                {
                    Vector2Int castleAreaOffset = new(x, y);
                    castlePoint = new(castlePointOfInterestConfigSO, castleOrigin, castleOrigin + castleAreaOffset);

                    if (!_mapAssembler.WithinBounds(castleOrigin + castleAreaOffset))
                        continue;

                    CreatePointOfInterest(castlePoint, false);
                }
        }

        private void PlaceResourcePoints()
        {
            PlacePoints(
                ResourceLoader.CONFIG_ResourcePointOfInterest,
                config => new ResourcePoint((ResourcePointOfInterestConfigSO)config, FindPositionForPointOfInterest())
            );
        }

        private void PlaceVillagePoints()
        {
            PlacePoints(
                ResourceLoader.CONFIG_VillagePointOfInterest,
                config => new VillagePoint((VillagePointOfInterestConfigSO)config, FindPositionForPointOfInterest())
            );
        }

        private void PlaceTownPoints()
        {
            PlacePoints(
                ResourceLoader.CONFIG_TownPointOfInterest,
                config => new TownPoint((TownPointOfInterestConfigSO)config, FindPositionForPointOfInterest())
            );
        }

        private void PlacePoints<T>(BasePointOfInterestConfigSO config, System.Func<BasePointOfInterestConfigSO, T> pointFactory)
            where T : BasePointOfInterest
        {
            int pointsAmount = UnityEngine.Random.Range(config.PointsAmountMin, config.PointsAmountMax + 1);

            for (int x = 0; x < pointsAmount; x++)
            {
                T point = pointFactory(config);
                CreatePointOfInterest(point, false);
            }
        }

        private Vector2Int FindPositionForPointOfInterest()
        {
            int xPosition = UnityEngine.Random.Range(0, _defaultMapGenerationConfigSO.MapWidth);
            int yPosition = UnityEngine.Random.Range(0, _defaultMapGenerationConfigSO.MapHeight);
            Vector2Int pointPosition = new(xPosition, yPosition);

            int count = 50;
            while(_pointOfInterestList.Contains(_pointOfInterestList.Where(e => e.PointPosition == pointPosition).FirstOrDefault()))
            {
                xPosition = UnityEngine.Random.Range(0, _defaultMapGenerationConfigSO.MapWidth);
                yPosition = UnityEngine.Random.Range(0, _defaultMapGenerationConfigSO.MapHeight);
                pointPosition = new(xPosition, yPosition);

                count--;

                if(count < 0)
                {
                    Debug.LogError("Couldn't find place for PointOfInterest");
                    return Vector2Int.zero;
                }
            }

            return pointPosition;
        }

        private void CreatePointOfInterest(BasePointOfInterest pointOfInterest, bool fromSaveData)
        {
            _tileGameObjectPlacer.AssignPointOfInterestToTileAtPosition(pointOfInterest.PointPosition, pointOfInterest);
            _tilePlacer.PlacePointOfInterestTile(pointOfInterest.PointPosition, pointOfInterest.GetPointTile());

            if(!fromSaveData) _pointOfInterestList.Add(pointOfInterest);
        }
    }
}