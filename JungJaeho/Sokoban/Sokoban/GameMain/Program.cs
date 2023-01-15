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
        // 기호 상수 정의
        public const int GOAL_COUNT = 3;
        public const int BOX_COUNT = GOAL_COUNT;
        public const int WALL_COUNT = 2;

        public const int MIN_X = 0;
        public const int MIN_Y = 2;
        public const int MAX_X = 26;
        public const int MAX_Y = 13;
        public const int OFFSET_X = 1;
        public const int OFFSET_Y = 1;

        // Recorder Setting
        public const int RECORD_COUNT = 100;
        public const int REWIND_INTERVAL = 100;

        static void Main()
        {
            // 초기 세팅
            Console.ResetColor(); // 컬러를 초기화 하는 것
            Console.CursorVisible = false; // 커서를 숨기기
            Console.Title = "홍성재의 파이어펀치"; // 타이틀을 설정한다.
            Console.BackgroundColor = ConsoleColor.Black; // 배경색을 설정한다.
            Console.ForegroundColor = ConsoleColor.Yellow; // 글꼴색을 설정한다.
            Console.Clear(); // 출력된 내용을 지운다.


            Recorder recorder = new Recorder(RECORD_COUNT, REWIND_INTERVAL);
            Renderer renderer = new Renderer();
            Teleporter tp = new Teleporter(new Vector2(5, 5), new Vector2(7, 9), "T");
            Player player = new Player(new Vector2(2, 3), 0, ConsoleColor.Yellow);

            // 박스 위치를 저장하기 위한 변수
            Box[] boxes = new Box[3]
            {
                new Box(new Vector2(3, 5), ConsoleColor.DarkCyan),
                new Box(new Vector2(7, 4), ConsoleColor.DarkCyan),
                new Box(new Vector2(4, 4), ConsoleColor.DarkCyan)
            };

            // 박스가 골 위에 있는지를 저장하기 위한 변수
            bool[] isBoxOnGoal = new bool[BOX_COUNT];

            // 벽 위치를 저장하기 위한 변수
            Wall[] walls = new Wall[WALL_COUNT]
           { 
                new Wall(new Vector2(7, 7), ConsoleColor.Red),
                new Wall(new Vector2(7, 10), ConsoleColor.Red)
            };

            Goal[] goals = new Goal[GOAL_COUNT]
            {
                new Goal(new Vector2(9, 9), ConsoleColor.White),
                new Goal(new Vector2(5, 6), ConsoleColor.White),
                new Goal(new Vector2(3, 3), ConsoleColor.White)
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

                recorder.Update(key, ref player, ref boxes);

                if (recorder.IsRewinding == true)
                    continue;

                // player, boxes 기록
                recorder.Record(player, boxes);

                Update(key);
                // 박스와 골의 처리
                int boxOnGoalCount = 0;

                // 골 지점에 박스에 존재하냐?
                for (int boxId = 0; boxId < BOX_COUNT; ++boxId) // 모든 골 지점에 대해서
                {
                    // 현재 박스가 골 위에 올라와 있는지 체크한다.
                    boxes[boxId].IsOnGoal = false; // 없을 가능성이 높기 때문에 false로 초기화 한다.

                    for (int goalId = 0; goalId < GOAL_COUNT; ++goalId) // 모든 박스에 대해서
                    {
                        // 박스가 골 지점 위에 있는지 확인한다.
                        if (IsCollided(boxes[boxId].Pos, goals[goalId].Pos))
                        {
                            ++boxOnGoalCount;

                            boxes[boxId].IsOnGoal = true; // 박스가 골 위에 있다는 사실을 저장해둔다.

                            break;
                        }
                    }
                }


                // 모든 골 지점에 박스가 올라와 있다면?
                if (boxOnGoalCount == GOAL_COUNT)
                {
                    Console.Clear();
                    Console.WriteLine("축하합니다. 클리어 하셨습니다.");

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
                if (player.Pos.X - 1 >= MIN_X + OFFSET_X)
                    renderer.Render(new Vector2(player.Pos.X - 1, player.Pos.Y), "ε", player.Color);
                // 플레이어 오른쪽 날개
                if (player.Pos.X - 1 >= MIN_X + OFFSET_X)
                    renderer.Render(new Vector2(player.Pos.X - 1, player.Pos.Y), "ε", player.Color);

                // 테두리를 그려준다.
                for (int i = MIN_X; i <= MAX_X; ++i)
                {
                    Random rand = new Random();
                    ConsoleColor col = recorder.IsRewinding ? (ConsoleColor)rand.Next(1, 15) : ConsoleColor.White;
                    renderer.Render(new Vector2(i, MIN_Y), "■", col);
                    renderer.Render(new Vector2(i, MAX_Y), "■", col);
                }
                for (int i = MIN_Y; i <= MAX_Y; ++i)
                {
                    Random rand = new Random();
                    ConsoleColor col = recorder.IsRewinding ? (ConsoleColor)rand.Next(1, 15) : ConsoleColor.White;
                    renderer.Render(new Vector2(MIN_X, i), "■",col);
                    renderer.Render(new Vector2(MAX_X, i), "■", col);
                }

                // 골을 그린다.
                for (int i = 0; i < GOAL_COUNT; ++i)
                {
                    renderer.Render(goals[i].Pos, "G", goals[i].Color);
                }

                // Teleport
                renderer.Render(tp.Pos1, tp.Icon, ConsoleColor.Blue);
                renderer.Render(tp.Pos2, tp.Icon, ConsoleColor.Blue);

                // 플레이어 몸
                renderer.Render(player.Pos, "ё", player.Color);


                if (player.Pos.X + 1 <= MAX_X - OFFSET_Y)
                    renderer.Render(new Vector2(player.Pos.X + 1, player.Pos.Y), "з", player.Color);



                // 박스를 그린다.
                for (int boxId = 0; boxId < BOX_COUNT; ++boxId)
                {
                    string boxShape = isBoxOnGoal[boxId] ? "O" : "B";
                    renderer.Render(boxes[boxId].Pos, boxShape, boxes[boxId].Color);
                }

                // 벽을 그린다.
                for (int wallId = 0; wallId < WALL_COUNT; ++wallId)
                {
                    renderer.Render(walls[wallId].Pos, "W", walls[wallId].Color);
                }
                if (recorder.IsRewinding)
                {
                    string ui = $"| Rewinding... : {recorder.Index:D2} |";
                    int startPos = MAX_X / 2 - (ui.Length / 2);
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < ui.Length; ++i)
                    {
                        builder.Append("-");
                    }
                    renderer.Render(new Vector2(startPos, 0), ui, ConsoleColor.White);
                    renderer.Render(new Vector2(startPos, 1), builder.ToString(), ConsoleColor.White);
                }

            }

            void Update(ConsoleKey key)
            {
                MovePlayer(key, ref player);

                // 플레이어와 벽의 충돌 처리
                for (int wallId = 0; wallId < WALL_COUNT; ++wallId)
                {
                    if (false == IsCollided(player.Pos, walls[wallId].Pos))
                    {
                        continue;
                    }

                    switch (player.MoveDirection)
                    {
                        case Direction.Left:
                            player.Pos.X = walls[wallId].Pos.X + 1;
                            break;
                        case Direction.Right:
                            player.Pos.X = walls[wallId].Pos.X - 1;
                            break;
                        case Direction.Up:
                            player.Pos.Y = walls[wallId].Pos.Y + 1;
                            break;
                        case Direction.Down:
                            player.Pos.Y = walls[wallId].Pos.Y - 1;
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine($"[Error] 플레이어 이동 방향 데이터가 오류입니다. : {player.MoveDirection}");

                            return;
                    }

                    break;
                }


                // 박스 이동 처리
                // 플레이어가 박스를 밀었을 때라는 게 무엇을 의미하는가? => 플레이어가 이동했는데 플레이어의 위치와 박스 위치가 겹쳤다.
                for (int i = 0; i < BOX_COUNT; ++i)
                {
                    if (false == IsCollided(player.Pos, boxes[i].Pos))
                    {
                        continue;
                    }

                    switch (player.MoveDirection)
                    {
                        case Direction.Left:
                            boxes[i].Pos.X = Math.Clamp(boxes[i].Pos.X - 1, MIN_X + OFFSET_X, MAX_X - OFFSET_X);
                            player.Pos.X = boxes[i].Pos.X + 1;
                            break;
                        case Direction.Right:
                            boxes[i].Pos.X = Math.Clamp(boxes[i].Pos.X + 1, MIN_X + OFFSET_X, MAX_X - OFFSET_X);
                            player.Pos.X = boxes[i].Pos.X - 1;
                            break;
                        case Direction.Up:
                            boxes[i].Pos.Y = Math.Clamp(boxes[i].Pos.Y - 1, MIN_Y + OFFSET_Y, MAX_Y - OFFSET_Y);
                            player.Pos.Y = boxes[i].Pos.Y + 1;
                            break;
                        case Direction.Down:
                            boxes[i].Pos.Y = Math.Clamp(boxes[i].Pos.Y + 1, MIN_Y + OFFSET_Y, MAX_Y - OFFSET_Y);
                            player.Pos.Y = boxes[i].Pos.Y - 1;
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine($"[Error] 플레이어 이동 방향 데이터가 오류입니다. : {player.MoveDirection}");

                            return;
                    }

                    player.PushedBoxIndex = i;
                    break;
                }

                // 박스와 벽의 충돌 처리
                for (int wallId = 0; wallId < WALL_COUNT; ++wallId)
                {
                    if (false == IsCollided(boxes[player.PushedBoxIndex].Pos, walls[wallId].Pos))
                    {
                        continue;
                    }

                    switch (player.MoveDirection)
                    {
                        case Direction.Left:
                            boxes[player.PushedBoxIndex].Pos.X = walls[wallId].Pos.X + 1;
                            player.Pos.X = boxes[player.PushedBoxIndex].Pos.X + 1;
                            break;
                        case Direction.Right:
                            boxes[player.PushedBoxIndex].Pos.X = walls[wallId].Pos.X - 1;
                            player.Pos.X = boxes[player.PushedBoxIndex].Pos.X - 1;
                            break;
                        case Direction.Up:
                            boxes[player.PushedBoxIndex].Pos.Y = walls[wallId].Pos.Y + 1;
                            player.Pos.Y = boxes[player.PushedBoxIndex].Pos.Y + 1;
                            break;
                        case Direction.Down:
                            boxes[player.PushedBoxIndex].Pos.Y = walls[wallId].Pos.Y - 1;
                            player.Pos.Y = boxes[player.PushedBoxIndex].Pos.Y - 1;
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine($"[Error] 플레이어 이동 방향 데이터가 오류입니다. : {player.MoveDirection}");

                            return;
                    }

                    break;
                }

                // 박스끼리 충돌 처리
                for (int collidedBoxId = 0; collidedBoxId < BOX_COUNT; ++collidedBoxId)
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

                    switch (player.MoveDirection)
                    {
                        case Direction.Left:
                            boxes[player.PushedBoxIndex].Pos.X = boxes[collidedBoxId].Pos.X + 1;
                            player.Pos.X = boxes[player.PushedBoxIndex].Pos.X + 1;

                            break;
                        case Direction.Right:
                            boxes[player.PushedBoxIndex].Pos.X = boxes[collidedBoxId].Pos.X - 1;
                            player.Pos.X = boxes[player.PushedBoxIndex].Pos.X - 1;

                            break;
                        case Direction.Up:
                            boxes[player.PushedBoxIndex].Pos.Y = boxes[collidedBoxId].Pos.Y + 1;
                            player.Pos.Y = boxes[player.PushedBoxIndex].Pos.Y + 1;

                            break;
                        case Direction.Down:
                            boxes[player.PushedBoxIndex].Pos.Y = boxes[collidedBoxId].Pos.Y - 1;
                            player.Pos.Y = boxes[player.PushedBoxIndex].Pos.Y - 1;

                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine($"[Error] 플레이어 이동 방향 데이터가 오류입니다. : {player.MoveDirection}");

                            return;
                    }

                    break;
                }

                tp.Update(ref player, in boxes, in walls);

                for(int i = 0; i < BOX_COUNT; ++i)
                {
                    isBoxOnGoal[i] = false;
                    for(int j = 0; j < GOAL_COUNT; ++j)
                    {
                        if(IsCollided(boxes[i].Pos, goals[j].Pos))
                        {
                            isBoxOnGoal[i] = true;
                            break;
                        }
                    }
                }

            }
            
           

            // 플레이어를 이동시킨다.
            void MovePlayer(ConsoleKey key, ref Player player)
            {
                switch (key)
                {
                    case ConsoleKey.LeftArrow:
                        player.Pos.X = Math.Max(MIN_X + OFFSET_X, player.Pos.X - 1);
                        player.MoveDirection = Direction.Left;
                        break;
                    case ConsoleKey.RightArrow:
                        player.Pos.X = Math.Min(player.Pos.X + 1, MAX_X - OFFSET_X);
                        player.MoveDirection = Direction.Right;
                        break;
                    case ConsoleKey.UpArrow:
                        player.Pos.Y = Math.Max(MIN_Y + OFFSET_Y, player.Pos.Y - 1);
                        player.MoveDirection = Direction.Up;
                        break;
                    case ConsoleKey.DownArrow:
                        player.Pos.Y = Math.Min(player.Pos.Y + 1, MAX_Y - OFFSET_Y);
                        player.MoveDirection = Direction.Down;
                        break;
                }
            }
            
            

            // 두 물체가 충돌했는지 판별합니다.
            bool IsCollided(Vector2 x1, Vector2 x2)
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