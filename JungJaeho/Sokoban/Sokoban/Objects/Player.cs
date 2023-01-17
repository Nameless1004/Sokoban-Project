using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    class Player
    {
        public Player(Vector2 pos, int pushedBoxId, ConsoleColor color)
        {
            Pos = pos;
            _moveDirection = Direction.None;
            _pushedBoxIndex = 0;
            _color = color;
        }

        public  Vector2         Pos;

        private int             _pushedBoxIndex;
        private Direction       _moveDirection;
        private ConsoleColor    _color;

        public int          PushedBoxIndex { get { return _pushedBoxIndex; } set { _pushedBoxIndex = value; } }
        public Direction    MoveDirection { get { return _moveDirection; } set { _moveDirection = value; } }
        public ConsoleColor Color { get { return _color; } set { _color = value; } }
    }
}
