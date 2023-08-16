using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SokobanAI2
{
    public class SokobanMap
    {
        private SokobanTile[,] _tiles;
        public int RowCount { get; }
        public int ColumnCount { get; }
        public SokobanMap(SokobanTile[,] tiles)
        {
            _tiles = tiles;
            RowCount = _tiles.GetLength(0);
            ColumnCount = _tiles.GetLength(1);
        }
        public bool InMap(SokobanPosition position)
        {
            return position.RowIndex >= 0 && position.RowIndex < RowCount
                && position.ColumnIndex >=0 && position.ColumnIndex < ColumnCount;
        }
        public SokobanTile GetTile(SokobanPosition position)
        {
            return _tiles[position.RowIndex, position.ColumnIndex];
        }
    }
}
