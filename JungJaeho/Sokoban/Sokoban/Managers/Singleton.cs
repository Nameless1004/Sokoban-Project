using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    public class Singleton<T>
    {
        public static readonly Lazy<T> _instance = new Lazy<T>();
        public static T Instance { get { return _instance.Value; } }
    }
}
