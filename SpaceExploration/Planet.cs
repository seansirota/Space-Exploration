using System;

namespace SpaceExploration
{
    class Planet : CelObject<Planet.PlanetType>
    {
        private const int PlanetPowerBase = 2;
        public int ID { get; set; }
        public override string? Name { get; set; }
        public override PlanetType Type { get; set; }
        public override double Mass { get; set; }
        public override bool Visited { get; set; }
        public override bool EventActive { get; set; }
        public override int Cooldown { get; set; }

        public Planet(string name, PlanetType type, double mass) {
            ID = GenID.CreateGenID();
            Name = name;
            Type = type;
            Mass = mass;
        }

        public static void GenerateNewPlanets(List<Planet> planets, string systemName)
        {
            int totalPlanets;
            int planetChance = Program.Rand.Next(1, 101);
            int bonusPlanets = 0;
            int bonusPlanetThreshold;
            bool bonusPlanetAdded;

            if (planetChance <= 35)
                totalPlanets = 3;
            else if (planetChance <= 65)
                totalPlanets = 2;
            else if (planetChance <= 90)
                totalPlanets = 1;
            else
                totalPlanets = 0;

            do
            {
                bonusPlanetThreshold = (int)Math.Pow(PlanetPowerBase, bonusPlanets + 1);
                planetChance = Program.Rand.Next(1, bonusPlanetThreshold + 1);

                if (planetChance == bonusPlanetThreshold)
                {
                    bonusPlanetAdded = true;
                    bonusPlanets++;
                }
                else
                    bonusPlanetAdded = false;
            } while (bonusPlanetAdded);

            totalPlanets += bonusPlanets;

            string planetName;
            int typeChance;
            int cumulativeTotal;
            PlanetType type;
            double mass;

            for (int i = 0; i < totalPlanets; i++)
            {
                typeChance = Program.Rand.Next(1, 101);
                cumulativeTotal = 0;

                foreach (KeyValuePair<PlanetType, PlanetTypeData> catalog in PlanetCatalog)
                {
                    cumulativeTotal += catalog.Value.SpawnChance;
                    if (typeChance > cumulativeTotal)
                        continue;
                    else
                    {
                        planetName = Namer.BuildName(2, systemName, i + 1);
                        type = catalog.Key;
                        mass = (double)Program.Rand.Next((int)(catalog.Value.MinMass * 100), (int)(catalog.Value.MaxMass * 100) + 1) / 100;

                        Planet planet = new Planet(planetName, type, mass);
                        planets.Add(planet);
                        break;
                    }
                }
            }
        }

        public static readonly Dictionary<PlanetType, PlanetTypeData> PlanetCatalog = new Dictionary<PlanetType, PlanetTypeData>()
        {
            [PlanetType.Rock] = new("Rock", 25, 0, 1, 0.1, 2, 5, 2, 10, 5, 5, 40, 35, 60, 15, 5, 20, 70, 40, 25, 5, 5, 0, 0, 10, 0),
            [PlanetType.Ice] = new("Ice", 16, 1, 1, 0.2, 5, 20, 10, 15, 35, 30, 10, 10, 20, 5, 5, 5, 20, 10, 5, 1, 95, 40, 35, 20, 0),
            [PlanetType.Gas] = new("Gas Giant", 15, 0, 3, 10, 300, 95, 85, 10, 20, 5, 0, 0, 0, 5, 2, 0, 5, 2, 0, 0, 20, 60, 35, 15, 0),
            [PlanetType.Terran] = new("Terran", 12, 25, 2, 0.5, 3, 15, 5, 45, 40, 70, 20, 20, 40, 10, 5, 15, 45, 25, 20, 3, 80, 15, 10, 35, 0),
            [PlanetType.Toxic] = new("Toxic", 8, 5, 2, 0.5, 4, 20, 10, 25, 25, 10, 15, 15, 35, 60, 70, 10, 40, 20, 10, 5, 10, 35, 50, 85, 0),
            [PlanetType.Volcanic] = new("Volcanic", 7, 0, 3, 0.3, 3, 5, 2, 15, 5, 10, 55, 40, 70, 85, 20, 35, 80, 45, 25, 15, 5, 0, 5, 60, 1),
            [PlanetType.Ocean] = new("Ocean", 7, 20, 2, 0.5, 5, 20, 5, 35, 40, 75, 10, 10, 25, 5, 2, 5, 25, 10, 5, 1, 100, 20, 10, 25, 0),
            [PlanetType.Desert] = new("Desert", 5, 8, 1, 0.3, 2, 5, 2, 15, 10, 10, 25, 20, 55, 20, 10, 15, 50, 30, 20, 8, 5, 5, 0, 45, 0),
            [PlanetType.Abandoned] = new("Abandoned", 2, 0, 2, 0.5, 4, 10, 5, 35, 20, 30, 30, 35, 50, 20, 10, 40, 55, 40, 45, 15, 30, 10, 5, 25, 5),
            [PlanetType.Synthetic] = new("Synthetic", 1, 50, 4, 0.5, 6, 5, 5, 20, 10, 20, 45, 60, 85, 15, 5, 70, 70, 65, 75, 25, 10, 0, 0, 5, 15),
            [PlanetType.Ruined] = new("Ruined", 1, 0, 1, 0.5, 5, 10, 5, 40, 25, 35, 35, 35, 50, 30, 15, 45, 60, 45, 40, 20, 35, 10, 5, 35, 3),
            [PlanetType.Dark] = new("Dark", 1, 0, 5, 1, 20, 15, 10, 25, 20, 5, 20, 20, 40, 25, 15, 35, 55, 35, 30, 30, 5, 20, 15, 40, 25)
        };

        public sealed record PlanetTypeData(
            string? DisplayName,
            int SpawnChance, // Chance to spawn. Shared with other star types
            int LifeChance, // Chance to host intelligent life.
            int FuelCost, // Cost of visiting planet.
            double MinMass, // Earth Masses
            double MaxMass,
            int HydrogenChance, // Chance to extract in one attempt
            int HeliumChance,
            int CarbonChance,
            int NitrogenChance,
            int OxygenChance,
            int MagnesiumChance,
            int AluminumChance,
            int SiliconChance,
            int SulfurChance,
            int ChlorineChance,
            int TitaniumChance,
            int IronChance,
            int NickelChance,
            int CopperChance,
            int UraniumChance,
            int WaterChance,
            int MethaneChance,
            int AmmoniaChance,
            int CarbonDioxideChance,
            int AntimatterChance
        );

        public enum PlanetType
        {
            Rock, // Basic types, detectable with level 1 celestial sensors
            Ice,
            Gas,
            Terran,
            Toxic, // Uncommon types, only detectable with level 2 sensors
            Volcanic,
            Ocean,
            Desert,
            Abandoned, //Rarest types, only detectable with level 3 sensors.
            Synthetic,
            Ruined,
            Dark
        }
    }
}