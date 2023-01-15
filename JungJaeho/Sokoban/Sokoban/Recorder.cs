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
            Vector2 Pos;
            Direction direction;
            int PushedBoxIndex;
        }
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="recordCount">최대 기록 수</param>
        /// <param name="rewindingSpeed">되감기 간격(ms)</param>
        public Recorder(int recordCount, int rewindInterval)
        {
            _recordCount = recordCount;
            _playerMoveHistory = new Player[_recordCount];
            _boxesMoveHistory  = new Box[_recordCount][];
            _isRewinding = false;
            _rewindInterval = rewindInterval;
        }

        private int         _recordCount;
        private int         _index;
        private bool        _isRewinding;
        private int         _rewindInterval;
        private Player[]    _playerMoveHistory;
        private Box[][]     _boxesMoveHistory;

        #region Properties
        public int      Index { get { return _index; } }
        public bool     IsRewinding { get { return _isRewinding; } set { _isRewinding = value; } }
        public int      RewindInterval { get { return _rewindInterval; } }
        #endregion

        
        public void Update(ConsoleKey key, ref Player player, ref Box[] boxes)
        {
            if (key == ConsoleKey.Spacebar)
            {
                _isRewinding = true;
            }

            if (_isRewinding == true)
            {
                Rewind(ref player, ref boxes);
                Thread.Sleep(RewindInterval);
            }
        }

        /// <summary>
        /// 되감기해주는 함수
        /// </summary>
        /// <param name="player">플레이어</param>
        /// <param name="boxes">박스들</param>
        /// <returns></returns>
        public void Rewind(ref Player player, ref Box[] boxes)
        {
            if (_index <= 0)
            {
                IsRewinding = false;
                return;
            }
            player = _playerMoveHistory[_index - 1];
            boxes = (Box[])_boxesMoveHistory[_index - 1].Clone();
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
                RecordBoxesMove(boxes, _index);
                _index++;
            }
            else
            {
                //   Record Index
                //    1 2 3
                // => 2 3 4
                // => 3 4 5
                MoveForwardElements(player, boxes, _recordCount);
                RecordPlayerMove(player, _index - 1);
                RecordBoxesMove(boxes, _index - 1);
                //_playerMoveHistory[_index - 1] = player;
                //_boxesMoveHistory[_index - 1] = (Box[])boxes.Clone();
            }
        }

        private void RecordPlayerMove(Player player, int index)
        {
            _playerMoveHistory[_index] = player;
        }

        private void RecordBoxesMove(Box[] boxes, int index)
        {
            _boxesMoveHistory[_index] = (Box[])boxes.Clone();
        }

        private void MoveForwardElements(Player player, Box[] boxes, int recordCount)
        {
            for (int i = 0; i < recordCount - 1; ++i)
            {
                //Player
                Player temp = _playerMoveHistory[i];
                _playerMoveHistory[i] = _playerMoveHistory[i + 1];
                _playerMoveHistory[i + 1] = temp;

                // Box
                Box[] temp2 = (Box[])_boxesMoveHistory[i].Clone();
                _boxesMoveHistory[i] = (Box[])_boxesMoveHistory[i + 1].Clone();
                _boxesMoveHistory[i + 1] = (Box[])temp2.Clone();
            }
        }
        /// <summary>
        /// Test함수입니다. 플레이어 이동한곳 표시
        /// </summary>
        public void TrackingPlayer(Renderer renderer, string playerIcon)
        {
            ConsoleColor prev = Console.ForegroundColor;
           
            for (int i = 0; i < _index; ++i)
            {
                ConsoleColor color = (ConsoleColor)(1+i%14);
                //Console.ForegroundColor = ConsoleColor.Gray;
                renderer.Render(_playerMoveHistory[i].Pos, playerIcon, color);
            }
        }
    }
}
