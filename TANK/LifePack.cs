using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TANK
{
    class LifePack
    {
        public int LifePackLocationX { get; set; }
        public int LifePackLocationY { get; set; }
        public int lifetime { get; set; }

        public override string ToString()
        {
            return " X Coordinate " + this.LifePackLocationX + "\n Y Coordinate " + this.LifePackLocationY 
                + "\n Lifetime " + this.lifetime + "\n";
        }
    }
}
