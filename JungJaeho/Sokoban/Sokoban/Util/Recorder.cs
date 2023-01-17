using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Sokoban;

namespace Sokoban
{
    class Recorder
    {
        struct PlayerInfo
        {
            public Vector2 Pos;
            public Direction MovdDirection;
            public int PushedBoxIndex;
            public ConsoleColor Color;
        }
        struct BoxInfo
        {
            public Vector2 Pos;
            public bool IsOnGoal;
            public ConsoleColor Color;
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="recordCount">최대 기록 수</param>
        /// <param name="rewindingSpeed">되감기 간격(ms)</param>
        public Recorder(int recordCount, int rewindInterval)
        {
            _recordCount = recordCount;
            _playerMoveHistory = new PlayerInfo[_recordCount];
            _boxesMoveHistory = new BoxInfo[_recordCount, Game.BOX_COUNT];
            _isRewinding = false;
            _rewindInterval = rewindInterval;
        }

        private int _recordCount;
        private int _index;
        private bool _isRewinding;
        private int _rewindInterval;
        private PlayerInfo[] _playerMoveHistory;
        private BoxInfo[,] _boxesMoveHistory;

        #region Properties
        public int Index { get { return _index; } }
        public bool IsRewinding { get { return _isRewinding; } set { _isRewinding = value; } }
        public int RewindInterval { get { return _rewindInterval; } }
        #endregion

        public bool StartRewinding()
        {
            if (_index == 0) return false;
            _isRewinding = true;
            return true;
        }

        public void Update(Player player, Box[] boxes)
        {
            if (_isRewinding == false) return;

            Rewind(player, boxes);
            Thread.Sleep(_rewindInterval);
        }

        /// <summary>
        /// 되감기해주는 함수
        /// </summary>
        /// <param name="player">플레이어</param>
        /// <param name="boxes">박스들</param>
        /// <returns></returns>
        public void Rewind(Player player, Box[] boxes)
        {
            if (_index <= 0)
            {
                IsRewinding = false;
                return;
            }
            RecoveryPlayer(player, _index - 1);
            RecoveryBoxes(boxes, _index - 1);
            --_index;
        }


        /// <summary>
        /// 플레이어와 박스의 위치를 기록해주는 함수입니다.
        /// </summary>
        /// <param name="player">플레이어</param>
        /// <param name="boxes">박스들</param>
        public void Record(Player player, Box[] boxes)
        {
            if (_index < _recordCount)
            {
                RecordPlayerMove(player, _index);
                RecordBoxesMove(in boxes, _index);
                _index++;
            }
            else
            {
                MoveForwardElements();
                RecordPlayerMove(player, _index - 1);
                RecordBoxesMove(in boxes, _index - 1);
            }
        }


        /// <summary>
        /// 플레이어  기록
        /// </summary>
        /// <param name="player">플레이어 객체</param>
        /// <param name="index">기록할 인덱스</param>
        private void RecordPlayerMove(Player player, int index)
        {
            PlayerInfo p = ExtractPlayerInfo(in player);
            _playerMoveHistory[index] = p;
        }

        /// <summary>
        /// 박스 기록
        /// </summary>
        /// <param name="boxes">박스담은 배열</param>
        /// <param name="index">기록할 인덱스</param>
        private void RecordBoxesMove(in Box[] boxes, int index)
        {
            for (int i = 0; i < Game.BOX_COUNT; ++i)
            {
                BoxInfo b = ExtractBoxInfo(in boxes[i]);
                _boxesMoveHistory[index, i] = b;
            }
        }

        /// <summary>
        /// 배열 원소를 앞으로 한칸씩 밀어줍니다.
        /// </summary>
        private void MoveForwardElements()
        {
            for (int i = 0; i < _recordCount - 1; ++i)
            {
                //Player
                PlayerInfo temp = _playerMoveHistory[i];
                _playerMoveHistory[i] = _playerMoveHistory[i + 1];
                _playerMoveHistory[i + 1] = temp;

                // Box
                for (int boxIndex = 0; boxIndex < Game.BOX_COUNT; ++boxIndex)
                {
                    BoxInfo temp2 = _boxesMoveHistory[i, boxIndex];
                    _boxesMoveHistory[i, boxIndex] = _boxesMoveHistory[i + 1, boxIndex];
                    _boxesMoveHistory[i + 1, boxIndex] = temp2;
                }
            }
        }

        /// <summary>
        /// 플레이어 객체에서 플레이어 인포 추출
        /// </summary>
        /// <param name="player">플레이어 객체</param>
        /// <returns>플레이저 정보를 담은 구조체</returns>
        private PlayerInfo ExtractPlayerInfo(in Player player)
        {
            PlayerInfo p = new PlayerInfo();
            p.Pos = player.Pos;
            p.MovdDirection = player.MoveDirection;
            p.PushedBoxIndex = player.PushedBoxIndex;
            p.Color = player.Color;
            return p;
        }

        /// <summary>
        /// 박스에서 박스정보 추출
        /// </summary>
        /// <param name="box">박스</param>
        /// <returns>박스정보를 담은 구조체</returns>
        private BoxInfo ExtractBoxInfo(in Box box)
        {
            BoxInfo b = new BoxInfo();
            b.Pos = box.Pos;
            b.IsOnGoal = box.IsOnGoal;
            b.Color = box.Color;

            return b;
        }

        /// <summary>
        /// 플레이어를 HistoryIndex 시점으로 복구합니다.
        /// </summary>
        /// <param name="player">플레이어 객체</param>
        /// <param name="historyIndex">옮길 시점</param>
        private void RecoveryPlayer(Player player, int historyIndex)
        {
            PlayerInfo playerInfo = _playerMoveHistory[historyIndex];

            player.Pos = playerInfo.Pos;
            player.MoveDirection = playerInfo.MovdDirection;
            player.PushedBoxIndex = playerInfo.PushedBoxIndex;
            player.Color = playerInfo.Color;
        }

        /// <summary>
        /// 박스를 HistoryIndex 시점으로 복구합니다.
        /// </summary>
        /// <param name="box">박스</param>
        /// <param name="historyIndex">복구 시점</param>
        /// <param name="boxIndex">박스 번호</param>
        private void RecoveryBox(Box box, int historyIndex, int boxIndex)
        {
            BoxInfo boxInfo = _boxesMoveHistory[historyIndex, boxIndex];

            box.Pos = boxInfo.Pos;
            box.IsOnGoal = boxInfo.IsOnGoal;
            box.Color = boxInfo.Color;
        }

        /// <summary>
        /// 박스들을 HistoryIndex 시점으로 복구합니다.
        /// </summary>
        /// <param name="boxes">박스들을 담은 배열</param>
        /// <param name="historyIndex">복구 시점</param>
        private void RecoveryBoxes(Box[] boxes, int historyIndex)
        {
            for (int boxIndex = 0; boxIndex < Game.BOX_COUNT; ++boxIndex)
            {
                RecoveryBox(boxes[boxIndex], historyIndex, boxIndex);
            }
        }
        /// <summary>
        /// Test함수입니다. 플레이어 이동한곳 표시
        /// </summary>
        public void TrackingPlayer(Renderer renderer, string playerIcon)
        {
            for (int i = 0; i < _index; ++i)
            {
                //ConsoleColor color = (ConsoleColor)(1+i%14);
                ConsoleColor color = ConsoleColor.DarkGray;
                renderer.Render(_playerMoveHistory[i].Pos, playerIcon, color);
            }
        }
    }
}
