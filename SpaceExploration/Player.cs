using System;
using System.Numerics;

namespace SpaceExploration
{
    class Player
    {
        public static int X { get; set; } = 0;
        public static int Y { get; set; } = 0;
        public static int? CurrentSystem { get; set; } = null;
        public static CelObjectGeneric? CurrentObject = null;
        public static Dictionary<FunType, IFunction> Functions = new Dictionary<FunType, IFunction>()
        {
            [FunType.SystemScanner] = new SystemScanner(),
            [FunType.CelestialScanner] = new CelestialScanner(),
            [FunType.RockMiner] = new RockMiner(),
            [FunType.GasSiphon] = new GasSiphon(),
            [FunType.CollectClaw] = new CollectClaw(),
            [FunType.CargoCapacity] = new CargoCapacity(),
            [FunType.FuelCapacity] = new FuelCapacity(),
            [FunType.HullIntegrity] = new HullIntegrity(),
            [FunType.AirCapacity] = new AirCapacity()
        };
        public static Dictionary<ResType, double> ResourceAmounts = new Dictionary<ResType, double>()
        {
            [ResType.Money] = 0,
            [ResType.Cargo] = GetFunction<int>(FunType.CargoCapacity),
            [ResType.Fuel] = GetFunction<double>(FunType.FuelCapacity),
            [ResType.Hull] = GetFunction<double>(FunType.HullIntegrity),
            [ResType.Air] = GetFunction<int>(FunType.AirCapacity)
        };
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

        public static T GetFunction<T>(FunType funType)
        {
            IFunction<T> function = (IFunction<T>)Functions[funType];

            return function.FunctionAttributes[function.Level];
        }
    }
    public interface IFunction
    {
        public int Level { get; }
    }

    public interface IFunction<T> : IFunction
    {
        public Dictionary<int, T> FunctionAttributes { get; }
    }

    public class SystemScanner : IFunction<int>
    {
        public int Level { get; set; }
        public Dictionary<int, int> FunctionAttributes { get; set; }

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

    public class CelestialScanner : IFunction<List<PlanetType>>
    {
        public int Level { get; set; }
        public Dictionary<int, List<PlanetType>> FunctionAttributes { get; set; }

        public CelestialScanner()
        {
            Level = 1;
            FunctionAttributes = new Dictionary<int, List<PlanetType>>()
            {
                {1, new List<PlanetType>() // Planet types detectablable within star system
                    {
                        PlanetType.Rock,
                        PlanetType.Ice,
                        PlanetType.Gas,
                        PlanetType.Terran
                    }
                },
                {2, new List<PlanetType>()
                    {
                        PlanetType.Toxic,
                        PlanetType.Volcanic,
                        PlanetType.Ocean,
                        PlanetType.Desert
                    }
                },
                {3, new List<PlanetType>()
                    {
                        PlanetType.Abandoned,
                        PlanetType.Synthetic,
                        PlanetType.Ruined,
                        PlanetType.Dark
                    }
                }
            };
        }
    }

    public class FuelCapacity : IFunction<double>
    {
        public int Level { get; set; }
        public Dictionary<int, double> FunctionAttributes { get; set; }

        public FuelCapacity()
        {
            Level = 1;
            FunctionAttributes = new Dictionary<int, double>()
            {
                {1, 50}, // How much fuel the PLayer's ship can hold
                {2, 100},
                {3, 150}
            };
        }
    }

    public class RockMiner : IFunction<Tuple<int, int>>
    {
        public int Level { get; set; }
        public Dictionary<int, Tuple<int, int>> FunctionAttributes { get; set; }

        public RockMiner()
        {
            Level = 1;
            FunctionAttributes = new Dictionary<int, Tuple<int, int>>()
            {
                { 1, new Tuple<int, int>( 3, 5 ) }, // Min and max units that can be extracted per attempt
                { 2, new Tuple<int, int>( 5, 7 ) },
                { 3, new Tuple<int, int>( 7, 10 ) }
            };
        }
    }

    public class GasSiphon : IFunction<Tuple<int, int>>
    {
        public int Level { get; set; }
        public Dictionary<int, Tuple<int, int>> FunctionAttributes { get; set; }

        public GasSiphon()
        {
            Level = 1;
            FunctionAttributes = new Dictionary<int, Tuple<int, int>>()
            {
                { 1, new Tuple<int, int>( 5, 8 ) }, // Min and max units that can be extracted per attempt
                { 2, new Tuple<int, int>( 8, 12 ) },
                { 3, new Tuple<int, int>( 12, 18 ) }
            };
        }
    }

    public class CollectClaw : IFunction<Tuple<int, int>>
    {
        public int Level { get; set; }
        public Dictionary<int, Tuple<int, int>> FunctionAttributes { get; set; }

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

    public class CargoCapacity : IFunction<int>
    {
        public int Level { get; set; }
        public Dictionary<int, int> FunctionAttributes { get; set; }

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

    public class AirCapacity : IFunction<int>
    {
        public int Level { get; set; }
        public Dictionary<int, int> FunctionAttributes { get; set; }

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

    public class HullIntegrity : IFunction<double>
    {
        public int Level { get; set; }
        public Dictionary<int, double> FunctionAttributes { get; set; }

        public HullIntegrity()
        {
            Level = 1;
            FunctionAttributes = new Dictionary<int, double>()
            {
                {1, 75}, // How many hitpoints the ship hull has before failing.
                {2, 150},
                {3, 250}
            };
        }
    }

    public enum ResType
    {
        Money,
        Cargo,
        Fuel,
        Hull,
        Air
    }

    public enum FunType
    {
        SystemScanner,
        CelestialScanner,
        RockMiner,
        GasSiphon,
        CollectClaw,
        CargoCapacity,
        FuelCapacity,
        HullIntegrity,
        AirCapacity
    }
}