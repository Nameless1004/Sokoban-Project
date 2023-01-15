using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    class Renderer
    {
        public void Render(Vector2 position, string icon, ConsoleColor color = ConsoleColor.Black)
        {
            ConsoleColor prev = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.SetCursorPosition(position.X, position.Y);
            Console.Write(icon);
            Console.ForegroundColor = prev;
        }
    }
}
