using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    struct Goal
    {
        public Goal(Vector2 pos, ConsoleColor color)
        {
            Pos = pos;
            Color = color;
        }
        public Vector2 Pos;
        public ConsoleColor Color;
    }

    //class Goal
    //{
    //    private int _x;
    //    private int _y;

    //    public int X { get { return _x; } set { _x = value; } }
    //    public int Y { get { return _y; } }
    //}
}
