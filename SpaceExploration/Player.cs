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
        public static SystemScanner SysScan { get; set; } = new SystemScanner();
        public static CelestialScanner CelScan { get; set; } = new CelestialScanner();
        public static FuelCapacity FuelCap { get; set; } = new FuelCapacity();
        public static RockMiner RockMiner { get; set; } = new RockMiner();
        public static GasSiphon GasSiphon { get; set; } = new GasSiphon();
        public static CollectClaw CollectClaw { get; set; } = new CollectClaw();
        public static CargoCapacity CargoCap { get; set; } = new CargoCapacity();
        public static AirCapacity AirCap { get; set; } = new AirCapacity();
        public static HullIntegrity HullInt { get; set; } = new HullIntegrity();
        public static int Cargo { get; set; } = CargoCap.FunctionAttributes[CargoCap.Level];
        public static double Fuel { get; set; } = FuelCap.FunctionAttributes[FuelCap.Level];
        public static double Hull { get; set; } = HullInt.FunctionAttributes[HullInt.Level];
        public static double Air { get; set; } = AirCap.FunctionAttributes[AirCap.Level];
        public static Dictionary<Element.ElementType, int> ElementAmounts = new Dictionary<Element.ElementType, int>()
        {
            [Element.ElementType.Hydrogen] = 0,
            [Element.ElementType.Helium] = 0,
            [Element.ElementType.Carbon] = 0,
            [Element.ElementType.Nitrogen] = 0,
            [Element.ElementType.Oxygen] = 0,
            [Element.ElementType.Magnesium] = 0,
            [Element.ElementType.Aluminum] = 0,
            [Element.ElementType.Silicon] = 0,
            [Element.ElementType.Sulfur] = 0,
            [Element.ElementType.Chlorine] = 0,
            [Element.ElementType.Titanium] = 0,
            [Element.ElementType.Iron] = 0,
            [Element.ElementType.Nickel] = 0,
            [Element.ElementType.Copper] = 0,
            [Element.ElementType.Uranium] = 0,
            [Element.ElementType.Water] = 0,
            [Element.ElementType.Methane] = 0,
            [Element.ElementType.Ammonia] = 0,
            [Element.ElementType.CarbonDioxide] = 0,
            [Element.ElementType.Antimatter] = 0,
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
        public Dictionary<int, Tuple<int, int>> FunctionAttributes;

        public RockMiner()
        {
            Level = 1;
            FunctionAttributes = new Dictionary<int, Tuple<int, int>>()
            {
                { 1, new Tuple<int, int>( 3, 5 ) }, // Min and max units that can be extracted per attempt
                { 2, new Tuple<int, int>( 5, 10 ) },
                { 3, new Tuple<int, int>( 10, 20 ) }
            };
        }
    }

    struct GasSiphon
    {
        public int Level;
        public Dictionary<int, Tuple<int, int>> FunctionAttributes;

        public GasSiphon()
        {
            Level = 1;
            FunctionAttributes = new Dictionary<int, Tuple<int, int>>()
            {
                { 1, new Tuple<int, int>( 10, 15 ) }, // Min and max units that can be extracted per attempt
                { 2, new Tuple<int, int>( 15, 25 ) },
                { 3, new Tuple<int, int>( 25, 50 ) }
            };
        }
    }

    struct CollectClaw
    {
        public int Level;
        public Dictionary<int, Tuple<int, int>> FunctionAttributes;

        public CollectClaw()
        {
            Level = 1;
            FunctionAttributes = new Dictionary<int, Tuple<int, int>>()
            {
                { 1, new Tuple<int, int>( 1, 2 ) }, // Min and max units that can be extracted per attempt
                { 2, new Tuple<int, int>( 2, 3 ) },
                { 3, new Tuple<int, int>( 3, 5 ) }
            };
        }
    }

    struct CargoCapacity
    {
        public int Level;
        public Dictionary<int, int> FunctionAttributes;

        public CargoCapacity()
        {
            Level = 1;
            FunctionAttributes = new Dictionary<int, int>()
            {
                {1, 200}, // Maximum cargo capacity for all elements
                {2, 350},
                {3, 500}
            };
        }
    }

    struct AirCapacity
    {
        public int Level;
        public Dictionary<int, int> FunctionAttributes;

        public AirCapacity()
        {
            Level = 1;
            FunctionAttributes = new Dictionary<int, int>()
            {
                {1, 100}, // How much air the PLayer's ship can hold
                {2, 200},
                {3, 300}
            };
        }
    }

    struct HullIntegrity
    {
        public int Level;
        public Dictionary<int, int> FunctionAttributes;

        public HullIntegrity()
        {
            Level = 1;
            FunctionAttributes = new Dictionary<int, int>()
            {
                {1, 75}, // How many hitpoints the ship hull has before failing.
                {2, 150},
                {3, 250}
            };
        }
    }
}