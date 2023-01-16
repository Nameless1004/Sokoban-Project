using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    internal class Singleton<T>
    {
        public static readonly Lazy<T> _instance = new Lazy<T>();
        public static T GetInstance() => _instance.Value;
    }
}
