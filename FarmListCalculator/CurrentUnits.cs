using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmListCalculator
{
    internal class CurrentUnits
    {
        static CurrentUnits _instance = new CurrentUnits();
        public static CurrentUnits Instance { get { return _instance; } }
        public int[] Units { get; set; }

        private CurrentUnits()
        {
            Units = new int[10]
            {
                0,0,0,0,0,0,0,0,0,0
            };
        }
    }
}
