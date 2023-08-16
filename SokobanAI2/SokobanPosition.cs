using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SokobanAI2
{
    public struct SokobanPosition
    {
        public int RowIndex { get; }
        public int ColumnIndex { get; }
        private bool Equals(SokobanPosition other)
        {
            return RowIndex == other.RowIndex && ColumnIndex == other.ColumnIndex;
        }
        public static bool operator ==(SokobanPosition pos1, SokobanPosition pos2)
        {
            return pos1.Equals(pos2);
        }
        public static bool operator !=(SokobanPosition pos1, SokobanPosition pos2)
        {
            return !pos1.Equals(pos2);
        }
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj == null || obj is not SokobanPosition)
                return false;
            SokobanPosition rhs = (SokobanPosition)obj;
            return Equals(rhs);
        }
        public override int GetHashCode()
        {
            Tuple<int, int> tuple = new Tuple<int, int>(RowIndex, ColumnIndex);
            return tuple.GetHashCode();
        }
        public SokobanPosition(int rowIndex, int columnIndex)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
        }
        public SokobanPosition Neighbor(Direction direction)
        {
            SokobanPosition newPosition;
            switch (direction)
            {
                case Direction.Top:
                    newPosition = new SokobanPosition(RowIndex - 1, ColumnIndex);
                    break;
                case Direction.Bottom:
                    newPosition = new SokobanPosition(RowIndex + 1, ColumnIndex);
                    break;
                case Direction.Left:
                    newPosition = new SokobanPosition(RowIndex, ColumnIndex - 1);
                    break;
                default:
                    newPosition = new SokobanPosition(RowIndex, ColumnIndex + 1);
                    break;
            }
            return newPosition;
        }
    }
    public class SokobanPositionComparer : IEqualityComparer<SokobanPosition>, IComparer<SokobanPosition>
    {
        public int Compare(SokobanPosition x, SokobanPosition y)
        {
            if (x.RowIndex != y.RowIndex)
            {
                return x.RowIndex - y.RowIndex;
            }
            else
            {
                return x.ColumnIndex - y.ColumnIndex;
            }
        }

        public bool Equals(SokobanPosition x, SokobanPosition y)
        {
            return x.RowIndex == y.RowIndex && x.ColumnIndex == y.ColumnIndex;
        }
        public int GetHashCode(SokobanPosition obj)
        {
            Tuple<int, int> tuple = new Tuple<int, int>(obj.RowIndex, obj.ColumnIndex);
            return tuple.GetHashCode();
        }
    }
}
