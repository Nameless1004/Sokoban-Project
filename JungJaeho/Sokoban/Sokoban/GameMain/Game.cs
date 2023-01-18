using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    public static class Game
    {
        // 기호 상수 정의
        public const int GOAL_COUNT = 3;
        public const int BOX_COUNT = GOAL_COUNT;
        public const int WALL_COUNT = 12;

        public const int MIN_X = 0;
        public const int MIN_Y = 2;
        public const int MAX_X = 26;
        public const int MAX_Y = 13;
        public const int OFFSET_X = 1;
        public const int OFFSET_Y = 1;

        // Recorder Setting
        public const int RECORD_COUNT = 15;
        public const int REWIND_INTERVAL = 100;

        public static Random random = new Random();

        public static void Initialize()
        {
            // 초기 세팅
            Console.ResetColor(); // 컬러를 초기화 하는 것
            Console.CursorVisible = false; // 커서를 숨기기
            Console.Title = "ㅇㅇ"; // 타이틀을 설정한다.
            Console.BackgroundColor = ConsoleColor.Black; // 배경색을 설정한다.
            Console.ForegroundColor = ConsoleColor.Yellow; // 글꼴색을 설정한다.
            Console.Clear(); // 출력된 내용을 지운다.
        }

        // 플레이어를 이동시킨다.
        public static void MovePlayer(ConsoleKey key, Player player)
        {
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    MoveToLeftOfTarget(out player.Pos, in player.Pos);
                    player.MoveDirection = Direction.Left;
                    break;
                case ConsoleKey.RightArrow:
                    MoveToRightOfTarget(out player.Pos, in player.Pos);
                    player.MoveDirection = Direction.Right;
                    break;
                case ConsoleKey.UpArrow:
                    MoveToUpOfTarget(out player.Pos, in player.Pos);
                    player.MoveDirection = Direction.Up;
                    break;
                case ConsoleKey.DownArrow:
                    MoveToDownOfTarget(out player.Pos, in player.Pos);
                    player.MoveDirection = Direction.Down;
                    break;
            }
        }


        public static void PushOut(Direction dir, ref Vector2 objPos, in Vector2 collidedObj)
        {
            switch (dir)
            {
                case Direction.Left:
                    MoveToRightOfTarget(out objPos, in collidedObj);
                    break;
                case Direction.Right:
                    MoveToLeftOfTarget(out objPos, in collidedObj);
                    break;
                case Direction.Up:
                    MoveToDownOfTarget(out objPos, in collidedObj);
                    break;
                case Direction.Down:
                    MoveToUpOfTarget(out objPos, in collidedObj);
                    break;
            }
        }

        public static void MoveBox(Direction dir, ref Vector2 boxPos, in Vector2 PlayerPos)
        {
            switch (dir)
            {
                case Direction.Left:
                    MoveToLeftOfTarget(out boxPos, in PlayerPos);
                    break;
                case Direction.Right:
                    MoveToRightOfTarget(out boxPos, in PlayerPos);
                    break;
                case Direction.Up:
                    MoveToUpOfTarget(out boxPos, in PlayerPos);
                    break;
                case Direction.Down:
                    MoveToDownOfTarget(out boxPos, in PlayerPos);
                    break;
            }
        }

        

        public static int CountBoxOnGoal(Box[] boxes, Goal[] goals)
        {
            int boxCount = boxes.Length;
            int goalCount = goals.Length;

            int result = 0;
            for (int boxId = 0; boxId < boxCount; ++boxId)
            {
                boxes[boxId].IsOnGoal = false;
                for (int goalId = 0; goalId < goalCount; ++goalId)
                {
                    if (CollisionManager.Instance.IsCollided(boxes[boxId].Pos, goals[goalId].Pos))
                    {
                        ++result;
                        boxes[boxId].IsOnGoal = true;
                        break;
                    }
                }
            }
            return result;
        }

        public static void MoveToLeftOfTarget(out Vector2 pos, in Vector2 target) => pos = new Vector2(Math.Max(Game.MIN_X + Game.OFFSET_X, target.X - 1), target.Y);
        public static void MoveToRightOfTarget(out Vector2 pos, in Vector2 target) => pos = new Vector2(Math.Min(target.X + 1, Game.MAX_X - Game.OFFSET_X), target.Y);
        public static void MoveToUpOfTarget(out Vector2 pos, in Vector2 target) => pos = new Vector2(target.X, Math.Max(Game.MIN_Y + Game.OFFSET_Y, target.Y - 1));
        public static void MoveToDownOfTarget(out Vector2 pos, in Vector2 target) => pos = new Vector2(target.X, Math.Min(target.Y + 1, Game.MAX_Y - Game.OFFSET_Y));

        public static void ExitWithError(string errorMessage)
        {
            Console.Clear();
            Console.WriteLine(errorMessage);
            Environment.Exit(1);
        }



    }
}
