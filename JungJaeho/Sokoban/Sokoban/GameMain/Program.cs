using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Sokoban
{
    enum Direction // 방향을 저장하는 타입
    {
        None,
        Left,
        Right,
        Up,
        Down
    }


    public class Sokoban
    {

        static void Main()
        {
            // 초기 세팅
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.ResetColor(); // 컬러를 초기화 하는 것
            Console.CursorVisible = false; // 커서를 숨기기
            Console.Title = "ㅇㅇ"; // 타이틀을 설정한다.
            Console.BackgroundColor = ConsoleColor.Black; // 배경색을 설정한다.
            Console.ForegroundColor = ConsoleColor.Yellow; // 글꼴색을 설정한다.
            Console.Clear(); // 출력된 내용을 지운다.

            Sound clearSound = new Sound(@"C:\Users\jjho9\OneDrive\바탕 화면\Sokoban-Project\JungJaeho\Sokoban\Sounds\clearSound.wav");
            Sound teleportSound = new Sound(@"C:\Users\jjho9\OneDrive\바탕 화면\Sokoban-Project\JungJaeho\Sokoban\Sounds\tpSound.wav");
            

            Recorder recorder = new Recorder(Game.RECORD_COUNT/*, Game.REWIND_INTERVAL*/);
            Renderer renderer = new Renderer();
            //Teleporter tp = new Teleporter(new Vector2(5, 5), new Vector2(5, 9), "T", ConsoleColor.Blue);
            Teleporter tp2 = new Teleporter(new Vector2(5, Game.MIN_Y+Game.OFFSET_Y), new Vector2(5, Game.MAX_Y-Game.OFFSET_Y), "@", ConsoleColor.Cyan);

            Player player = new Player(new Vector2(2, 3), 0, ConsoleColor.White);

            // 박스 위치를 저장하기 위한 변수
            Box[] boxes = new Box[3]
            {
                new Box(new Vector2(3, 5), ConsoleColor.DarkYellow),
                new Box(new Vector2(7, 4), ConsoleColor.DarkYellow),
                new Box(new Vector2(4, 4), ConsoleColor.DarkYellow)
            };

            // 박스가 골 위에 있는지를 저장하기 위한 변수
            bool[] isBoxOnGoal = new bool[Game.BOX_COUNT];

            // 벽 위치를 저장하기 위한 변수
            Wall[] walls = new Wall[Game.WALL_COUNT]
           { 
                new Wall(new Vector2(7, 7),  ConsoleColor.Red),
                new Wall(new Vector2(7, 10), ConsoleColor.Red)
            };

            Goal[] goals = new Goal[Game.GOAL_COUNT]
            {
                new Goal(new Vector2(9, 9), ConsoleColor.Yellow),
                new Goal(new Vector2(5, 6), ConsoleColor.Yellow),
                new Goal(new Vector2(3, 3), ConsoleColor.Yellow)
            };


            ConsoleKey key = ConsoleKey.NoName;
            // 게임 루프 구성
            while (true)
            {
                Render();

                // 되감기상태가 아닐 때만 키입력 받음
                if (recorder.IsRewinding == false)
                {
                    key = Console.ReadKey().Key;
                }

                if (key == ConsoleKey.Spacebar)
                {
                    if(recorder.StartRewinding())
                    {
                        teleportSound.PlaySound();
                    }
                    else
                    {
                        key = ConsoleKey.NoName;
                        continue;
                    }
                    key = ConsoleKey.NoName;
                }

                recorder.Update(ref player, ref boxes);

                if (recorder.IsRewinding == true)
                    continue;

                // player, boxes 기록
                recorder.Record(ref player, boxes);

                Update(key);

                int boxOnGoalCount = CountBoxOnGoal(boxes, goals, ref isBoxOnGoal);

                // 모든 골 지점에 박스가 올라와 있다면?
                if (boxOnGoalCount == Game.GOAL_COUNT)
                {
                    clearSound.PlaySound();
                    Console.Clear();
                    Console.WriteLine("축하합니다. 클리어 하셨습니다.");
                    Thread.Sleep(2500);
                    break;
                }

            }
            // 프레임을 그립니다.
            void Render()
            {
                // 이전 프레임을 지운다.
                Console.Clear();

                // 플레이어 이동경로
                recorder.TrackingPlayer(renderer, "ё");

                // 플레이어 왼쪽날개
                if (player.Pos.X - 1 >= Game.MIN_X + Game.OFFSET_X)
                    renderer.Render(new Vector2(player.Pos.X - 1, player.Pos.Y), "ε", player.Color);
                // 플레이어 오른쪽 날개
                if (player.Pos.X + 1 <= Game.MAX_X - Game.OFFSET_Y)
                    renderer.Render(new Vector2(player.Pos.X + 1, player.Pos.Y), "з", player.Color);

                // 테두리를 그려준다
                for (int i = Game.MIN_X; i <= Game.MAX_X; ++i)
                {
                    for(int j= Game.MIN_Y; j <= Game.MAX_Y; ++j)
                    {
                        if (i == Game.MIN_X || i == Game.MAX_X || j == Game.MIN_Y || j == Game.MAX_Y)
                        {
                            Random rand = new Random();
                            ConsoleColor col = recorder.IsRewinding ? (ConsoleColor)rand.Next(1, 15) : ConsoleColor.Red;
                            renderer.Render(new Vector2(i, j), "∏", col);
                            renderer.Render(new Vector2(i, j), "∏", col);
                        }
                    }
                }

                // 골을 그린다.
                for (int i = 0; i < Game.GOAL_COUNT; ++i)
                {

                    renderer.Render(goals[i].Pos, "⚐", goals[i].Color);
                }

                // Teleport
                //renderer.Render(tp.Pos1, tp.Icon, tp.Color);
                //renderer.Render(tp.Pos2, tp.Icon, tp.Color);
                
                renderer.Render(tp2.Pos1, tp2.Icon, tp2.Color);
                renderer.Render(tp2.Pos2, tp2.Icon, tp2.Color);

                // 플레이어 몸

                renderer.Render(player.Pos, "ё", player.Color);






                // 박스를 그린다.
                for (int boxId = 0; boxId < Game.BOX_COUNT; ++boxId)
                {
                    string boxShape = isBoxOnGoal[boxId] ? "▣" : "▩";
                    renderer.Render(boxes[boxId].Pos, boxShape, boxes[boxId].Color);
                }

                // 벽을 그린다.
                for (int wallId = 0; wallId < Game.WALL_COUNT; ++wallId)
                {
                    renderer.Render(walls[wallId].Pos, "■", walls[wallId].Color);
                }

                // Draw Ui
                if (recorder.IsRewinding)
                {
                    string ui = $"| Rewinding... : {recorder.Index:D2} |";
                    int startPos = Game.MAX_X / 2 - (ui.Length / 2);
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < ui.Length; ++i)
                    {
                        builder.Append("-");
                    }
                    renderer.Render(new Vector2(startPos, 0), ui, ConsoleColor.White);
                    renderer.Render(new Vector2(startPos, 1), builder.ToString(), ConsoleColor.White);
                }

                renderer.Render(new Vector2(Game.MAX_X + 2, Game.MIN_Y),     "-------------------", ConsoleColor.White);
                renderer.Render(new Vector2(Game.MAX_X + 2, Game.MIN_Y + 1), "  Space : 되감기   ", ConsoleColor.White);
                renderer.Render(new Vector2(Game.MAX_X + 2, Game.MIN_Y + 2), "-------------------", ConsoleColor.White);

            }

            void Update(ConsoleKey key)
            {
                MovePlayer(key, ref player);

                // 플레이어와 벽의 충돌 처리
                for (int wallId = 0; wallId < Game.WALL_COUNT; ++wallId)
                {
                    if (false == IsCollided(player.Pos, walls[wallId].Pos))
                    {
                        continue;
                    }

                    OnCollision(player.MoveDirection, ref player.Pos, in walls[wallId].Pos);
                    break;
                }


                // 박스 이동 처리
                // 플레이어가 박스를 밀었을 때라는 게 무엇을 의미하는가? => 플레이어가 이동했는데 플레이어의 위치와 박스 위치가 겹쳤다.
                for (int i = 0; i < Game.BOX_COUNT; ++i)
                {
                    if (false == IsCollided(player.Pos, boxes[i].Pos))
                    {
                        continue;
                    }

                    switch (player.MoveDirection)
                    {
                        case Direction.Left:
                            MoveToLeftOfTarget(out boxes[i].Pos, in player.Pos);
                            break;
                        case Direction.Right:
                            MoveToRightOfTarget(out boxes[i].Pos, in player.Pos);
                            break;
                        case Direction.Up:
                            MoveToUpOfTarget(out boxes[i].Pos, in player.Pos);
                            break;
                        case Direction.Down:
                            MoveToDownOfTarget(out boxes[i].Pos, in player.Pos);
                            break;
                        default:
                            ExitWithError($"[Error] 플레이어 방향  : {player.MoveDirection}");
                            break;
                    }

                    player.PushedBoxIndex = i;
                    OnCollision(player.MoveDirection, ref player.Pos, in boxes[i].Pos);
                    break;
                }

                // 박스와 벽의 충돌 처리
                for (int wallId = 0; wallId < Game.WALL_COUNT; ++wallId)
                {
                    if (false == IsCollided(boxes[player.PushedBoxIndex].Pos, walls[wallId].Pos))
                    {
                        continue;
                    }

                    OnCollision(player.MoveDirection, ref boxes[player.PushedBoxIndex].Pos, walls[wallId].Pos);
                    OnCollision(player.MoveDirection, ref player.Pos, boxes[player.PushedBoxIndex].Pos);

                    break;
                }

                // 박스끼리 충돌 처리
                for (int collidedBoxId = 0; collidedBoxId < Game.BOX_COUNT; ++collidedBoxId)
                {
                    // 같은 박스라면 처리할 필요가 X
                    if (player.PushedBoxIndex == collidedBoxId)
                    {
                        continue;
                    }

                    if (false == IsCollided(boxes[player.PushedBoxIndex].Pos, boxes[collidedBoxId].Pos))
                    {
                        continue;
                    }

                    OnCollision(player.MoveDirection, ref boxes[player.PushedBoxIndex].Pos, in boxes[collidedBoxId].Pos);
                    OnCollision(player.MoveDirection, ref player.Pos, in boxes[player.PushedBoxIndex].Pos);
                    break;
                }

                //tp.Update(ref player, in boxes, in walls);
                tp2.Update(ref player, in boxes, in walls);

            }
            
           

            // 플레이어를 이동시킨다.
            void MovePlayer(ConsoleKey key, ref Player player)
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

            void OnCollision(Direction playerMoveDirection, ref Vector2 objPos, in Vector2 collidedObjPos)
            {
                switch(playerMoveDirection)
                {
                    case Direction.Left:
                        MoveToRightOfTarget(out objPos, in collidedObjPos);
                        break;
                    case Direction.Right:
                        MoveToLeftOfTarget(out objPos, in collidedObjPos);
                        break;
                    case Direction.Up:
                        MoveToDownOfTarget(out objPos, in collidedObjPos);
                        break;
                    case Direction.Down:
                        MoveToUpOfTarget(out objPos, in collidedObjPos);
                        break;
                }
            }

            int CountBoxOnGoal(in Box[] boxes, in Goal[] goals, ref bool[] isBoxOnGoal)
            {
                int boxCount = boxes.Length;
                int goalCount = goals.Length;

                int result = 0;
                for(int boxId = 0; boxId < boxCount; ++boxId)
                {
                    isBoxOnGoal[boxId] = false;
                    for(int goalId = 0; goalId < goalCount; ++goalId)
                    {
                        if(IsCollided(boxes[boxId].Pos, goals[goalId].Pos))
                        {
                            ++result;
                            isBoxOnGoal[(boxId)] = true;
                            break;
                        }
                    }
                }
                return result;
            }

            void MoveToLeftOfTarget(out Vector2 pos, in Vector2 target) => pos = new Vector2(Math.Max(Game.MIN_X + Game.OFFSET_X, target.X - 1), target.Y);
            void MoveToRightOfTarget(out Vector2 pos, in Vector2 target) => pos = new Vector2(Math.Min(target.X + 1, Game.MAX_X - Game.OFFSET_X), target.Y);
            void MoveToUpOfTarget(out Vector2 pos, in Vector2 target) => pos = new Vector2(target.X, Math.Max(Game.MIN_Y + Game.OFFSET_Y, target.Y - 1));
            void MoveToDownOfTarget(out Vector2 pos, in Vector2 target) => pos = new Vector2(target.X, Math.Min(target.Y + 1, Game.MAX_Y - Game.OFFSET_Y));

            void ExitWithError(string errorMessage)
            {
                Console.Clear();
                Console.WriteLine(errorMessage);   
                Environment.Exit(1);
            }

            // 두 물체가 충돌했는지 판별합니다.
            bool IsCollided(in Vector2 x1, in Vector2 x2)
            {
                if (x1 == x2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}