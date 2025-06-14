using System;
using System.Collections.Generic;
using System.Linq;
using Yg.MapGeneration;

namespace Yg.YgPathFinder
{
    public class Pathfinder
    {
        public static List<BaseTile> FindPath(BaseTile startTile, BaseTile targetTile, bool playerPath = false)
        {
            var toSearch = new List<BaseTile>() { startTile };
            var processed = new List<BaseTile>();

            while (toSearch.Any())
            {
                var current = toSearch[0];
                foreach (var t in toSearch)
                    if (t.F < current.F || t.F == current.F && t.H < current.H)
                        current = t;

                processed.Add(current);
                toSearch.Remove(current);

                if (current == targetTile)
                {
                    var currentPathTile = targetTile;
                    var path = new List<BaseTile>();
                    var count = 100;
                    while (currentPathTile != startTile)
                    {
                        path.Add(currentPathTile);
                        currentPathTile = currentPathTile.PreviousTile;
                        count--;
                        if (count < 0) throw new Exception();
                    }

                    path.Reverse();
                    return path;
                }

                bool isTileWalkable;
                foreach (var neighbor in current.Neighbours.Where(t => !processed.Contains(t)))
                {
                    isTileWalkable = playerPath ? neighbor.PlayerWalkable : neighbor.Walkable;
                    if (!isTileWalkable) continue;

                    var inSearch = toSearch.Contains(neighbor);
                    var costToNeighbor = current.G + current.GetDistanceToTile(neighbor);

                    if (!inSearch || costToNeighbor < neighbor.G)
                    {
                        neighbor.SetG(costToNeighbor);
                        neighbor.SetPreviousTile(current);

                        if (!inSearch)
                        {
                            neighbor.SetH(neighbor.GetDistanceToTile(targetTile));
                            toSearch.Add(neighbor);
                        }
                    }
                }
            }

            return null;
        }
    }
}
