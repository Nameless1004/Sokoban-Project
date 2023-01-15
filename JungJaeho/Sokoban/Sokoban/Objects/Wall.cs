using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    struct Wall
    {
        public Wall(Vector2 pos, ConsoleColor color)
        {
            Pos = pos;
            Color = color;
        }
        public Vector2 Pos;
        public ConsoleColor Color;
    }

    //class Wall
    //{
    //    public Wall(int x, int y) { _x = x; _y = y; }

    //    private int _x;
    //    private int _y;

    //    public int X { get { return _x; } }
    //    public int Y { get { return _y; } }
    //}

}
