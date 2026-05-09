using System;
using System.Numerics;

namespace SpaceExploration
{
    class Player
    {
        public static int X { get; set; } = 0;
        public static int Y { get; set; } = 0;
        public static int? currentSystem { get; set; } = null;
        public static CelObjectGeneric? currentObject = null;
        public static double Fuel { get; set; } = 50;
        public static double Hull { get; set; } = 100;
        public static double Air { get; set; } = 100;
        public static SystemScanner SysScan { get; set; } = new SystemScanner();
        public static CelestialScanner CelScan { get; set; } = new CelestialScanner();
        public static FuelCapacity FuelCap { get; set; } = new FuelCapacity();
        public static RockMiner RockMiner { get; set; } = new RockMiner();
        public static GasSiphon GasSiphon { get; set; } = new GasSiphon();
        public static CollectClaw CollectClaw { get; set; } = new CollectClaw();
        public static Dictionary<Element.ElementName, int> ElementAmounts = new Dictionary<Element.ElementName, int>()
        {
            [Element.ElementName.Hydrogen] = 0,
            [Element.ElementName.Helium] = 0,
            [Element.ElementName.Carbon] = 0,
            [Element.ElementName.Nitrogen] = 0,
            [Element.ElementName.Oxygen] = 0,
            [Element.ElementName.Magnesium] = 0,
            [Element.ElementName.Aluminum] = 0,
            [Element.ElementName.Silicon] = 0,
            [Element.ElementName.Sulfur] = 0,
            [Element.ElementName.Chlorine] = 0,
            [Element.ElementName.Titanium] = 0,
            [Element.ElementName.Iron] = 0,
            [Element.ElementName.Nickel] = 0,
            [Element.ElementName.Copper] = 0,
            [Element.ElementName.Uranium] = 0,
            [Element.ElementName.Water] = 0,
            [Element.ElementName.Methane] = 0,
            [Element.ElementName.Ammonia] = 0,
            [Element.ElementName.CarbonDioxide] = 0,
            [Element.ElementName.Antimatter] = 0,
        };
    }

    struct SystemScanner
    {
        public int Level;
        public Dictionary<int, int> FunctionAttributes;

        public SystemScanner()
        {
            Level = 1;
            FunctionAttributes = new Dictionary<int, int>()
            {
                {1, 5}, // Radius of scan for star systems
                {2, 10},
                {3, 15}
            };
        }
    }

    struct CelestialScanner
    {
        public int Level;
        public Dictionary<int, List<Planet.PlanetType>> FunctionAttributes;

        public CelestialScanner()
        {
            Level = 1;
            FunctionAttributes = new Dictionary<int, List<Planet.PlanetType>>()
            {
                {1, new List<Planet.PlanetType>() // Planet types detectablable within star system
                    {
                        Planet.PlanetType.Rock,
                        Planet.PlanetType.Ice,
                        Planet.PlanetType.Gas,
                        Planet.PlanetType.Terran
                    }
                },
                {2, new List<Planet.PlanetType>()
                    {
                        Planet.PlanetType.Toxic,
                        Planet.PlanetType.Volcanic,
                        Planet.PlanetType.Ocean,
                        Planet.PlanetType.Desert
                    }
                },
                {3, new List<Planet.PlanetType>()
                    {
                        Planet.PlanetType.Abandoned,
                        Planet.PlanetType.Synthetic,
                        Planet.PlanetType.Ruined,
                        Planet.PlanetType.Dark
                    }
                }
            };
        }
    }

    struct FuelCapacity
    {
        public int Level;
        public Dictionary<int, int> FunctionAttributes;

        public FuelCapacity()
        {
            Level = 1;
            FunctionAttributes = new Dictionary<int, int>()
            {
                {1, 50}, // How much fuel the PLayer's ship can hold
                {2, 100},
                {3, 150}
            };
        }
    }

    struct RockMiner
    {
        public int Level;
        public Dictionary<int, Tuple<int, int, int>> FunctionAttributes;

        public RockMiner()
        {
            Level = 1;
            FunctionAttributes = new Dictionary<int, Tuple<int, int, int>>() // Min and max units that can be extracted per attempt
            {
                { 1, new Tuple<int, int, int>( 1, 3, 5 ) },
                { 2, new Tuple<int, int, int>( 2, 5, 10 ) },
                { 3, new Tuple<int, int, int>( 3, 10, 20 ) }
            };
        }
    }

    struct GasSiphon
    {
        public int Level;
        public Dictionary<int, Tuple<int, int, int>> FunctionAttributes;

        public GasSiphon()
        {
            Level = 1;
            FunctionAttributes = new Dictionary<int, Tuple<int, int, int>>() // Min and max units that can be extracted per attempt
            {
                { 1, new Tuple<int, int, int>( 1, 10, 15 ) },
                { 2, new Tuple<int, int, int>( 2, 15, 25 ) },
                { 3, new Tuple<int, int, int>( 3, 25, 50 ) }
            };
        }
    }

    struct CollectClaw
    {
        public int Level;
        public Dictionary<int, Tuple<int, int, int>> FunctionAttributes;

        public CollectClaw()
        {
            Level = 1;
            FunctionAttributes = new Dictionary<int, Tuple<int, int, int>>() // Min and max units that can be extracted per attempt
            {
                { 1, new Tuple<int, int, int>( 1, 1, 2 ) },
                { 2, new Tuple<int, int, int>( 2, 2, 3 ) },
                { 3, new Tuple<int, int, int>( 3, 3, 5 ) }
            };
        }
    }
}