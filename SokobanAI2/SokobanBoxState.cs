using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SokobanAI2
{
    public class SokobanBoxState
    {
        public SortedSet<SokobanPosition> BoxPositions { get; }
        public SortedSet<SokobanPosition> PlayerCloud { get; }
        public SokobanMap Map { get; }
        public bool GameOver { get; }
        /// <summary>
        /// Warning: This constructor can only be called by SokobanState class
        /// </summary>
        /// <param name="boxPositions"></param>
        /// <param name="playerCloud"></param>
        /// <param name="map"></param>
        public SokobanBoxState(SortedSet<SokobanPosition> boxPositions, SortedSet<SokobanPosition> playerCloud, SokobanMap map)
        {
            BoxPositions = boxPositions;
            PlayerCloud = playerCloud;
            Map = map;
            GameOver = IsGameOver();
        }
        public bool ValidMove(SokobanPosition boxPosition, Direction direction)
        {
            SokobanPosition playerPosition = boxPosition.Neighbor(direction.Inverse());
            if (!PlayerCloud.Contains(playerPosition))
                return false;
            SokobanPosition destPosition = boxPosition.Neighbor(direction);
            // destination cannot be wall
            if (Map.GetTile(destPosition) == SokobanTile.Wall)
                return false;
            // destination cannot be box
            if (BoxPositions.Contains(destPosition))
                return false;
            return true;
        }
        private bool IsGameOver()
        {
            bool gameover = true;
            foreach(var boxPosition in BoxPositions)
            {
                if (Map.GetTile(boxPosition) != SokobanTile.Goal)
                    gameover = false;
            }
            return gameover;
        }
        public SokobanBoxState ToNextState(SokobanPosition boxPosition, Direction direction)
        {
            if (!ValidMove(boxPosition, direction))
            {
                throw new Exception("Box move is not a valid move.");
            }
            SortedSet<SokobanPosition> newBoxPositions = new SortedSet<SokobanPosition>(BoxPositions, new SokobanPositionComparer());
            if (!newBoxPositions.Contains(boxPosition))
            {
                throw new Exception("Specified box does not exist.");
            }
            newBoxPositions.Remove(boxPosition);
            SokobanPosition newBoxPosition = boxPosition.Neighbor(direction);
            newBoxPositions.Add(newBoxPosition);
            SokobanPosition newPlayerPosition = boxPosition;
            SokobanState newState = new SokobanState(newPlayerPosition, newBoxPositions, Map);
            SokobanBoxState newBoxState = newState.ToBoxState();
            return newBoxState;
        }
        public void Print()
        {
            Console.WriteLine("Sokoban Box State: ");
            char[,] consoleOutput = new char[Map.RowCount, Map.ColumnCount];
            for(int i = 0; i < Map.RowCount; i++)
            {
                for(int j = 0; j < Map.ColumnCount; j++)
                {
                    SokobanPosition position = new SokobanPosition(i, j);
                    SokobanTile tile = Map.GetTile(position);
                    if (tile == SokobanTile.Empty)
                    {
                        if (BoxPositions.Contains(position))
                        {
                            consoleOutput[i, j] = 'b';
                        }
                        else if(PlayerCloud.Contains(position))
                        {
                            consoleOutput[i, j] = 'p';
                        }
                    }
                    else if(tile == SokobanTile.Wall)
                    {
                         if (BoxPositions.Contains(position) || PlayerCloud.Contains(position))
                         {
                            throw new Exception("A tile is not supposed to be both a wall and a player or a box");
                         }
                        consoleOutput[i, j] = 'w';
                    }
                    else if(tile == SokobanTile.Goal)
                    {
                        if (BoxPositions.Contains(position))
                        {
                            consoleOutput[i, j] = 'B';
                        }
                        else if(PlayerCloud.Contains(position))
                        {
                            consoleOutput[i, j] = 'P';
                        }
                        else
                        {
                            consoleOutput[i, j] = 'g';
                        }
                    }
                }
            }
            for(int i = 0;i < Map.RowCount; i++)
            {
                for(int j = 0;j < Map.ColumnCount; j++)
                {
                    char character = consoleOutput[i, j];
                    if (character != '\0')
                    {
                        Console.Write(character);
                    }
                    else
                    {
                        Console.Write(' ');
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
    public class SokobanBoxStateComparer : IComparer<SokobanBoxState>
    {
        public int Compare(SokobanBoxState? x, SokobanBoxState? y)
        {
            if (x == null && y == null)
                return 0;
            if (x == null)
                return -1;
            if (y == null)
                return 1;
            if (x.BoxPositions.Count != y.BoxPositions.Count)
                return x.BoxPositions.Count - y.BoxPositions.Count;
            List<SokobanPosition> xBoxList = x.BoxPositions.ToList();
            List<SokobanPosition> yBoxList = y.BoxPositions.ToList();
            for(int i = 0;i < xBoxList.Count;i++)
            {
                if (xBoxList.ElementAt(i) != yBoxList.ElementAt(i))
                    return new SokobanPositionComparer().Compare(xBoxList.ElementAt(i), yBoxList.ElementAt(i));
            }
            if (x.PlayerCloud.Count!= y.PlayerCloud.Count)
                return x.PlayerCloud.Count - y.PlayerCloud.Count;
            List<SokobanPosition> xPlayerList = x.PlayerCloud.ToList();
            List<SokobanPosition> yPlayerList = y.PlayerCloud.ToList();
            for(int i = 0;i < xPlayerList.Count;i++)
            {
                if (xPlayerList.ElementAt(i)!= yPlayerList.ElementAt(i))
                    return new SokobanPositionComparer().Compare(xPlayerList.ElementAt(i), yPlayerList.ElementAt(i));
            }
            return 0;
        }
        
    }
}
