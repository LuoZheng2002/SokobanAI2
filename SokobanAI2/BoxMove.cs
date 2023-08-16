using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SokobanAI2
{
    public struct BoxMove
    {
        public SokobanPosition BoxPosition{get;}
        public Direction Direction { get;}
        public BoxMove(SokobanPosition boxPosition, Direction direction)
        {
            BoxPosition = boxPosition;
            Direction = direction;
        }
    }
    public class BoxMoveComparer : IEqualityComparer<BoxMove>
    {
        public bool Equals(BoxMove x, BoxMove y)
        {
            return x.BoxPosition == y.BoxPosition && x.Direction == y.Direction;
        }

        public int GetHashCode([DisallowNull] BoxMove obj)
        {
            Tuple<SokobanPosition, Direction> tuple = new Tuple<SokobanPosition, Direction>(obj.BoxPosition, obj.Direction);
            return tuple.GetHashCode();
        }
    }
}
