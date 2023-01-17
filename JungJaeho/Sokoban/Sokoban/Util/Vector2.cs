using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    struct Vector2
    {
        public Vector2(int x, int y) { X = x; Y = y; }
        public int X;
        public int Y;

        //public Vector2 Normalizing()
        //{
        //    int size = (int)Math.Sqrt((X * X) + (Y * Y));
        //    Vector2 NormalizedVec = new Vector2();
        //    NormalizedVec.X = X / size;
        //    NormalizedVec.Y = Y / size;
        //    return NormalizedVec;
        //}

        static public int Distance(Vector2 start, Vector2 dest)
        {
            int dist = 5;
            return dist;
        }
   
        #region Operator Overloading
        public static Vector2 operator +(Vector2 x1, Vector2 x2)
        {
            return new Vector2(x1.X + x2.X , x1.Y + x2.Y);
        }
        public static Vector2 operator -(Vector2 x1, Vector2 x2)
        {
            return new Vector2(x1.X - x2.X, x1.Y - x2.Y);
        }
        public static Vector2 operator *(Vector2 x1, Vector2 x2)
        {
            return new Vector2(x1.X * x2.X, x1.Y * x2.Y);
        }
        public static Vector2 operator /(Vector2 x1, Vector2 x2)
        {
            return new Vector2(x1.X / x2.X, x1.Y / x2.Y);
        }
        public static bool operator==(Vector2 x1, Vector2 x2)
        {
            return (x1.X == x2.X && x1.Y == x2.Y);
        }
        public static bool operator!=(Vector2 x1, Vector2 x2)
        {
            return !(x1.X == x2.X && x1.Y == x2.Y);
        }
        #endregion
    }
}
