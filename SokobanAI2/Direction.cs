using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SokobanAI2
{
    public enum Direction
    {
        Top,
        Bottom,
        Left,
        Right
    }
    public static class DirectionMethods
    {
        public static List<Direction> Directions = new List<Direction>
        {
            Direction.Top,
            Direction.Bottom,
            Direction.Left,
            Direction.Right
        };
        public static Direction Inverse(this Direction direction)
        {
            Direction inverse;
            switch(direction)
            {
                case Direction.Top:
                    inverse = Direction.Bottom;
                    break;
                case Direction.Bottom:
                    inverse = Direction.Top;
                    break;
                case Direction.Left:
                    inverse = Direction.Right;
                    break;
                default:
                    inverse = Direction.Left;
                    break;
            }
            return inverse;
        }
        public static char Symbol(this Direction direction)
        {
            char symbol;
            switch (direction)
            {
                case Direction.Top:
                    symbol = '↑';
                    break;
                case Direction.Bottom:
                    symbol = '↓';
                    break;
                case Direction.Left:
                    symbol = '←';
                    break;
                default:
                    symbol = '→';
                    break;
            }
            return symbol;
        }
    }
}
