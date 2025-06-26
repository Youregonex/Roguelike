using System.Collections.Generic;
using System.Linq;
using Yg.MapGeneration;

namespace Yg.YgPathFinder
{
    public class Pathfinder
    {
        public static List<BaseTile> FindPath(BaseTile startTile, BaseTile targetTile, bool playerPath = false, int maxSteps = -1)
        {
            var toSearch = new HashSet<BaseTile>() { startTile };
            var processed = new HashSet<BaseTile>();

            while (toSearch.Any())
            {
                var current = toSearch.OrderBy(t => t.F).ThenBy(t => t.H).First();

                processed.Add(current);
                toSearch.Remove(current);

                if (current == targetTile)
                {
                    return ConstructPath(startTile, targetTile, maxSteps);
                }

                foreach (var neighbor in current.Neighbours.Where(t => !processed.Contains(t)))
                {
                    bool isTileWalkable = playerPath ? neighbor.PlayerWalkable : neighbor.Walkable;
                    if (!isTileWalkable) continue;

                    var costToNeighbor = current.G + current.GetDistanceToTile(neighbor);

                    if (!toSearch.Contains(neighbor) || costToNeighbor < neighbor.G)
                    {
                        neighbor.SetG(costToNeighbor);
                        neighbor.SetH(neighbor.GetDistanceToTile(targetTile));
                        neighbor.SetPreviousTile(current);

                        if (!toSearch.Contains(neighbor))
                        {
                            toSearch.Add(neighbor);
                        }
                    }
                }
            }

            return null;
        }

        private static List<BaseTile> ConstructPath(BaseTile startTile, BaseTile targetTile, int maxSteps)
        {
            var path = new List<BaseTile>();
            var current = targetTile;

            // Backtrack from target to start
            while (current != null && current != startTile)
            {
                path.Add(current);
                current = current.PreviousTile;
            }

            path.Reverse();

            if (maxSteps > 0 && path.Count > maxSteps)
                path = path.Take(maxSteps).ToList();

            return path;
        }
    }
}