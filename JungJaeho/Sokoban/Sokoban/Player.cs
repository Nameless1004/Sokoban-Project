using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    struct Player
    {
        public int X;
        public int Y;
        public int PushedBoxIndex;
        public Direction MoveDirection;
        public ConsoleColor Color;
    }

    //    class Player : GameObject
    //    {
    //        public Player(int x, int y)
    //        {
    //            _x = x;
    //            _y = y;
    //            _moveDirection = Direction.None;
    //            _pushedBoxIndex = 0;
    //        }
    //        private Direction _moveDirection;
    //        private int _pushedBoxIndex;

    //        public Vector2 Pos { get { return _pos; } set { _pos = value; } }
    //        public int Y { get { return _y; } set { _y = value; } }
    //        public Direction MoveDirection { get { return _moveDirection; } set { _moveDirection = value; } }
    //        public int PushedBoxIndex { get { return _pushedBoxIndex; } set { _pushedBoxIndex = value; } }
    //        //int GetX() => _x;
    //        //int SetX(int data) => _x = data;
    //        //int GetY() => _y;
    //        //int SetY(int data) => _y = data;
    //        //Direction GetDirection() => _moveDirection;
    //        //void SetDirection(Direction dir) => _moveDirection = dir;
    //        //int GetPushedBoxId() => _pushedBoxIndex;
    //        //void SetPushedBoxId(int data) => _pushedBoxIndex = data;
    //    }
    //}
}
