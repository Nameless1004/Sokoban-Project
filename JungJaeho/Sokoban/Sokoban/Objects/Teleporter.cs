using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Sokoban;
namespace Sokoban
{
    class Teleporter
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

        public void Update(Player player, in Box[] boxes, in Wall[] walls)
        {
            TeleportPlayer(player);
            TeleportBox(player, boxes[player.PushedBoxIndex], in boxes, in walls);
        }
        
        private void TeleportPlayer(Player player)
        {
            if (IsCollided(player.Pos, Pos1))
            {
                int randomVal = RandomManager.Instance.GetRandomRangeInt(1, 2);
                if(randomVal <= 1)
                {
                    Console.Clear();
                    Console.WriteLine("죽음 ㅅㄱ");
                    SoundManager.Instance.PlaySound("ending");
                    Thread.Sleep(1000);
                    Environment.Exit(1);      
                }
                OnCollision(() => { MoveToDest(out player.Pos, in Pos2); });
            }
            else if (IsCollided(player.Pos, Pos2))
            {
                OnCollision(() => { MoveToDest(out player.Pos, in Pos1); });
            }
        }

        private void MoveToDest(out Vector2 start, in Vector2 dest)
        {
            start = dest;
        }

        private void OnCollision(Action action)
        {
            action();
        }

        private bool IsCollided(in Vector2 obj, in Vector2 comp)
        {
            return obj == comp;
        }

        void MoveToLeftOfTarget(out Vector2 pos, in Vector2 target) => pos = new Vector2(Math.Max(Game.MIN_X + Game.OFFSET_X, target.X - 1), target.Y);
        void MoveToRightOfTarget(out Vector2 pos, in Vector2 target) => pos = new Vector2(Math.Min(target.X + 1, Game.MAX_X - Game.OFFSET_X), target.Y);
        void MoveToUpOfTarget(out Vector2 pos, in Vector2 target) => pos = new Vector2(target.X, Math.Max(Game.MIN_Y + Game.OFFSET_Y, target.Y - 1));
        void MoveToDownOfTarget(out Vector2 pos, in Vector2 target) => pos = new Vector2(target.X, Math.Min(target.Y + 1, Game.MAX_Y - Game.OFFSET_Y));

        void MoveToTarget(Direction dir, ref Vector2 obj, in Vector2 targetPos)
        {
            switch (dir)
            {
                case Direction.Left:
                    MoveToLeftOfTarget(out obj, in targetPos);
                    break;
                case Direction.Right:
                    MoveToRightOfTarget(out obj,in targetPos);
                    break;
                case Direction.Up:
                    MoveToUpOfTarget(out obj, in targetPos);
                    break;
                case Direction.Down:
                    MoveToDownOfTarget(out obj, in targetPos);
                    break;
            }
        }

        void PushOut(Direction dir, ref Vector2 obj, in Vector2 targetPos)
        {
            switch (dir)
            {
                case Direction.Left:
                    MoveToRightOfTarget(out obj, targetPos);
                    break;
                case Direction.Right:
                    MoveToLeftOfTarget(out obj, targetPos);
                    break;
                case Direction.Up:
                    MoveToDownOfTarget(out obj, targetPos);
                    break;
                case Direction.Down:
                    MoveToUpOfTarget(out obj, targetPos);
                    break;
            }
        }

        private void TeleportBox(Player player, Box pushedBox, in Box[] boxes, in Wall[] walls)
        {
            if (IsCollided(in pushedBox.Pos, in Pos1))
            {
                if (IsTeleportable(player.MoveDirection, Pos2, boxes, walls))
                {
                    OnCollision(() => { MoveToTarget(player.MoveDirection, ref pushedBox.Pos, in Pos2); });
                }
                else
                {
                    OnCollision(() => PushOut(player.MoveDirection, ref pushedBox.Pos, Pos1));
                    OnCollision(() => PushOut(player.MoveDirection, ref player.Pos, pushedBox.Pos));
                }
            }
            else if(IsCollided(in pushedBox.Pos, in Pos2))
            {
                if (IsTeleportable(player.MoveDirection, Pos1, boxes, walls))
                {
                    OnCollision(() => { MoveToTarget(player.MoveDirection, ref pushedBox.Pos, in Pos1); });
                }
                else
                {
                    OnCollision(() => PushOut(player.MoveDirection, ref pushedBox.Pos, Pos1));
                    OnCollision(() => PushOut(player.MoveDirection, ref player.Pos, pushedBox.Pos));
                }
            }
        }

        private bool IsTeleportable(Direction moveDirection, in Vector2 Pos, in Box[] boxes, in Wall[] walls)
        {
            Vector2 right =  new Vector2(Pos.X + 1, Pos.Y);
            Vector2 left =   new Vector2(Pos.X - 1, Pos.Y);
            Vector2 top =    new Vector2(Pos.X, Pos.Y - 1);
            Vector2 bottom = new Vector2(Pos.X, Pos.Y + 1);

            bool result = false;
            switch (moveDirection)
            {
                case Direction.Left:
                    result = IsDestPositionCanMove(left, in boxes, in walls);
                    break;
                case Direction.Right:
                    result = IsDestPositionCanMove(right, in boxes, in walls);
                    break;
                case Direction.Up:
                    result = IsDestPositionCanMove(top, in boxes, in walls);
                    break;
                case Direction.Down:
                    result = IsDestPositionCanMove(bottom, in boxes, in walls);
                    break;
            }

            return result;
        }

        private bool IsDestPositionCanMove(Vector2 there, in Box[] boxes, in Wall[] walls) 
        {
            if (there.X < Game.MIN_X + Game.OFFSET_X || there.X > Game.MAX_X - Game.OFFSET_X 
                || there.Y < Game.MIN_Y + Game.OFFSET_Y || there.Y > Game.MAX_Y - Game.OFFSET_Y)
            {
                return false;
            }
            for (int i = 0; i < boxes.Length; ++i)
            {
                if (boxes[i].Pos == there)
                {
                    return false;
                }
            }
            for (int i = 0; i < walls.Length; ++i)
            {
                if (walls[i].Pos == there)
                {
                    return false;
                }
            }

            return true;
        }
        
    }
}
