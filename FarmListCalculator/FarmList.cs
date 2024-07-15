using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmListCalculator
{
    internal class FarmList
    {
        internal string Name { get; private set; }
        internal List<FarmListItem> Targets { get; private set; }
        public FarmList(string name, List<FarmListItem> targets)
        {
            Name = name;
            Targets = targets ?? new List<FarmListItem>();
        }
    }

    internal class FarmListItem
    {
        internal string Name { get; private set; }
        internal double Distance { get; private set; }
        internal double? RealDistance { get; set; }
        internal Dictionary<string, int> Units { get; private set; }
        internal int? TimeToRotate { get; set; }

        public FarmListItem(string name, double distance, Dictionary<string, int> units)
        {
            Name = name;
            Distance = distance;
            Units = units ?? new Dictionary<string, int>();
        }

    }
}
