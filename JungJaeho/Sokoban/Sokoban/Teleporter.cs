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
        public Teleporter(Vector2 p1, Vector2 p2, string icon)
        {
            Pos1 = p1;
            Pos2 = p2;
            Icon = icon;
        }

        public Vector2 Pos1;
        public Vector2 Pos2;
        public string Icon;

        public void Update(ref Player player, in Box[] boxes, in Wall[] walls)
        {
            TeleportPlayer(ref player);
            TeleportBox(ref player, ref boxes[player.PushedBoxIndex], in boxes, in walls);
        }
        
        private void TeleportPlayer(ref Player player)
        {
            if (player.X == Pos1.X && player.Y == Pos1.Y)
            {
                player.X = Pos2.X;
                player.Y = Pos2.Y;
            }
            else if (player.X == Pos2.X && player.Y == Pos2.Y)
            {
                player.X = Pos1.X;
                player.Y = Pos1.Y;
            }
        }

        private void TeleportBox(ref Player player, ref Box pushedBox, in Box[] boxes, in Wall[] walls)
        {
            // 박스 텔포
            if (pushedBox.X == Pos1.X && pushedBox.Y ==  Pos1.Y)
            {
                int prevBoxPosX = pushedBox.X;
                int prevBoxPosY = pushedBox.Y;

                if (false == IsTeleportable(Pos2, player.MoveDirection, boxes, walls))
                {
                    switch (player.MoveDirection)
                    {
                        case Direction.Left:
                            pushedBox.X = prevBoxPosX + 1;
                            pushedBox.Y = prevBoxPosY;
                            player.X = player.X + 1;
                            break;

                        case Direction.Right:
                            pushedBox.X = prevBoxPosX - 1;
                            pushedBox.Y = prevBoxPosY;
                            player.X = player.X - 1;
                            break;

                        case Direction.Up:
                            pushedBox.X = prevBoxPosX;
                            pushedBox.Y = prevBoxPosY + 1;
                            player.Y = player.Y + 1;
                            break;

                        case Direction.Down:
                            pushedBox.X = prevBoxPosX;
                            pushedBox.Y = prevBoxPosY - 1;
                            player.Y = player.Y - 1;
                            break;
                    }
                }
                else
                {
                    switch (player.MoveDirection)
                    {
                        case Direction.Left:
                            pushedBox.X = Pos2.X - 1;
                            pushedBox.Y = Pos2.Y;
                            break;

                        case Direction.Right:
                            pushedBox.X = Pos2.X + 1;
                            pushedBox.Y = Pos2.Y;
                            break;

                        case Direction.Up:
                            pushedBox.X = Pos2.X;
                            pushedBox.Y = Pos2.Y - 1;
                            player.Y = player.Y + 1;
                            break;

                        case Direction.Down:
                            pushedBox.X = Pos2.X;
                            pushedBox.Y = Pos2.Y + 1;
                            player.Y = player.Y - 1;
                            break;
                    }
                }
            }
            else if (pushedBox.X == Pos2.X && pushedBox.Y == Pos2.Y)
            {
                int prevBoxPosX = pushedBox.X;
                int prevBoxPosY = pushedBox.Y;


                if (false == IsTeleportable(Pos1, player.MoveDirection, in boxes,in walls))
                {
                    switch (player.MoveDirection)
                    {
                        case Direction.Left:
                            pushedBox.X = prevBoxPosX + 1;
                            pushedBox.Y = prevBoxPosY;
                            player.X = player.X + 1;
                            break;

                        case Direction.Right:
                            pushedBox.X = prevBoxPosX - 1;
                            pushedBox.Y = prevBoxPosY;
                            player.X = player.X - 1;
                            break;

                        case Direction.Up:
                            pushedBox.X = prevBoxPosX;
                            pushedBox.Y = prevBoxPosY + 1;
                            player.Y = player.Y + 1;
                            break;

                        case Direction.Down:
                            pushedBox.X = prevBoxPosX;
                            pushedBox.Y = prevBoxPosY - 1;
                            player.Y = player.Y - 1;
                            break;
                    }
                }
                else
                {
                    switch (player.MoveDirection)
                    {
                        case Direction.Left:
                            pushedBox.X = Pos1.X - 1;
                            pushedBox.Y = Pos1.Y;
                            break;

                        case Direction.Right:
                            pushedBox.X = Pos1.X + 1;
                            pushedBox.Y = Pos1.Y;
                            break;

                        case Direction.Up:
                            pushedBox.X = Pos1.X;
                            pushedBox.Y = Pos1.Y - 1;
                            player.Y = player.Y + 1;
                            break;

                        case Direction.Down:
                            pushedBox.X = Pos1.X;
                            pushedBox.Y = Pos1.Y + 1;
                            player.Y = player.Y - 1;
                            break;
                    }
                }

            }

        }

        private bool IsTeleportable(Vector2 Pos, Direction moveDirection, in Box[] boxes, in Wall[] walls)
        {
            int right = Pos.X + 1;
            int left = Pos.X - 1;
            int top = Pos.Y - 1;
            int bottom = Pos.Y + 1;
            if (left <= Sokoban.MIN_X + Sokoban.OFFSET_X || Sokoban.MAX_X - Sokoban.OFFSET_X <= right || top <= Sokoban.MIN_Y + Sokoban.OFFSET_Y || Sokoban.MAX_Y - Sokoban.OFFSET_Y <= bottom)
                return false;
            switch (moveDirection)
            {
                case Direction.Left:
                    for (int i = 0; i < boxes.Length; ++i)
                    {
                        if (boxes[i].X == left && boxes[i].Y == Pos.Y)
                        {
                            return false;
                        }
                    }
                    for (int i = 0; i < walls.Length; ++i)
                    {
                        if (walls[i].X == left && walls[i].Y == Pos.Y)
                        {
                            return false;
                        }
                    }
                    break;
                case Direction.Right:
                    for (int i = 0; i < boxes.Length; ++i)
                    {
                        if (boxes[i].X == right && boxes[i].Y == Pos.Y)
                        {
                            return false;
                        }
                    }
                    for (int i = 0; i < walls.Length; ++i)
                    {
                        if (walls[i].X == left && walls[i].Y == Pos.Y)
                        {
                            return false;
                        }
                    }
                    break;
                case Direction.Up:
                    for (int i = 0; i < boxes.Length; ++i)
                    {
                        if (boxes[i].X == Pos.X && boxes[i].Y == top)
                        {
                            return false;
                        }
                    }
                    for (int i = 0; i < walls.Length; ++i)
                    {
                        if (walls[i].X == Pos.X && walls[i].Y == top)
                        {
                            return false;
                        }
                    }
                    break;
                case Direction.Down:
                    for (int i = 0; i < boxes.Length; ++i)
                    {
                        if (boxes[i].X == Pos.X && boxes[i].Y == bottom)
                        {
                            return false;
                        }
                    }
                    for (int i = 0; i < walls.Length; ++i)
                    {
                        if (walls[i].X == Pos.X && walls[i].Y == bottom)
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
