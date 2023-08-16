namespace SokobanAI2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> consoleInput = new List<string>
            {
                "      wwwwww",
                "      w    w",
                "  wwwww g  w",
                "www  wwwg  w",
                "w b  b  g ww",
                "w pbb w g w ",
                "ww    wwwww ",
                " wwwwww     "
            };
            SokobanProblem problem = new SokobanProblem(consoleInput);
            SokobanAI.Solve(problem, out List<List<BoxMove>> boxMoves, out List<List<Direction>> playerMoves);
            Console.WriteLine("Box moves:");
            for(int i = 0;i < boxMoves.Count;i++)
            {
                List<BoxMove> boxSequence = boxMoves[i];
                Console.WriteLine($"Plan {i + 1}: Total box moves: {boxSequence.Count}");
                foreach(var boxMove in boxSequence)
                {
                    Console.Write($"[({boxMove.BoxPosition.ColumnIndex}, {boxMove.BoxPosition.RowIndex}), {boxMove.Direction}] ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("Player moves:");
            for(int i = 0;i < playerMoves.Count; i++)
            {
                List<Direction> playerSequence = playerMoves[i];
                Console.WriteLine($"Plan {i + 1}: Total player moves: {playerSequence.Count}");
                foreach(var direction in playerSequence)
                {
                    Console.Write(direction.Symbol() + " ");
                }
                Console.WriteLine();
            }
        }
    }
}