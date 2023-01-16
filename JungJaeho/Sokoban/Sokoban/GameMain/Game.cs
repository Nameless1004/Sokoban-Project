using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    internal class Game
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
    }
}
