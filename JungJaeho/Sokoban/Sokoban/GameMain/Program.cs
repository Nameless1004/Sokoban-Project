using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using System.Text;
namespace Sokoban
{
    public enum Direction // 방향을 저장하는 타입
    {
        None,
        Left,
        Right,
        Up,
        Down
    }


    public class Sokoban
    {
        delegate void CollisionCallback(Direction dir, ref Vector2 pos, in Vector2 collideObjPos);

        static void Main()
        {
            Game.Initialize();
            
            SoundManager.Instance.AddSound("end", @"..\..\..\Sounds\clearSound.wav");
            SoundManager.Instance.AddSound("rewind", @"..\..\..\Sounds\rewindSound.wav");

            Recorder recorder = new Recorder(5, Game.REWIND_INTERVAL);
            Renderer renderer = new Renderer();

            Teleporter tp = new Teleporter(new Vector2(5, Game.MIN_Y + Game.OFFSET_Y + 2), new Vector2(25, Game.MAX_Y - Game.OFFSET_Y), "@", ConsoleColor.Cyan);
            Player player = new Player(new Vector2(2, 3), 0, ConsoleColor.White);

            Box[] boxes = new Box[3]
            {
                new Box(new Vector2(13, 5), ConsoleColor.DarkYellow),
                new Box(new Vector2(13, 8), ConsoleColor.DarkYellow),
                new Box(new Vector2(4, 4), ConsoleColor.DarkYellow)
            };

            Wall[] walls = new Wall[Game.WALL_COUNT]
           {
                new Wall(new Vector2(7, 7),  ConsoleColor.Red),
                new Wall(new Vector2(20, 10), ConsoleColor.Red),
                new Wall(new Vector2(20, 11), ConsoleColor.Red),
                new Wall(new Vector2(20, 12), ConsoleColor.Red),
                new Wall(new Vector2(21, 10), ConsoleColor.Red),
                new Wall(new Vector2(22, 10), ConsoleColor.Red),
                new Wall(new Vector2(23, 10), ConsoleColor.Red),
                new Wall(new Vector2(24, 10), ConsoleColor.Red),
                new Wall(new Vector2(25, 10), ConsoleColor.Red),

                new Wall(new Vector2(15, 7), ConsoleColor.Red),
                new Wall(new Vector2(12, 5), ConsoleColor.Red),
                new Wall(new Vector2(8, 9), ConsoleColor.Red),
            };

            Goal[] goals = new Goal[Game.GOAL_COUNT]
            {
                new Goal(new Vector2(22, 12), ConsoleColor.Yellow),
                new Goal(new Vector2(5, 6), ConsoleColor.Yellow),
                new Goal(new Vector2(12, 8), ConsoleColor.Yellow)
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
                    if (recorder.StartRewinding())
                    {
                        key = ConsoleKey.NoName;
                        //SoundManager.GetInstance().PlaySound("rewind");
                        //Thread.Sleep(1000);
                    }
                    else
                    {
                        key = ConsoleKey.NoName;
                        continue;
                    }
                }

                recorder.Update(player, boxes);

                if (recorder.IsRewinding == true)
                    continue;

                // player, boxes 기록
                recorder.Record(player, boxes);

                Update(key);

                int boxOnGoalCount = Game.CountBoxOnGoal(boxes, goals);

                // 모든 골 지점에 박스가 올라와 있다면?
                if (boxOnGoalCount == Game.GOAL_COUNT)
                {
                    SoundManager.Instance.PlaySound("end");
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
                {
                    renderer.Render(new Vector2(player.Pos.X - 1, player.Pos.Y), "ε", player.Color);
                }

                // 플레이어 오른쪽 날개
                if (player.Pos.X + 1 <= Game.MAX_X - Game.OFFSET_Y)
                {
                    renderer.Render(new Vector2(player.Pos.X + 1, player.Pos.Y), "з", player.Color);
                }

                // 테두리를 그려준다
                for (int i = Game.MIN_X; i <= Game.MAX_X; ++i)
                {
                    for (int j = Game.MIN_Y; j <= Game.MAX_Y; ++j)
                    {
                        if (i == Game.MIN_X || i == Game.MAX_X || j == Game.MIN_Y || j == Game.MAX_Y)
                        {
                            ConsoleColor col = recorder.IsRewinding ? (ConsoleColor)Game.random.Next(1, 15) : ConsoleColor.Red;
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


                renderer.Render(tp.Pos1, tp.Icon, tp.Color);
                renderer.Render(tp.Pos2, tp.Icon, tp.Color);

                // 플레이어 몸
                renderer.Render(player.Pos, "ё", player.Color);

                // 박스를 그린다.
                for (int boxId = 0; boxId < Game.BOX_COUNT; ++boxId)
                {
                    string boxShape = boxes[boxId].IsOnGoal ? "▣" : "▩";
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

                renderer.Render(new Vector2(Game.MAX_X + 2, Game.MIN_Y), "-------------------", ConsoleColor.White);
                renderer.Render(new Vector2(Game.MAX_X + 2, Game.MIN_Y + 1), "  Space : 되감기   ", ConsoleColor.White);
                renderer.Render(new Vector2(Game.MAX_X + 2, Game.MIN_Y + 2), "-------------------", ConsoleColor.White);
                renderer.Render(new Vector2(Game.MAX_X + 2, Game.MIN_Y + 3), "-------------------", ConsoleColor.White);
                renderer.Render(new Vector2(Game.MAX_X + 2, Game.MIN_Y + 4), "   플레이어 좌표     ", ConsoleColor.White);
                renderer.Render(new Vector2(Game.MAX_X + 2, Game.MIN_Y + 5), $"    X : {player.Pos.X - (Game.MIN_X + Game.OFFSET_X)} Y : {player.Pos.Y - (Game.MIN_Y + Game.OFFSET_Y)}  ", ConsoleColor.White);
                renderer.Render(new Vector2(Game.MAX_X + 2, Game.MIN_Y + 6), "-------------------", ConsoleColor.White);
                Thread.Sleep(1);
            }

            void Update(ConsoleKey key)
            {
                Game.MovePlayer(key, player);

                // 플레이어와 벽의 충돌 처리
                for (int wallId = 0; wallId < Game.WALL_COUNT; ++wallId)
                {
                    if (false == CollisionManager.Instance.IsCollided(player.Pos, walls[wallId].Pos))
                    {
                        continue;
                    }

                    CollisionManager.Instance.OnCollision(() =>
                    {
                        Game.PushOut(player.MoveDirection, ref player.Pos, in walls[wallId].Pos);
                    });
                    break;
                }

                // 박스 이동 처리
                // 플레이어가 박스를 밀었을 때라는 게 무엇을 의미하는가? => 플레이어가 이동했는데 플레이어의 위치와 박스 위치가 겹쳤다.
                for (int i = 0; i < Game.BOX_COUNT; ++i)
                {
                    if (false == CollisionManager.Instance.IsCollided(player.Pos, boxes[i].Pos))
                    {
                        continue;
                    }

                    Game.MoveBox(player.MoveDirection, ref boxes[i].Pos, in player.Pos);
                    player.PushedBoxIndex = i;

                    CollisionManager.Instance.OnCollision(() =>
                    {
                        Game.PushOut(player.MoveDirection, ref player.Pos, in boxes[i].Pos);
                    });
                    break;
                }

                // 박스와 벽의 충돌 처리
                for (int wallId = 0; wallId < Game.WALL_COUNT; ++wallId)
                {
                    if (false == CollisionManager.Instance.IsCollided(boxes[player.PushedBoxIndex].Pos, walls[wallId].Pos))
                    {
                        continue;
                    }

                    CollisionManager.Instance.OnCollision(() =>
                    {
                        Game.PushOut(player.MoveDirection, ref boxes[player.PushedBoxIndex].Pos, in walls[wallId].Pos);
                    });

                    CollisionManager.Instance.OnCollision(() =>
                    {
                        Game.PushOut(player.MoveDirection, ref player.Pos, in boxes[player.PushedBoxIndex].Pos);
                    });

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

                    if (false == CollisionManager.Instance.IsCollided(boxes[player.PushedBoxIndex].Pos, boxes[collidedBoxId].Pos))
                    {
                        continue;
                    }

                    CollisionManager.Instance.OnCollision(() =>
                    {
                        Game.PushOut(player.MoveDirection, ref boxes[player.PushedBoxIndex].Pos, in boxes[collidedBoxId].Pos);
                    });

                    CollisionManager.Instance.OnCollision(() =>
                    {
                        Game.PushOut(player.MoveDirection, ref player.Pos, in boxes[player.PushedBoxIndex].Pos);
                    });
                    break;
                }

                tp.Update(player, in boxes, in walls);
            }


        }
    }
}