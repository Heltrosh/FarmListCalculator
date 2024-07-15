using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmListCalculator
{
    internal class UnitSpeeds
    {
        public Dictionary<string, int> Teutons { get; private set; }
        public Dictionary<string, int> Gauls { get; private set; }
        public Dictionary<string, int> Romans { get; private set; }

        public UnitSpeeds()
        {
            Teutons = new Dictionary<string, int>
            {
                { "Clubswinger", 7 },
                { "Spearman", 7 },
                { "Axeman", 6 },
                { "Scout", 9 },
                { "Paladin", 10 },
                { "Teutonic Knight", 9 },
                { "Ram", 4 },
                { "Catapult", 3 },
                { "Chief", 4 },
                { "Settler", 5 }
            };

            Gauls = new Dictionary<string, int>
            {
                { "Phalanx", 7 },
                { "Swordsman", 6 },
                { "Pathfinder", 17 },
                { "Theutates Thunder", 19 },
                { "Druidrider", 16 },
                { "Haeduan", 13 },
                { "Ram", 4 },
                { "Catapult", 3 },
                { "Chief", 5 },
                { "Settler", 5 }
            };

            Romans = new Dictionary<string, int>
            {
                { "Legionnaire", 6 },
                { "Praetorian", 5 },
                { "Imperian", 7 },
                { "Equites Legati", 16 },
                { "Equites Imperatoris", 14 },
                { "Equites Caesaris", 10 },
                { "Ram", 4 },
                { "Fire Catapult", 3 },
                { "Senator", 4 },
                { "Settler", 5 }
            };
        }

        public int GetUnitSpeed(string tribe, string unitName)
        {
            switch (tribe.ToLower())
            {
                case "teutons":
                    return Teutons.TryGetValue(unitName, out var teutonSpeed) ? teutonSpeed : -1;
                case "gauls":
                    return Gauls.TryGetValue(unitName, out var gaulSpeed) ? gaulSpeed : -1;
                case "romans":
                    return Romans.TryGetValue(unitName, out var romanSpeed) ? romanSpeed : -1;
                default:
                    return -1;
            }
        }
    }
}
