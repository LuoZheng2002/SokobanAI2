namespace SokobanAI2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filename;
            while (true)
            {
                Console.WriteLine("Please input the text file name of a sokoban map (relative to the executable)");
                filename = Console.ReadLine()!;
                if (File.Exists(filename))
                {
                    break;
                }
                Console.WriteLine("The specified filename does not exist! Please input again.");
            }

            // Input Method 1: Construct from file
            SokobanProblem problem = SokobanProblem.ConstructFromFile(filename);

            // Input Method2: Construct from console
            // SokobanProblem problem2 = SokobanProblem.ConstructFromConsole();

            // Input Method3: Construct with hardcoded structure
            //List<string> consoleInput = new List<string>
            //{
            //    "wwww  ",
            //    "w gw  ",
            //    "w  www",
            //    "wBp  w",
            //    "w  b w",
            //    "w  www",
            //    "wwww  "
            //};
            //SokobanProblem problem3 = new SokobanProblem(consoleInput);

            SokobanAI.Solve(problem, out List<List<BoxMove>> boxMoves, out List<List<Direction>> playerMoves);
            
            // Printing results
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