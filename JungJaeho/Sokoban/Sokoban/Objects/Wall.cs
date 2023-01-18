using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    public class Wall
    {
        public Wall(Vector2 pos, ConsoleColor color)
        {
            Pos = pos;
            Color = color;
        }
        public Vector2      Pos;
        public ConsoleColor Color;
    }
}
