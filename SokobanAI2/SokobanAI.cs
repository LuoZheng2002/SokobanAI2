using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SokobanAI2
{
    using PositionAndDirection = (SokobanPosition Position, Direction? Direction);
    public class SokobanAI
    {
        public static void Solve(SokobanProblem problem,
            out List<List<BoxMove>> boxMoves,
            out List<List<Direction>> playerMoves)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            Console.WriteLine("[Info] Start solving a sokoban problem.");
            SokobanMap map = problem.Map;
            SokobanState initialState = problem.InitialState;
            SokobanBoxState initialBoxState = initialState.ToBoxState();
            SokobanGraph graph = new SokobanGraph(initialBoxState);
            boxMoves = graph.GetAllSuccessfulSequences();
            playerMoves = new List<List<Direction>>();
            Console.WriteLine("[Info] Generating player routes......");
            foreach (List<BoxMove> boxSequence in boxMoves)
            {
                Queue<BoxMove> boxMovesQueue = new Queue<BoxMove>(boxSequence);
                SokobanBoxState currentBoxState = initialBoxState;
                SokobanPosition currentPlayerPosition = initialState.PlayerPosition;
                List<Direction> playerSequence = new List<Direction>();
                while(boxMovesQueue.Count > 0)
                {
                    BoxMove boxMove = boxMovesQueue.Dequeue();
                    SokobanPosition endPosition = boxMove.BoxPosition.Neighbor(boxMove.Direction.Inverse());
                    List<Direction> route = FindRoute(currentBoxState, currentPlayerPosition, endPosition);
                    playerSequence.InsertRange(playerSequence.Count, route);
                    playerSequence.Add(boxMove.Direction);
                    currentBoxState = currentBoxState.ToNextState(boxMove.BoxPosition, boxMove.Direction);
                    currentPlayerPosition = boxMove.BoxPosition;
                }
                playerMoves.Add(playerSequence);
            }
            Console.WriteLine("[Info] All player routes generated!");
            stopwatch.Stop();
            Console.WriteLine($"[Info] Time elapsed: {stopwatch.Elapsed}");
        }
        private static List<Direction> FindRoute(SokobanBoxState boxState, SokobanPosition startPosition, SokobanPosition endPosition)
        {
            SokobanMap map = boxState.Map;
            Queue<PositionAndDirection> unvisited = new Queue<PositionAndDirection>();
            unvisited.Enqueue((startPosition, null));
            HashSet<SokobanPosition> visited = new HashSet<SokobanPosition>(new SokobanPositionComparer());
            Dictionary<SokobanPosition, Direction?> routes = new Dictionary<SokobanPosition, Direction?>(new SokobanPositionComparer());
            bool foundRoute = false;
            while(unvisited.Count > 0)
            {
                (SokobanPosition position, Direction? direction) = unvisited.Dequeue();
                if (visited.Contains(position))
                {
                    continue;
                }
                visited.Add(position);
                if (!map.InMap(position))
                {
                    continue;
                }
                if (map.GetTile(position) == SokobanTile.Wall)
                {
                    continue;
                }
                if (boxState.BoxPositions.Contains(position))
                {
                    continue;
                }
                if (!routes.ContainsKey(position))
                {
                    routes.Add(position, direction);
                }
                if (position == endPosition)
                {
                    foundRoute = true;
                    break;
                }
                foreach (Direction dir in DirectionMethods.Directions)
                {
                    // distance, direction
                    unvisited.Enqueue((position.Neighbor(dir), dir));
                }
            }
            if (!foundRoute)
            {
                throw new Exception("Cannot find route.");
            }
            List<Direction> directions = new List<Direction>();
            SokobanPosition currentPosition = endPosition;
            while(true)
            {
                bool found = routes.TryGetValue(currentPosition, out Direction? direction);
                if (!found)
                {
                    throw new Exception("The position is not in the routes");
                }
                if (currentPosition == startPosition)
                {
                    if (direction != null)
                    {
                        throw new Exception("The direction of start position should be null");
                    }
                    break;
                }
                if (direction == null)
                {
                    throw new Exception("Direction should not be null");
                }
                directions.Add(direction.Value);
                currentPosition = currentPosition.Neighbor(direction.Value.Inverse());
            }
            directions.Reverse();
            return directions;
        }
    }
}
