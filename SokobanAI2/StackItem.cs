using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SokobanAI2
{
    public class StackItem
    {
        /// <summary>
        /// Cannot be modified by the class!
        /// </summary>
        public SokobanGraphNode Node { get; }
        public Queue<BoxMove> RemainingOptions { get; }
        public StackItem(SokobanGraphNode node)
        {
            Node = node;
            RemainingOptions = new Queue<BoxMove>(node.Children.Keys);
        }
    }
}
