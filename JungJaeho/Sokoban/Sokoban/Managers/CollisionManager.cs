using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    public class CollisionManager : Singleton<CollisionManager>
    {
        public void OnCollision(Action action)
        {
            action();
        }
        // 두 물체가 충돌했는지 판별합니다.
        public bool IsCollided(in Vector2 x1, in Vector2 x2)
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
