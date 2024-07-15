using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmListCalculator
{
    internal class IndexConverter
    {
        public Dictionary<int, string> Tribes { get; private set; }
        public Dictionary<int, string> RomanUnits { get; private set; }
        public Dictionary<int, string> TeutonUnits { get; private set; }
        public Dictionary<int, string> GaulUnits { get; private set; }

        public IndexConverter()
        {
            Tribes = new Dictionary<int, string>
            {
                {1, "Romans" },
                {2, "Teutons" },
                {3, "Gauls" }
            };

            RomanUnits = new Dictionary<int, string>
            {
                {1, "Legionnaire"},
                {2, "Praetorian"},
                {3, "Imperian"},
                {4, "Equites Legati"},
                {5, "Equites Imperatoris"},
                {6, "Equites Caesaris"},
                {7, "Ram"},
                {8, "Fire Catapult"},
                {9, "Senator"},
                {10, "Settler"}
            };
            TeutonUnits = new Dictionary<int, string>
            {
                {1, "Clubswinger"},
                {2, "Spearman"},
                {3, "Axeman"},
                {4, "Scout"},
                {5, "Paladin"},
                {6, "Teutonic Knight"},
                {7, "Ram"},
                {8, "Catapult"},
                {9, "Chief"},
                {10, "Settler"}
            };

            GaulUnits = new Dictionary<int, string>
            {
                {1, "Phalanx"},
                {2, "Swordsman"},
                {3, "Pathfinder"},
                {4, "Theutates Thunder"},
                {5, "Druidrider"},
                {6, "Haeduan"},
                {7, "Ram"},
                {8, "Catapult"},
                {9, "Chief"},
                {10, "Settler"}
            };
        }

        public string GetNameByIndex(bool tribeOrUnit, int tribeIndex, int? unitIndex = null) // tribe = true, unit = false
        {
            if (tribeOrUnit)
                return Tribes[tribeIndex];
            if (unitIndex == null)
                throw new ArgumentNullException();
            switch (tribeIndex)
            {
                case 1:
                    return RomanUnits[unitIndex.Value];
                case 2:
                    return TeutonUnits[unitIndex.Value];
                case 3:
                    return GaulUnits[unitIndex.Value];
                default: 
                    throw new ArgumentOutOfRangeException();
            }
        }
        public int GetIndexByName(bool tribeOrUnit, string? tribeName = null, int? tribeIndex = null, string? unitName = null) // tribe = true, unit = false
        {
            if (tribeOrUnit)
            {
                if (tribeName == null)
                    throw new ArgumentNullException();
                return Tribes.Keys.FirstOrDefault(t => Tribes[t] == tribeName);
            }
            if (unitName == null || tribeIndex == null)
                throw new ArgumentNullException();
            switch (tribeIndex)
            {
                case 1:
                    return RomanUnits.Keys.FirstOrDefault(u => RomanUnits[u] == unitName);
                case 2:
                    return TeutonUnits.Keys.FirstOrDefault(u => TeutonUnits[u] == unitName);
                case 3:
                    return GaulUnits.Keys.FirstOrDefault(u => GaulUnits[u] == unitName);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

