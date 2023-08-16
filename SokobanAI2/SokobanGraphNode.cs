using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SokobanAI2
{
    public class SokobanGraphNode
    {
        public SokobanBoxState State { get; set; }
        public Dictionary<BoxMove, SokobanGraphNode> Children { get; set; }
        public SokobanGraphNode(SokobanBoxState state)
        {
            State = state;
            Children = new Dictionary<BoxMove, SokobanGraphNode>(new BoxMoveComparer());
        }
    }
}
