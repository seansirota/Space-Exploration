using System;
using System.Numerics;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;

namespace SpaceExploration
{
    class Player
    {
        public static int X { get; set; } = 0;
        public static int Y { get; set; } = 0;
        public static int? CurrentSystem { get; set; } = null;
        public static int Money = 0;
        public static CelObjectGeneric? CurrentObject = null;
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
            int OutputAmount // Output amount of resource from one input element amount
        );

        public static readonly Dictionary<(FunctionType, int), FunctionData> FunctionCatalog = new Dictionary<(FunctionType, int), FunctionData>()
        {
            [(FunctionType.SystemScanner, 1)] = new("Basic Scanner", "Can scan star systems up to 5 light years away.", new List<FunctionExchange>
            {
                new(ElementType.Silicon, 20),
                new(ElementType.Copper, 15),
                new(ElementType.Aluminum, 8)
            }),
            [(FunctionType.SystemScanner, 2)] = new("Advanced Scanner", "Improved scan range detects star systems up to 10 light years away.", new List<FunctionExchange>
            {
                new(ElementType.Silicon, 22),
                new(ElementType.Copper, 16),
                new(ElementType.Aluminum, 6),
                new(ElementType.Nickel, 5),
                new(ElementType.Titanium, 3)
            }),
            [(FunctionType.SystemScanner, 3)] = new("Flagship Scanner", "Military-grade scanner spots star systems up to 15 light years away.", new List<FunctionExchange>
            {
                new(ElementType.Silicon, 24),
                new(ElementType.Copper, 17),
                new(ElementType.Nickel, 8),
                new(ElementType.Titanium, 8),
                new(ElementType.Helium, 3),
                new(ElementType.Antimatter, 1)
            }),
            [(FunctionType.CelestialScanner, 1)] = new("Optical Sensor", "Light-based celestial sensor can detect planets passing in front of their star.", new List<FunctionExchange>
            {
                new(ElementType.Silicon, 25),
                new(ElementType.Helium, 10),
                new(ElementType.Magnesium, 6)
            }),
            [(FunctionType.CelestialScanner, 2)] = new("Thermal Sensor", "Uses thermal signatures to discover heat-emitting planets in low visibility.", new List<FunctionExchange>
            {
                new(ElementType.Silicon, 27),
                new(ElementType.Helium, 12),
                new(ElementType.Magnesium, 10),
                new(ElementType.Uranium, 3)
            }),
            [(FunctionType.CelestialScanner, 3)] = new("Gamma Resonance Sensor", "Scans for gamma radiation to detect unknown and anomalous planets.", new List<FunctionExchange>
            {
                new(ElementType.Silicon, 26),
                new(ElementType.Helium, 11),
                new(ElementType.Magnesium, 15),
                new(ElementType.Uranium, 6),
                new(ElementType.Antimatter, 1)
            }),
            [(FunctionType.RockMiner, 1)] = new("Industrial Drill", "Mines for materials using an iron drill on surface sites.", new List<FunctionExchange>
            {
                new(ElementType.Iron, 25),
                new(ElementType.Aluminum, 15),
                new(ElementType.Copper, 7)
            }),
            [(FunctionType.RockMiner, 2)] = new("Diamond Drill", "Carbon-based tip allows for excavation and extraction deep underground.", new List<FunctionExchange>
            {
                new(ElementType.Iron, 24),
                new(ElementType.Aluminum, 15),
                new(ElementType.Copper, 8),
                new(ElementType.Nickel, 6),
                new(ElementType.Carbon, 7)
            }),
            [(FunctionType.RockMiner, 3)] = new("Laser Scraper", "High-energy laser excavator can extract materials from a planet's mantle.", new List<FunctionExchange>
            {
                new(ElementType.Iron, 22),
                new(ElementType.Aluminum, 15),
                new(ElementType.Copper, 3),
                new(ElementType.Nickel, 9),
                new(ElementType.Titanium, 5),
                new(ElementType.Helium, 4),
                new(ElementType.Antimatter, 1)
            }),
            [(FunctionType.GasSiphon, 1)] = new("Atmospheric Suction", "Intake technology collects gases in rudimentary storage.", new List<FunctionExchange>
            {
                new(ElementType.Silicon, 20),
                new(ElementType.Copper, 13),
                new(ElementType.CarbonDioxide, 10)
            }),
            [(FunctionType.GasSiphon, 2)] = new("Cryogenic Ingestion", "Collects, cools, and stabilizes gases under extremely low temperatures.", new List<FunctionExchange>
            {
                new(ElementType.Silicon, 21),
                new(ElementType.Copper, 15),
                new(ElementType.CarbonDioxide, 5),
                new(ElementType.Nitrogen, 5),
                new(ElementType.Water, 5),
                new(ElementType.Nickel, 3)
            }),
            [(FunctionType.GasSiphon, 3)] = new("Quantum Containment", "Stores unstable gaseous matter using gravitation and electromagnetic fields.", new List<FunctionExchange>
            {
                new(ElementType.Silicon, 22),
                new(ElementType.Copper, 16),
                new(ElementType.CarbonDioxide, 6),
                new(ElementType.Nickel, 8),
                new(ElementType.Uranium, 6),
                new(ElementType.Antimatter, 1)
            }),
            [(FunctionType.CollectClaw, 1)] = new("Mechanical Claw", "Retrieves delicate and unstable materials with care and precision.", new List<FunctionExchange>
            {
                new(ElementType.Copper, 18),
                new(ElementType.Silicon, 16),
                new(ElementType.Aluminum, 5)
            }),
            [(FunctionType.CollectClaw, 2)] = new("Field Initiator", "Creates a containment field that draws objects using electromagnetic forces.", new List<FunctionExchange>
            {
                new(ElementType.Copper, 17),
                new(ElementType.Silicon, 15),
                new(ElementType.Aluminum, 10),
                new(ElementType.Magnesium, 10)
            }),
            [(FunctionType.CollectClaw, 3)] = new("Nuclear Stabilizer", "Uses a radiation-absorbant claw to store radiactive materials in decay-resistant storage.", new List<FunctionExchange>
            {
                new(ElementType.Copper, 14),
                new(ElementType.Silicon, 12),
                new(ElementType.Aluminum, 4),
                new(ElementType.Magnesium, 15),
                new(ElementType.Uranium, 8),
                new(ElementType.Titanium, 8),
                new(ElementType.Antimatter, 1)
            }),
            [(FunctionType.CargoCapacity, 1)] = new("Standard Storage", "Simple storage structure for containing raw materials.", new List<FunctionExchange>
            {
                new(ElementType.Iron, 25),
                new(ElementType.Aluminum, 15)
            }),
            [(FunctionType.CargoCapacity, 2)] = new("Vacuum Storage", "Greater compression of materials by vacuuming out air and increasing pressure.", new List<FunctionExchange>
            {
                new(ElementType.Iron, 20),
                new(ElementType.Aluminum, 10),
                new(ElementType.Carbon, 20)
            }),
            [(FunctionType.CargoCapacity, 3)] = new("Quantum Compression", "Ultimate compression of matter by temporarily reducing distances between atoms.", new List<FunctionExchange>
            {
                new(ElementType.Iron, 20),
                new(ElementType.Aluminum, 10),
                new(ElementType.Carbon, 25),
                new(ElementType.Titanium, 5),
                new(ElementType.Antimatter, 1)
            }),
            [(FunctionType.FuelCapacity, 1)] = new("Fuel Tank", "Generic fuel storage for containing most fuel types.", new List<FunctionExchange>
            {
                new(ElementType.Aluminum, 18),
                new(ElementType.Copper, 12),
                new(ElementType.CarbonDioxide, 10)
            }),
            [(FunctionType.FuelCapacity, 2)] = new("Coolant Tank", "Fuel maintained using coolant and medium fluids.", new List<FunctionExchange>
            {
                new(ElementType.Aluminum, 18),
                new(ElementType.Copper, 12),
                new(ElementType.CarbonDioxide, 8),
                new(ElementType.Methane, 14)
            }),
            [(FunctionType.FuelCapacity, 3)] = new("Stateful Silo", "Advancing cooling and subduing tech keeps fuel in a single, controlled state.", new List<FunctionExchange>
            {
                new(ElementType.Aluminum, 18),
                new(ElementType.Copper, 12),
                new(ElementType.Methane, 12),
                new(ElementType.Ammonia, 10),
                new(ElementType.Chlorine, 8),
                new(ElementType.Antimatter, 1)
            }),
            [(FunctionType.HullIntegrity, 1)] = new("Iron Plating", "Reinforced iron armor to protect the ship from radiation and enemy fire.", new List<FunctionExchange>
            {
                new(ElementType.Iron, 23),
                new(ElementType.Aluminum, 18)
            }),
            [(FunctionType.HullIntegrity, 2)] = new("Composite Shell", "Carbon-composite material with high structural integrity used to protect the ship.", new List<FunctionExchange>
            {
                new(ElementType.Iron, 20),
                new(ElementType.Aluminum, 16),
                new(ElementType.Carbon, 8),
                new(ElementType.Nickel, 6)
            }),
            [(FunctionType.HullIntegrity, 3)] = new("Radiation Shield", "Uses nuclear force to repel radiation and reflect most high-energy beams.", new List<FunctionExchange>
            {
                new(ElementType.Iron, 20),
                new(ElementType.Aluminum, 16),
                new(ElementType.Carbon, 10),
                new(ElementType.Nickel, 12),
                new(ElementType.Uranium, 4),
                new(ElementType.Antimatter, 1)
            }),
            [(FunctionType.AirCapacity, 1)] = new("Atmosphere Regulator", "Diffusion system for releasing breathable air into the ship.", new List<FunctionExchange>
            {
                new(ElementType.Copper, 22),
                new(ElementType.Aluminum, 10),
                new(ElementType.CarbonDioxide, 9)
            }),
            [(FunctionType.AirCapacity, 2)] = new("Smart Circulation", "Systematic checks and adjustments on air composition keeps it clean and low toxicity.", new List<FunctionExchange>
            {
                new(ElementType.Copper, 20),
                new(ElementType.Aluminum, 10),
                new(ElementType.CarbonDioxide, 10),
                new(ElementType.Nitrogen, 5),
                new(ElementType.Silicon, 5)
            }),
            [(FunctionType.AirCapacity, 3)] = new("Nanobot Purification", "Releases nanobot tech into atmosphere to purify air molecules at the microscopic level.", new List<FunctionExchange>
            {
                new(ElementType.Copper, 18),
                new(ElementType.Aluminum, 9),
                new(ElementType.CarbonDioxide, 11),
                new(ElementType.Nitrogen, 8),
                new(ElementType.Silicon, 8),
                new(ElementType.Sulfur, 6),
                new(ElementType.Antimatter, 1)
            })
        };

