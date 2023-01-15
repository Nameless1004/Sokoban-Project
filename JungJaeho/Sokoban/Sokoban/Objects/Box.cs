using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    struct Box
    {
        public Box(Vector2 pos, ConsoleColor color)
        {
            Pos = pos;
            IsOnGoal= false;
            Color = color;
        }
        public Vector2 Pos;
        public bool IsOnGoal;
        public ConsoleColor Color;
    }

    //class Box
    //{
    //    private int _x;
    //    private int _y;
    //    private bool _isOnGoal;

    //    public int X { get { return _x; } set { _x = value; } }
    //    public int Y { get { return _y; } set { _y = value; } }
    //    public bool IsOnGoal { get { return _isOnGoal; } set { _isOnGoal = IsOnGoal; } }
    //}

}
