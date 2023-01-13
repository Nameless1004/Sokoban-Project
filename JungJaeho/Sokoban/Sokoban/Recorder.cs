using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    class Recorder
    {
        public Recorder(int recordCount)
        {
            _recordCount = recordCount;
            _playerMoveHistory = new Player[_recordCount];
            _boxesMoveHistory  = new Box[_recordCount][];
        }
        private int _recordCount;
        private int _index;
        public int Index { get { return _index; } }

        public Player[] _playerMoveHistory;
        public Box[][] _boxesMoveHistory;

        /// <summary>
        /// 한번씩 뒤로 되돌려주는 함수 입니다.
        /// </summary>
        /// <param name="player">플레이어</param>
        /// <param name="boxes">박스들</param>
        /// <returns></returns>
        public bool UndoAll(ref Player player, ref Box[] boxes)
        {
            if (_index <= 0)
            {
                return false;
            }
            player = _playerMoveHistory[_index - 1];
            boxes = (Box[])_boxesMoveHistory[_index - 1].Clone();
            --_index;
            return true;
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
        public void TrackingPlayer()
        {
            ConsoleColor prev = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Gray;
            for (int i = 0; i < _index; ++i)
            {
                Console.SetCursorPosition(_playerMoveHistory[i].X, _playerMoveHistory[i].Y);
                Console.Write("◆");
            }
            Console.ForegroundColor = prev;
        }
    }
}