        public sealed record FunctionData(
            string? DisplayName,
            string? Description,
            List<FunctionExchange> FunctionCosts
        );

        public sealed record FunctionExchange(
            ElementType ElementType, // Which element is required to upgrade the function
            int InputAmount // Input amount of element needed to upgrade the function
        );

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
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public Dictionary<int, int> FunctionAttributes { get; set; }

        public SystemScanner()
        {
            Level = 1;
            DisplayName = Player.FunctionCatalog[(FunctionType.SystemScanner, Level)].DisplayName!;
            Description = Player.FunctionCatalog[(FunctionType.SystemScanner, Level)].Description!;
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
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public Dictionary<int, List<PlanetType>> FunctionAttributes { get; set; }

        public CelestialScanner()
        {
            Level = 1;
            DisplayName = Player.FunctionCatalog[(FunctionType.CelestialScanner, Level)].DisplayName!;
            Description = Player.FunctionCatalog[(FunctionType.CelestialScanner, Level)].Description!;
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
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public Dictionary<int, Tuple<int, int>> FunctionAttributes { get; set; }

        public RockMiner()
        {
            Level = 1;
            DisplayName = Player.FunctionCatalog[(FunctionType.RockMiner, Level)].DisplayName!;
            Description = Player.FunctionCatalog[(FunctionType.RockMiner, Level)].Description!;
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
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public Dictionary<int, Tuple<int, int>> FunctionAttributes { get; set; }

        public GasSiphon()
        {
            Level = 1;
            DisplayName = Player.FunctionCatalog[(FunctionType.GasSiphon, Level)].DisplayName!;
            Description = Player.FunctionCatalog[(FunctionType.GasSiphon, Level)].Description!;
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
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public Dictionary<int, Tuple<int, int>> FunctionAttributes { get; set; }

        public CollectClaw()
        {
            Level = 1;
            DisplayName = Player.FunctionCatalog[(FunctionType.CollectClaw, Level)].DisplayName!;
            Description = Player.FunctionCatalog[(FunctionType.CollectClaw, Level)].Description!;
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
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public Dictionary<int, int> FunctionAttributes { get; set; }

        public CargoCapacity()
        {
            Level = 1;
            DisplayName = Player.FunctionCatalog[(FunctionType.CargoCapacity, Level)].DisplayName!;
            Description = Player.FunctionCatalog[(FunctionType.CargoCapacity, Level)].Description!;
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
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public Dictionary<int, double> FunctionAttributes { get; set; }

        public FuelCapacity()
        {
            Level = 1;
            DisplayName = Player.FunctionCatalog[(FunctionType.FuelCapacity, Level)].DisplayName!;
            Description = Player.FunctionCatalog[(FunctionType.FuelCapacity, Level)].Description!;
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
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public Dictionary<int, double> FunctionAttributes { get; set; }

        public HullIntegrity()
        {
            Level = 1;
            DisplayName = Player.FunctionCatalog[(FunctionType.HullIntegrity, Level)].DisplayName!;
            Description = Player.FunctionCatalog[(FunctionType.HullIntegrity, Level)].Description!;
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
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public Dictionary<int, int> FunctionAttributes { get; set; }

        public AirCapacity()
        {
            Level = 1;
            DisplayName = Player.FunctionCatalog[(FunctionType.AirCapacity, Level)].DisplayName!;
            Description = Player.FunctionCatalog[(FunctionType.AirCapacity, Level)].Description!;
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