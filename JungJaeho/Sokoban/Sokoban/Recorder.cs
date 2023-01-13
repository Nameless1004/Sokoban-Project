using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    class Recorder
    {
        public Recorder(int recordCount, int boxCount)
        {
            _recordCount = recordCount;
            _playerMoveHistory = new Player[_recordCount];
            _boxesMoveHistory = new Box[_recordCount][];
        }
        private int _recordCount;
        private int _index;
        public int Index { get { return _index; } }

        public Player[] _playerMoveHistory;
        public Box[][] _boxesMoveHistory;

        public bool RollBackOneTime(ref Player player, ref Box[] boxes)
        {
            if (_index > 0)
            {
               player = _playerMoveHistory[_index - 1];
               boxes = (Box[])_boxesMoveHistory[_index - 1].Clone();
                --_index;
                return true;
            }
            return false;
        }
        public void RollBack(ref Player player, ref Box[] boxes)
        {
            for (int i = _index-1; i >= 0; --i)
            {
                player = _playerMoveHistory[i];
                boxes = (Box[])_boxesMoveHistory[i].Clone();
            }
            _index = 0;
        }
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
                // 2 3 1
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
    }
}
