using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SokobanAI2
{
    public class SokobanState
    {
        public SokobanPosition PlayerPosition { get;}
        public SortedSet<SokobanPosition> BoxPositions { get;}
        public SokobanMap Map { get;}
        public SokobanState(SokobanPosition playerPosition, SortedSet<SokobanPosition> boxPositions, SokobanMap map)
        {
            PlayerPosition = playerPosition;
            BoxPositions = boxPositions;
            Map = map;
        }
        public SokobanBoxState ToBoxState()
        {
            SokobanBoxState boxState = new SokobanBoxState(BoxPositions, GetPlayerCloud(), Map);
            return boxState;
        }
        private SortedSet<SokobanPosition> GetPlayerCloud()
        {
            SortedSet<SokobanPosition> playerCloud = new SortedSet<SokobanPosition>(new SokobanPositionComparer());
            playerCloud.Add(PlayerPosition);
            Queue<SokobanPosition> unvisited = new Queue<SokobanPosition>();
            unvisited.Enqueue(PlayerPosition);
            SortedSet<SokobanPosition> visited = new SortedSet<SokobanPosition>(new SokobanPositionComparer());
            int loopCount = 0;
            while (unvisited.Count > 0)
            {
                loopCount++;
                if (loopCount > 100000)
                {
                    throw new Exception("Loop doesn't stop");
                }

                SokobanPosition pos = unvisited.Dequeue();
                if (visited.Contains(pos))
                    continue;
                visited.Add(pos);
                //if (!Map.InMap(pos))
                //    continue;
                SokobanTile tile = Map.GetTile(pos);
                if (tile == SokobanTile.Wall)
                    continue;
                if (BoxPositions.Contains(pos))
                    continue;
                playerCloud.Add(pos);
                foreach (Direction direction in DirectionMethods.Directions)
                {
                    SokobanPosition newPos = pos.Neighbor(direction);
                    unvisited.Enqueue(newPos);
                }
            }
            return playerCloud;
        }
    }
}
