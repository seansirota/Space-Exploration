using System;
using System.Data.Common;

namespace SpaceExploration
{
    class Star : CelObject<Star.StarType>
    {
        private const int StarPowerBase = 3;
        public int ID { get; set; }
        public override string? Name { get; set; }
        public override StarType Type { get; set; }
        public int Temperature { get; set; }
        public override double Mass { get; set; }
        public override bool Visited { get; set; }
        public override bool EventActive { get; set; }
        public override int Cooldown { get; set; }

        public Star(string name, StarType type, int temperature, double mass) {
            ID = GenID.CreateGenID();
            Name = name;
            Type = type;
            Temperature = temperature;
            Mass = mass;
        }

        public static void GenerateNewStars(List<Star> stars, string systemName)
        {
            int totalStars;
            int starChance = Program.Rand.Next(1, 101);
            int bonusStars = 0;
            int bonusStarThreshold;
            bool bonusStarAdded;

            totalStars = (starChance <= 70) ?  1 : 2;

            do
            {
                bonusStarThreshold = (int)Math.Pow(StarPowerBase, bonusStars + 1);
                starChance = Program.Rand.Next(1, bonusStarThreshold + 1);

                if (starChance == bonusStarThreshold)
                {
                    bonusStarAdded = true;
                    bonusStars++;
                }
                else
                    bonusStarAdded = false;
            } while (bonusStarAdded);

            totalStars += bonusStars;

            string starName;
            int typeChance;
            int cumulativeTotal;
            StarType type;
            int temperature;
            double mass;

            for (int i = 0; i < totalStars; i++)
            {
                typeChance = Program.Rand.Next(1, 101);
                cumulativeTotal = 0;

                foreach (KeyValuePair<StarType, StarTypeData> catalog in StarCatalog)
                {
                    cumulativeTotal += catalog.Value.SpawnChance;
                    if (typeChance > cumulativeTotal)
                        continue;
                    else
                    {
                        starName = Namer.BuildName(1, systemName, i + 1);
                        type = catalog.Key;
                        temperature = Program.Rand.Next(catalog.Value.MinTemperature, catalog.Value.MaxTemperature + 1);
                        mass = (double)Program.Rand.Next((int)(catalog.Value.MinMass * 100), (int)(catalog.Value.MaxMass * 100) + 1) / 100;

                        Star star = new Star(starName, type, temperature, mass);
                        stars.Add(star);
                        break;
                    }
                }
            }
        }

        public static readonly Dictionary<int, Element.ElementType> StarIndex = new Dictionary<int, Element.ElementType>()
        {
            [7] = Element.ElementType.Hydrogen,
            [8] = Element.ElementType.Helium,
            [9] = Element.ElementType.Carbon,
            [10] = Element.ElementType.Nitrogen,
            [11] = Element.ElementType.Oxygen,
            [12] = Element.ElementType.Antimatter
        };

        public static readonly Dictionary<StarType, StarTypeData> StarCatalog = new Dictionary<StarType, StarTypeData>()
        {
            [StarType.RedDwarf] = new("Red Dwarf", 45, 2, 2000, 4000, 0.08, 0.5, 95, 70, 5, 3, 2, 1),
            [StarType.OrangeDwarf] = new("Orange Dwarf", 20, 2, 4000, 5300, 0.5, 0.8, 90, 65, 8, 6, 4, 1),
            [StarType.YellowDwarf] = new("Yellow Dwarf", 13, 2, 5300, 6000, 0.8, 1.2, 85, 60, 10, 8, 6, 1),
            [StarType.RedGiant] = new("Red Giant", 7, 3, 3000, 5000, 0.8, 8, 55, 80, 45, 32, 20, 2),
            [StarType.BlueGiant] = new("Blue Giant", 3, 3, 10000, 40000, 8, 50, 75, 85, 35, 35, 40, 2),
            [StarType.WhiteDwarf] = new("White Dwarf", 9, 2, 8000, 40000, 0.17, 1.4, 10, 15, 85, 90, 80, 5),
            [StarType.Pulsar] = new("Pulsar", 1, 4, 600000, 1000000, 1.4, 3, 0, 0, 5, 5, 5, 80),
            [StarType.BlackHole] = new("Black Hole", 2, 1, 0, 0, 3, 10, 0, 0, 0, 0, 0, 0)
        };

        public sealed record StarTypeData(
            string? DisplayName,
            int SpawnChance, // Chance to spawn. Shared with other star types
            int FuelCost, // Cost of visiting planet.
            int MinTemperature, // Kelvin
            int MaxTemperature,
            double MinMass, // Solar Masses
            double MaxMass,
            int HydrogenChance, // Chance to extract in one attempt
            int HeliumChance,
            int CarbonChance,
            int NitrogenChance,
            int OxygenChance,
            int AntimatterChance
        );

        public enum StarType
        {
            RedDwarf,
            OrangeDwarf,
            YellowDwarf,
            RedGiant,
            BlueGiant,
            WhiteDwarf,
            Pulsar,
            BlackHole
        }
    }
}