using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Sokoban;
namespace Sokoban
{
    struct Teleporter
    {
        public Teleporter(Vector2 p1, Vector2 p2, string icon, ConsoleColor color)
        {
            Pos1 = p1;
            Pos2 = p2;
            Icon = icon;
            Color = color;
        }

        
        public Vector2 Pos1;
        public Vector2 Pos2;
        public string Icon;
        public ConsoleColor Color;

        public void Update(ref Player player, in Box[] boxes, in Wall[] walls)
        {
            TeleportPlayer(ref player);
            TeleportBox(ref player, ref boxes[player.PushedBoxIndex], in boxes, in walls);
        }
        
        private void TeleportPlayer(ref Player player)
        {
            if (player.Pos.X == Pos1.X && player.Pos.Y == Pos1.Y)
            {
                player.Pos.X = Pos2.X;
                player.Pos.Y = Pos2.Y;
            }
            else if (player.Pos.X == Pos2.X && player.Pos.Y == Pos2.Y)
            {
                player.Pos.X = Pos1.X;
                player.Pos.Y = Pos1.Y;
            }
        }

        private void TeleportBox(ref Player player, ref Box pushedBox, in Box[] boxes, in Wall[] walls)
        {
            // 박스 텔포
            if (pushedBox.Pos == Pos1)
            {
                Vector2 prevBoxPos = pushedBox.Pos;

                if (false == IsTeleportable(Pos2, player.MoveDirection, boxes, walls))
                {
                    switch (player.MoveDirection)
                    {
                        case Direction.Left:
                            pushedBox.Pos.X = prevBoxPos.X + 1;
                            pushedBox.Pos.Y = prevBoxPos.Y;
                            player.Pos.X = player.Pos.X + 1;
                            break;

                        case Direction.Right:
                            pushedBox.Pos.X = prevBoxPos.X - 1;
                            pushedBox.Pos.Y = prevBoxPos.Y;
                            player.Pos.X = player.Pos.X - 1;
                            break;

                        case Direction.Up:
                            pushedBox.Pos.X = prevBoxPos.X;
                            pushedBox.Pos.Y = prevBoxPos.Y + 1;
                            player.Pos.Y = player.Pos.Y + 1;
                            break;

                        case Direction.Down:
                            pushedBox.Pos.X = prevBoxPos.X;
                            pushedBox.Pos.Y = prevBoxPos.Y - 1;
                            player.Pos.Y = player.Pos.Y - 1;
                            break;
                    }
                }
                else
                {
                    switch (player.MoveDirection)
                    {
                        case Direction.Left:
                            pushedBox.Pos.X = Pos2.X - 1;
                            pushedBox.Pos.Y = Pos2.Y;
                            break;

                        case Direction.Right:
                            pushedBox.Pos.X = Pos2.X + 1;
                            pushedBox.Pos.Y = Pos2.Y;
                            break;

                        case Direction.Up:
                            pushedBox.Pos.X = Pos2.X;
                            pushedBox.Pos.Y = Pos2.Y - 1;
                            break;

                        case Direction.Down:
                            pushedBox.Pos.X = Pos2.X;
                            pushedBox.Pos.Y = Pos2.Y + 1;
                            break;
                    }
                }
            }
            else if (pushedBox.Pos == Pos2)
            {
                Vector2 prevBoxPos = pushedBox.Pos;


                if (false == IsTeleportable(Pos1, player.MoveDirection, in boxes,in walls))
                {
                    switch (player.MoveDirection)
                    {
                        case Direction.Left:
                            pushedBox.Pos.X = prevBoxPos.X + 1;
                            pushedBox.Pos.Y = prevBoxPos.Y;
                            player.Pos.X = player.Pos.X + 1;
                            break;

                        case Direction.Right:
                            pushedBox.Pos.X = prevBoxPos.X - 1;
                            pushedBox.Pos.Y = prevBoxPos.Y;
                            player.Pos.X = player.Pos.X - 1;
                            break;

                        case Direction.Up:
                            pushedBox.Pos.X = prevBoxPos.X;
                            pushedBox.Pos.Y = prevBoxPos.Y + 1;
                            player.Pos.Y = player.Pos.Y + 1;
                            break;

                        case Direction.Down:
                            pushedBox.Pos.X = prevBoxPos.X;
                            pushedBox.Pos.Y = prevBoxPos.Y - 1;
                            player.Pos.Y = player.Pos.Y - 1;
                            break;
                    }
                }
                else
                {
                    switch (player.MoveDirection)
                    {
                        case Direction.Left:
                            pushedBox.Pos.X = Pos1.X - 1;
                            pushedBox.Pos.Y = Pos1.Y;
                            break;

                        case Direction.Right:
                            pushedBox.Pos.X = Pos1.X + 1;
                            pushedBox.Pos.Y = Pos1.Y;
                            break;

                        case Direction.Up:
                            pushedBox.Pos.X = Pos1.X;
                            pushedBox.Pos.Y = Pos1.Y - 1;
                            player.Pos.Y = player.Pos.Y + 1;
                            break;

                        case Direction.Down:
                            pushedBox.Pos.X = Pos1.X;
                            pushedBox.Pos.Y = Pos1.Y + 1;
                            player.Pos.Y = player.Pos.Y - 1;
                            break;
                    }
                }

            }

        }

        private bool IsTeleportable(in Vector2 Pos, Direction moveDirection, in Box[] boxes, in Wall[] walls)
        {
            Vector2 right =  new Vector2(Pos.X + 1, Pos.Y);
            Vector2 left =   new Vector2(Pos.X - 1, Pos.Y);
            Vector2 top =    new Vector2(Pos.X, Pos.Y - 1);
            Vector2 bottom = new Vector2(Pos.X, Pos.Y + 1);

            switch (moveDirection)
            {
                case Direction.Left:
                    if(left.X < Game.MIN_X + Game.OFFSET_X)
                    {
                        return false;
                    }
                    for (int i = 0; i < boxes.Length; ++i)
                    {
                        if (boxes[i].Pos == left)
                        {
                            return false;
                        }
                    }
                    for (int i = 0; i < walls.Length; ++i)
                    {
                        if (walls[i].Pos == left)
                        {
                            return false;
                        }
                    }
                    break;
                case Direction.Right:
                    if(Game.MAX_X - Game.OFFSET_X < right.X)
                    {
                        return false;
                    }
                    for (int i = 0; i < boxes.Length; ++i)
                    {
                        if (boxes[i].Pos == right)
                        {
                            return false;
                        }
                    }
                    for (int i = 0; i < walls.Length; ++i)
                    {
                        if (walls[i].Pos == right)
                        {
                            return false;
                        }
                    }
                    break;
                case Direction.Up:
                    if(top.Y < Game.MIN_Y + Game.OFFSET_Y)
                    {
                        return false;
                    }

                    for (int i = 0; i < boxes.Length; ++i)
                    {
                        if (boxes[i].Pos == top)
                        {
                            return false;
                        }
                    }
                    for (int i = 0; i < walls.Length; ++i)
                    {
                        if (walls[i].Pos == top)
                        {
                            return false;
                        }
                    }
                    break;
                case Direction.Down:
                    if(Game.MAX_Y - Game.OFFSET_Y < bottom.Y)
                    {
                        return false;
                    }
                    for (int i = 0; i < boxes.Length; ++i)
                    {
                        if (boxes[i].Pos == bottom)
                        {
                            return false;
                        }
                    }
                    for (int i = 0; i < walls.Length; ++i)
                    {
                        if (walls[i].Pos == bottom)
                        {
                            return false;
                        }
                    }
                    break;
            }
            return true;
        }
        
    }
}
