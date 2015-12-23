using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TANK
{
    class KeyEvents
    {
        public string getKeyCommand()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                return "UP#";
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                return "DOWN#";
            }
            else if (keyInfo.Key == ConsoleKey.RightArrow)
            {
                return "RIGHT#";
            }
            else if (keyInfo.Key == ConsoleKey.LeftArrow)
            {
                return "LEFT#";
            }

            return null;
        }
    }
}
