using System;
using System.Dynamic;
using System.Runtime;

namespace SpaceExploration
{
    class StarSystem
    {
        public const int ExclusionRadius = 2;
        public static readonly Dictionary<int, StarSystem> Systems = new Dictionary<int, StarSystem>();
        public static readonly Dictionary<int, Tuple<int, int>> Coordinates = new Dictionary<int, Tuple<int, int>>();
        public int ID { get; set; }
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string? Note { get; set; }
        public bool Visited { get; set; }
        public bool EventActive { get; set; }
        public List<Star> Stars { get; set; } = new List<Star>();
        public List<Planet> Planets { get; set; } = new List<Planet>();

        public StarSystem(int x, int y)
        {
            ID = GenID.CreateGenID();
            Name = Namer.CreateStem();
            X = x;
            Y = y;
            Systems[ID] = this;
            Coordinates[ID] = new Tuple<int, int>(X, Y);
        }

        public static void GenerateNewSystems()
        {
            Dictionary<int, Tuple<int, int>> excludeCoords = new Dictionary<int, Tuple<int, int>>();
            int playerX = Player.X;
            int playerY = Player.Y;
            int playerR = Player.SysScan.FunctionAttributes[Player.SysScan.Level];

            int surfaceArea = (int)Math.Pow(playerR * 2 + 1, 2);
            int exclusionArea = (int)Math.Pow(ExclusionRadius * 2 + 1, 2);
            int allowedGens = (surfaceArea / exclusionArea) + (surfaceArea % exclusionArea == 0 ? 0 : 1);

            int leftBound = playerX - playerR;
            int rightBound = playerX + playerR;
            int lowerBound = playerY - playerR;
            int upperBound = playerY + playerR;

            int coordX;
            int coordY;

            foreach (KeyValuePair<int, Tuple<int, int>> kvpCoord in Coordinates)
            {
                coordX = kvpCoord.Value.Item1;
                coordY = kvpCoord.Value.Item2;

                if (coordX >= leftBound && coordX <= rightBound && coordY >= lowerBound && coordY <= upperBound)
                    excludeCoords[kvpCoord.Key] = kvpCoord.Value;
            }

            allowedGens -= excludeCoords.Count;
            if (Player.currentSystem == null)
                excludeCoords[0] = new Tuple<int, int>(playerX, playerY);

            int randX;
            int randY;
            int localLeftBound;
            int localRightBound;
            int localLowerBound;
            int localUpperBound;
            bool failCond;

            for (int i = 0; i < allowedGens; i++)
            {
                randX = Program.Rand.Next(leftBound, rightBound + 1);
                randY = Program.Rand.Next(lowerBound, upperBound + 1);
                failCond = false;

                foreach (KeyValuePair<int, Tuple<int, int>> kvpCoord in excludeCoords)
                {
                    coordX = kvpCoord.Value.Item1;
                    coordY = kvpCoord.Value.Item2;

                    localLeftBound = coordX - ExclusionRadius;
                    localRightBound = coordX + ExclusionRadius;
                    localLowerBound = coordY - ExclusionRadius;
                    localUpperBound = coordY + ExclusionRadius;

                    if (randX >= localLeftBound && randX <= localRightBound && randY >= localLowerBound && randY <= localUpperBound)
                    {
                        failCond = true;
                        break;
                    }
                }

                if (failCond == true)
                    continue;

                StarSystem starSystem = new StarSystem(randX, randY);
                excludeCoords[starSystem.ID] = new Tuple<int, int>(randX, randY);

                Star.GenerateNewStars(starSystem.Stars, starSystem.Name);
                Planet.GenerateNewPlanets(starSystem.Planets, starSystem.Name);
            }
        }

        public static List<int> GetNearbySystems()
        {
            List<int> nearbySystems = new List<int>();
            int playerX = Player.X;
            int playerY = Player.Y;
            int playerR = Player.SysScan.FunctionAttributes[Player.SysScan.Level];

            int leftBound = playerX - playerR;
            int rightBound = playerX + playerR;
            int lowerBound = playerY - playerR;
            int upperBound = playerY + playerR;

            int coordX;
            int coordY;

            foreach (KeyValuePair<int, Tuple<int, int>> kvpCoord in Coordinates)
            {
                coordX = kvpCoord.Value.Item1;
                coordY = kvpCoord.Value.Item2;

                if (coordX == playerX && coordY == playerY)
                    continue;

                if (coordX >= leftBound && coordX <= rightBound && coordY >= lowerBound && coordY <= upperBound)
                    nearbySystems.Add(kvpCoord.Key);
            }
            
            return nearbySystems;
        }

        public static double GetDistance(StarSystem system)
        {
            int playerX = Player.X;
            int playerY = Player.Y;
            int systemX = system.X;
            int systemY = system.Y;

            int deltaX = systemX - playerX;
            int deltaY = systemY - playerY;

            double distance = Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));

            return distance;
        }

        public static string GetDirection(StarSystem system)
        {
            int playerX = Player.X;
            int playerY = Player.Y;
            int systemX = system.X;
            int systemY = system.Y;

            int deltaX = systemX - playerX;
            int deltaY = systemY - playerY;
            
            double angle = Math.Atan2(deltaY, deltaX);
            double degrees = angle * (180 / Math.PI);

            if (degrees < 0)
                degrees += 360;

            string[] directions = ["E", "NE", "N", "NW", "W", "SW", "S", "SE"];

            int index = (int)Math.Round(degrees / 45) % 8;
            string direction = directions[index];

            return direction;
        }
    }
}