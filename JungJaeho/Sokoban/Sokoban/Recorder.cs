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
        /// 한번씩 뒤로 되돌려주는 함수 입니다.
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
                _playerMoveHistory[_index] = player;
                _boxesMoveHistory[_index] = (Box[])boxes.Clone();
                _index++;
            }
            else
            {
                for (int i = 0; i < _recordCount-1; ++i)
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
                _playerMoveHistory[_index - 1] = player;
                _boxesMoveHistory[_index - 1] = (Box[])boxes.Clone();
            }
        }

        /// <summary>
        /// Test함수입니다. 플레이어 이동한곳 표시
        /// </summary>
        public void TrackingPlayer(string playerIcon)
        {
            ConsoleColor prev = Console.ForegroundColor;
           
            for (int i = 0; i < _index; ++i)
            {
                Console.ForegroundColor = (ConsoleColor)(1+i%14);
                //Console.ForegroundColor = ConsoleColor.Gray;
                Console.SetCursorPosition(_playerMoveHistory[i].X, _playerMoveHistory[i].Y);
                Console.Write(playerIcon);
            }
            Console.ForegroundColor = prev;
        }
    }
}
