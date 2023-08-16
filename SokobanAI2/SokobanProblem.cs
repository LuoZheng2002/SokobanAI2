using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SokobanAI2
{
    public class SokobanProblem
    {
        public SokobanMap Map { get; }
        public SokobanState InitialState { get; }
        public SokobanProblem(List<string> consoleInput)
        {
            GetProblemFromConsole(consoleInput, out SokobanMap map, out SokobanState initialState);
            Map = map;
            InitialState = initialState;
        }
        public static SokobanProblem ConstructFromConsole()
        {
            List<string> consoleInput = new List<string>();
            while(true)
            {
                string line = Console.ReadLine()!;
                if (line.Length == 0)
                    break;
                consoleInput.Add(line);
            }
            return new SokobanProblem(consoleInput);
        }
        private void GetProblemFromConsole(List<string> consoleInput, out SokobanMap map, out SokobanState state)
        {
            // to do
            int rowCount = consoleInput.Count;
            int columnCount = consoleInput.Max((line) => line.Length);
            SokobanTile[,] tiles = new SokobanTile[rowCount, columnCount];
            SortedSet<SokobanPosition> boxPositions = new SortedSet<SokobanPosition>(new SokobanPositionComparer());
            SokobanPosition playerPosition = new SokobanPosition();
            bool playerSet = false;
            for(int i = 0; i < rowCount; i++)
            {
                string line = consoleInput[i];
                for (int j = 0; j < columnCount; j++)
                {
                    switch(line[j])
                    {
                        case ' ':
                            break;
                        case 'w':
                            tiles[i, j] = SokobanTile.Wall;
                            break;
                        case 'b':
                            boxPositions.Add(new SokobanPosition(i, j));
                            break;
                        case 'B':
                            tiles[i, j] = SokobanTile.Goal;
                            boxPositions.Add(new SokobanPosition(i, j));
                            break;
                        case 'p':
                            if (playerSet)
                            {
                                throw new Exception("More than one player on the map");
                            }
                            playerPosition = new SokobanPosition(i, j);
                            playerSet = true;
                            break;
                        case 'P':
                            if (playerSet)
                            {
                                throw new Exception("More than one player on the map");
                            }
                            tiles[i, j] = SokobanTile.Goal;
                            playerPosition = new SokobanPosition(i, j);
                            playerSet = true;
                            break;
                        case 'g':
                            tiles[i, j] = SokobanTile.Goal;
                            break;
                    }
                }
            }
            if (!playerSet)
            {
                throw new Exception("There is no player in the map");
            }
            map = new SokobanMap(tiles);
            state = new SokobanState(playerPosition, boxPositions, map);
        }
    }
}
