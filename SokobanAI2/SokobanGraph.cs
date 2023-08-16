using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SokobanAI2
{
    public class SokobanGraph
    {
        public SokobanGraphNode Root { get;}
        public SortedSet<SokobanBoxState> VisitedBoxStates { get;}
        public SokobanGraph(SokobanBoxState initialState)
        {
            Root = new SokobanGraphNode(initialState);
            VisitedBoxStates = new SortedSet<SokobanBoxState>(new SokobanBoxStateComparer());
            GenerateGraph();
        }
        private void GenerateGraph()
        {
            Console.WriteLine("[Info] Generating Graph...");
            Queue<SokobanGraphNode> unvisitedNodes = new Queue<SokobanGraphNode>();
            unvisitedNodes.Enqueue(Root);
            int loopCount = 0;
            while (unvisitedNodes.Count > 0)
            {
                loopCount++;
                if (loopCount > 10000000)
                {
                    throw new Exception("Loop doesn't stop");
                }
                if (loopCount%1000 == 0)
                {
                    Console.WriteLine($"[Info] {loopCount} states visited");
                }
                SokobanGraphNode node = unvisitedNodes.Dequeue();
                SokobanBoxState state = node.State;

                // Console.WriteLine("Considering State: ");
                // state.Print();

                if (VisitedBoxStates.Contains(state))
                {
                    // Console.WriteLine("This state is already visited.");
                    continue;
                }
                VisitedBoxStates.Add(state);
                if (state.GameOver)
                {
                    // Console.WriteLine("This state is game over!");
                    continue;
                }
                Dictionary<BoxMove, SokobanGraphNode> children = node.Children;
                bool hasChildren = false;
                foreach (SokobanPosition boxPosition in state.BoxPositions)
                {
                    foreach (Direction direction in DirectionMethods.Directions)
                    {
                        if (state.ValidMove(boxPosition, direction))
                        {
                            hasChildren = true;
                            SokobanBoxState newBoxState = state.ToNextState(boxPosition, direction);
                            SokobanGraphNode newGraphNode = new SokobanGraphNode(newBoxState);
                            children.Add(new BoxMove(boxPosition, direction), newGraphNode);
                            // Console.WriteLine("Found a child: ");
                            // newBoxState.Print();
                            unvisitedNodes.Enqueue(newGraphNode);
                        }
                    }
                }
                if (!hasChildren)
                {
                    // Console.WriteLine("There is no child for this state");
                }
            }
            Console.WriteLine("[Info] Graph generated!");
            Console.WriteLine($"[Info] Non-duplicated state count: {VisitedBoxStates.Count}");
        }
        public List<List<BoxMove>> GetAllSuccessfulSequences()
        {
            Console.WriteLine("[Info] Searching box routes...");
            List<List<BoxMove>> successfulSequences = new List<List<BoxMove>>();
            Stack<StackItem> stack = new();
            stack.Push(new StackItem(Root));
            SortedSet<SokobanBoxState> visitedBoxStates = new SortedSet<SokobanBoxState>(new SokobanBoxStateComparer())
            {
                Root.State
            };
            List<BoxMove> boxMoves = new List<BoxMove>();
            int loopCount = 0;
            while(stack.Count > 0)
            {
                loopCount++;
                if (loopCount > 10000000)
                {
                    throw new Exception("Loop doesn't stop");
                }
                StackItem stackItem = stack.Peek();
                SokobanBoxState state = stackItem.Node.State;
                bool pop = false;
                if (state.GameOver)
                {
                    pop = true;
                    successfulSequences.Add(new List<BoxMove>(boxMoves));
                }
                // pop stack if the path is dead end or the state collides with previous ones
                if (stackItem.RemainingOptions.Count == 0)
                {
                    pop = true;
                }
                if(pop)
                {
                    if (stack.Count == 1)
                        break;
                    stack.Pop();
                    visitedBoxStates.Remove(state);
                    boxMoves.RemoveAt(boxMoves.Count - 1);
                    continue;
                }
                // retrieve an option
                BoxMove boxMove = stackItem.RemainingOptions.Dequeue();
                bool hasValue = stackItem.Node.Children.TryGetValue(boxMove, out SokobanGraphNode? newNode);
                Debug.Assert(hasValue);
                SokobanBoxState newBoxState = newNode!.State;

                // if the newBoxState is visited, do nothing
                if (visitedBoxStates.Contains(newBoxState))
                    continue;
                // if the newBoxState is not visited, push stack
                StackItem newStackItem = new StackItem(newNode);
                stack.Push(newStackItem);
                visitedBoxStates.Add(newBoxState);
                boxMoves.Add(boxMove);
            }
            Console.WriteLine("[Info] All box routes found!");
            return successfulSequences;
        }
    }
}
