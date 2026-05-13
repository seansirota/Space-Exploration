using System;
using System.Numerics;

namespace SpaceExploration
{
    class Player
    {
        public static int X { get; set; } = 0;
        public static int Y { get; set; } = 0;
        public static int? CurrentSystem { get; set; } = null;
        public static int Money = 0;
        public static CelObjectGeneric? CurrentObject = null;
        public static Dictionary<FunctionType, IFunction> Functions = new Dictionary<FunctionType, IFunction>()
        {
            [FunctionType.SystemScanner] = new SystemScanner(),
            [FunctionType.CelestialScanner] = new CelestialScanner(),
            [FunctionType.RockMiner] = new RockMiner(),
            [FunctionType.GasSiphon] = new GasSiphon(),
            [FunctionType.CollectClaw] = new CollectClaw(),
            [FunctionType.CargoCapacity] = new CargoCapacity(),
            [FunctionType.FuelCapacity] = new FuelCapacity(),
            [FunctionType.HullIntegrity] = new HullIntegrity(),
            [FunctionType.AirCapacity] = new AirCapacity()
        };
        public static Dictionary<ResourceType, double> ResourceAmounts = new Dictionary<ResourceType, double>()
        {
            [ResourceType.Fuel] = GetFunction<double>(FunctionType.FuelCapacity),
            [ResourceType.Hull] = GetFunction<double>(FunctionType.HullIntegrity),
            [ResourceType.Air] = GetFunction<int>(FunctionType.AirCapacity)
        };
        public static Dictionary<ElementType, int> ElementAmounts = new Dictionary<ElementType, int>()
        {
            [ElementType.Hydrogen] = 0,
            [ElementType.Helium] = 0,
            [ElementType.Carbon] = 0,
            [ElementType.Nitrogen] = 0,
            [ElementType.Oxygen] = 0,
            [ElementType.Magnesium] = 0,
            [ElementType.Aluminum] = 0,
            [ElementType.Silicon] = 0,
            [ElementType.Sulfur] = 0,
            [ElementType.Chlorine] = 0,
            [ElementType.Titanium] = 0,
            [ElementType.Iron] = 0,
            [ElementType.Nickel] = 0,
            [ElementType.Copper] = 0,
            [ElementType.Uranium] = 0,
            [ElementType.Water] = 0,
            [ElementType.Methane] = 0,
            [ElementType.Ammonia] = 0,
            [ElementType.CarbonDioxide] = 0,
            [ElementType.Antimatter] = 0,
        };

        public static T GetFunction<T>(FunctionType FunctionType)
        {
            IFunction<T> function = (IFunction<T>)Functions[FunctionType];

            return function.FunctionAttributes[function.Level];
        }

        public static readonly Dictionary<ResourceType, ResourceData> ResourceCatalog = new Dictionary<ResourceType, ResourceData>()
        {
            [ResourceType.Fuel] = new("Fuel", 150, FunctionType.FuelCapacity, new List<ResourceExchange> {
                new(ElementType.Water, 1),
                new(ElementType.Hydrogen, 3),
                new(ElementType.Helium, 7),
                new(ElementType.Antimatter, 30)
            }),
            [ResourceType.Hull] = new("Hull", 100, FunctionType.HullIntegrity, new List<ResourceExchange>
            {
                new(ElementType.Silicon, 1),
                new(ElementType.Copper, 1),
                new(ElementType.Iron,  4),
                new(ElementType.Aluminum, 10),
                new(ElementType.Nickel, 11),
                new(ElementType.Titanium, 25)
            }),
            [ResourceType.Air] = new("Air", 75, FunctionType.AirCapacity, new List<ResourceExchange>
            {
                new(ElementType.Nitrogen, 1),
                new(ElementType.CarbonDioxide, 2),
                new(ElementType.Water, 5),
                new(ElementType.Oxygen, 10)
            })
        };

        public sealed record ResourceData(
            string? DisplayName,
            int MoneyAmount,
            FunctionType Function,
            List<ResourceExchange> ElementCosts
        );

        public sealed record ResourceExchange(
            ElementType ElementType, // Element to input for creating the resource
            int OutputAmount //Output amount of resource from one input element amount
        );
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
                {2, 125},
                {3, 250}
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
                {3, 300}
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
                {2, 250},
                {3, 400}
            };
        }
    }

    public enum ResourceType
    {
        Fuel,
        Hull,
        Air
    }

    public enum FunctionType
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