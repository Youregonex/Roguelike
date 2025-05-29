using System.Collections.Generic;
using UnityEngine;

public class MapGenerator
{
    [CustomHeader("Settings")]
    [SerializeField] private int _width;
    [SerializeField] private int _height;


    public List<Vector2Int> GenerateMap(int width, int height)
    {
        List<Vector2Int> mapTilePositionList = new(width * height);

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                mapTilePositionList.Add(new Vector2Int(x, y));

        return mapTilePositionList;
    }
}
