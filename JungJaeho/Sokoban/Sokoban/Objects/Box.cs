using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    public class Box
    {
        public Box(Vector2 pos, ConsoleColor color)
        {
            Pos = pos;
            IsOnGoal = false;
            Color = color;
        }
        public Vector2 Pos;
        public bool IsOnGoal;
        public ConsoleColor Color;

        public void MoveToDirection(Direction dir)
        {
            switch (dir)
            {
                case Direction.Left:
                    Pos.X -= 1;
                    break;
                case Direction.Right:
                    Pos.X += 1;
                    break;
                case Direction.Up:
                    Pos.Y -= 1;
                    break;
                case Direction.Down:
                    Pos.Y += 1;
                    break;
            }

        }
        public void OnCollision(Direction dir)
        {
            switch (dir)
            {
                case Direction.Left:
                    Pos.X += 1;
                    break;
                case Direction.Right:
                    Pos.X -= 1;
                    break;
                case Direction.Up:
                    Pos.Y += 1;
                    break;
                case Direction.Down:
                    Pos.Y -= 1;
                    break;
            }
        }
    }
}
