using System;
using System.Runtime.CompilerServices;

namespace SpaceExploration
{
    partial class Player
    {
        public static T GetFunctionAttribute<T>(FunctionType functionType)
        {
            IFunctionInit function = Functions[functionType];
            T functionAttribute = ((IFunctionInit<T>)function).Attribute;

            return functionAttribute;
        }

        public static void UpdateFunction(FunctionType functionType)
        {
            IFunctionInit function = Functions[functionType];
            int level = ++function.Level;
            IFunction catalogData = FunctionCatalog[(functionType, level)] ;

            function.DisplayName = catalogData.DisplayName;
            function.Description = catalogData.Description;
            ((dynamic)function).Attribute = ((dynamic)catalogData).Attribute;
        }
        
        public static readonly Dictionary<(FunctionType, int), IFunctionData> FunctionCatalog = new Dictionary<(FunctionType, int), IFunctionData>()
        {
            [(FunctionType.SystemScanner, 1)] = new FunctionData<int>(
                "System Scanner",
                "Basic Scanner",
                "Star system scanner capable of scanning nearby star systems.",
                5, // Radius of scan for star systems
                new List<FunctionExchange>
                {
                    new(ElementType.Silicon, 20),
                    new(ElementType.Copper, 15),
                    new(ElementType.Aluminum, 8)
                }
            ),
            [(FunctionType.SystemScanner, 2)] = new FunctionData<int>(
                "System Scanner",
                "Advanced Scanner",
                "Improved scan range detects distant star systems hundreds of light years apart.",
                10,
                new List<FunctionExchange>
                {
                    new(ElementType.Silicon, 22),
                    new(ElementType.Copper, 16),
                    new(ElementType.Aluminum, 6),
                    new(ElementType.Nickel, 5),
                    new(ElementType.Titanium, 3)
                }
            ),
            [(FunctionType.SystemScanner, 3)] = new FunctionData<int>(
                "System Scanner",
                "Flagship Scanner",
                "Military-grade scanner spots star systems up to the outer reaches of the galaxy.",
                15,
                new List<FunctionExchange>
                {
                    new(ElementType.Silicon, 24),
                    new(ElementType.Copper, 17),
                    new(ElementType.Nickel, 8),
                    new(ElementType.Titanium, 8),
                    new(ElementType.Helium, 3),
                    new(ElementType.Antimatter, 1)
                }
            ),
            [(FunctionType.CelestialScanner, 1)] = new FunctionData<List<PlanetType>>(
                "Planet Sensor",
                "Optical Sensor",
                "Light-based celestial sensor can detect planets passing in front of their star.",
                new List<PlanetType>() // Planet types detectablable within star system
                {
                    PlanetType.Rock,
                    PlanetType.Ice,
                    PlanetType.Gas,
                    PlanetType.Terran
                },
                new List<FunctionExchange>
                {
                    new(ElementType.Silicon, 25),
                    new(ElementType.Helium, 10),
                    new(ElementType.Magnesium, 6)
                }
            ),
            [(FunctionType.CelestialScanner, 2)] = new FunctionData<List<PlanetType>>(
                "Planet Sensor",
                "Thermal Sensor",
                "Uses thermal signatures to discover heat-emitting planets in low visibility.",
                new List<PlanetType>()
                {
                    PlanetType.Toxic,
                    PlanetType.Volcanic,
                    PlanetType.Ocean,
                    PlanetType.Desert
                },
                new List<FunctionExchange>
                {
                    new(ElementType.Silicon, 27),
                    new(ElementType.Helium, 12),
                    new(ElementType.Magnesium, 10),
                    new(ElementType.Uranium, 3)
                }
            ),
            [(FunctionType.CelestialScanner, 3)] = new FunctionData<List<PlanetType>>(
                "Planet Sensor",
                "Gamma Resonance Sensor",
                "Scans for gamma radiation to detect unknown and anomalous planets.",
                new List<PlanetType>()
                {
                    PlanetType.Abandoned,
                    PlanetType.Synthetic,
                    PlanetType.Ruined,
                    PlanetType.Dark
                },
                new List<FunctionExchange>
                {
                    new(ElementType.Silicon, 26),
                    new(ElementType.Helium, 11),
                    new(ElementType.Magnesium, 15),
                    new(ElementType.Uranium, 6),
                    new(ElementType.Antimatter, 1)
                }
            ),
            [(FunctionType.RockMiner, 1)] = new FunctionData<Tuple<int, int>>(
                "Rock Miner",
                "Industrial Drill",
                "Mines for materials using an iron drill on surface sites.",
                new Tuple<int, int>(3, 5), // Min and max units that can be extracted per attempt
                new List<FunctionExchange>
                {
                    new(ElementType.Iron, 25),
                    new(ElementType.Aluminum, 15),
                    new(ElementType.Copper, 7)
                }
            ),
            [(FunctionType.RockMiner, 2)] = new FunctionData<Tuple<int, int>>(
                "Rock Miner",
                "Diamond Drill",
                "Carbon-based tip allows for excavation and extraction deep underground.",
                new Tuple<int, int>(5, 7),
                new List<FunctionExchange>
                {
                    new(ElementType.Iron, 24),
                    new(ElementType.Aluminum, 15),
                    new(ElementType.Copper, 8),
                    new(ElementType.Nickel, 6),
                    new(ElementType.Carbon, 7)
                }
            ),
            [(FunctionType.RockMiner, 3)] = new FunctionData<Tuple<int, int>>(
                "Rock Miner",
                "Laser Scraper",
                "High-energy laser excavator can extract materials from a planet's mantle.",
                new Tuple<int, int>(7, 10),
                new List<FunctionExchange>
                {
                    new(ElementType.Iron, 22),
                    new(ElementType.Aluminum, 15),
                    new(ElementType.Copper, 3),
                    new(ElementType.Nickel, 9),
                    new(ElementType.Titanium, 5),
                    new(ElementType.Helium, 4),
                    new(ElementType.Antimatter, 1)
                }
            ),
            [(FunctionType.GasSiphon, 1)] = new FunctionData<Tuple<int, int>>(
                "Gas Siphon",
                "Atmospheric Suction",
                "Intake technology collects gases in rudimentary storage.",
                new Tuple<int, int>(5, 8),
                new List<FunctionExchange>
                {
                    new(ElementType.Silicon, 20), // Min and max units that can be extracted per attempt
                    new(ElementType.Copper, 13),
                    new(ElementType.CarbonDioxide, 10)
                }
            ),
            [(FunctionType.GasSiphon, 2)] = new FunctionData<Tuple<int, int>>(
                "Gas Siphon",
                "Cryogenic Ingestion",
                "Collects, cools, and stabilizes gases under extremely low temperatures.",
                new Tuple<int, int>(8, 12),
                new List<FunctionExchange>
                {
                    new(ElementType.Silicon, 21),
                    new(ElementType.Copper, 15),
                    new(ElementType.CarbonDioxide, 5),
                    new(ElementType.Nitrogen, 5),
                    new(ElementType.Water, 5),
                    new(ElementType.Nickel, 3)
                }
            ),
            [(FunctionType.GasSiphon, 3)] = new FunctionData<Tuple<int, int>>(
                "Gas Siphon",
                "Quantum Containment",
                "Stores unstable gaseous matter using gravitation and electromagnetic fields.",
                new Tuple<int, int>(12, 18),
                new List<FunctionExchange>
                {
                    new(ElementType.Silicon, 22),
                    new(ElementType.Copper, 16),
                    new(ElementType.CarbonDioxide, 6),
                    new(ElementType.Nickel, 8),
                    new(ElementType.Uranium, 6),
                    new(ElementType.Antimatter, 1)
                }
            ),
            [(FunctionType.CollectClaw, 1)] = new FunctionData<Tuple<int, int>>(
                "Collect Claw",
                "Mechanical Claw",
                "Retrieves delicate and unstable materials with care and precision.",
                new Tuple<int, int>(1, 2),
                new List<FunctionExchange>
                {
                    new(ElementType.Copper, 18), // Min and max units that can be extracted per attempt
                    new(ElementType.Silicon, 16),
                    new(ElementType.Aluminum, 5)
                }
            ),
            [(FunctionType.CollectClaw, 2)] = new FunctionData<Tuple<int, int>>(
                "Collect Claw",
                "Field Initiator",
                "Creates a containment field that draws objects using electromagnetic forces.",
                new Tuple<int, int>(2, 3),
                new List<FunctionExchange>
                {
                    new(ElementType.Copper, 17),
                    new(ElementType.Silicon, 15),
                    new(ElementType.Aluminum, 10),
                    new(ElementType.Magnesium, 10)
                }
            ),
            [(FunctionType.CollectClaw, 3)] = new FunctionData<Tuple<int, int>>(
                "Collect Claw",
                "Nuclear Stabilizer",
                "Uses a radiation-absorbant claw to store radiactive materials in decay-resistant storage.",
                new Tuple<int, int>(3, 5),
                new List<FunctionExchange>
                {
                    new(ElementType.Copper, 14),
                    new(ElementType.Silicon, 12),
                    new(ElementType.Aluminum, 4),
                    new(ElementType.Magnesium, 15),
                    new(ElementType.Uranium, 8),
                    new(ElementType.Titanium, 8),
                    new(ElementType.Antimatter, 1)
                }
            ),
            [(FunctionType.CargoCapacity, 1)] = new FunctionData<int>(
                "Cargo Capacity",
                "Standard Storage",
                "Simple storage structure for containing raw materials.",
                200, // Maximum cargo capacity for all elements
                new List<FunctionExchange>
                {
                    new(ElementType.Iron, 25),
                    new(ElementType.Aluminum, 15)
                }
            ),
            [(FunctionType.CargoCapacity, 2)] = new FunctionData<int>(
                "Cargo Capacity",
                "Vacuum Storage",
                "Greater compression of materials by vacuuming out air and increasing pressure.",
                350,
                new List<FunctionExchange>
                {
                    new(ElementType.Iron, 20),
                    new(ElementType.Aluminum, 10),
                    new(ElementType.Carbon, 20)
                }
            ),
            [(FunctionType.CargoCapacity, 3)] = new FunctionData<int>(
                "Cargo Capacity",
                "Quantum Compression",
                "Ultimate compression of matter by temporarily reducing distances between atoms.",
                500,
                new List<FunctionExchange>
                {
                    new(ElementType.Iron, 20),
                    new(ElementType.Aluminum, 10),
                    new(ElementType.Carbon, 25),
                    new(ElementType.Titanium, 5),
                    new(ElementType.Antimatter, 1)
                }
            ),
            [(FunctionType.FuelCapacity, 1)] = new FunctionData<double>(
                "Fuel Capacity",
                "Fuel Tank",
                "Generic fuel storage for containing most fuel types.",
                50, // How much fuel the PLayer's ship can hold
                new List<FunctionExchange>
                {
                    new(ElementType.Aluminum, 18),
                    new(ElementType.Copper, 12),
                    new(ElementType.CarbonDioxide, 10)
                }
            ),
            [(FunctionType.FuelCapacity, 2)] = new FunctionData<double>(
                "Fuel Capacity",
                "Coolant Tank",
                "Fuel maintained using coolant and medium fluids.",
                125,
                new List<FunctionExchange>
                {
                    new(ElementType.Aluminum, 18),
                    new(ElementType.Copper, 12),
                    new(ElementType.CarbonDioxide, 8),
                    new(ElementType.Methane, 14)
                }
            ),
            [(FunctionType.FuelCapacity, 3)] = new FunctionData<double>(
                "Fuel Capacity",
                "Stateful Silo",
                "Advancing cooling and subduing tech keeps fuel in a single, controlled state.",
                250,
                new List<FunctionExchange>
                {
                    new(ElementType.Aluminum, 18),
                    new(ElementType.Copper, 12),
                    new(ElementType.Methane, 12),
                    new(ElementType.Ammonia, 10),
                    new(ElementType.Chlorine, 8),
                    new(ElementType.Antimatter, 1)
                }
            ),
            [(FunctionType.HullIntegrity, 1)] = new FunctionData<double>(
                "Hull Integrity",
                "Iron Plating",
                "Reinforced iron armor to protect the ship from radiation and enemy fire.",
                75, // How many hitpoints the ship hull has before failing.
                new List<FunctionExchange>
                {
                    new(ElementType.Iron, 23),
                    new(ElementType.Aluminum, 18)
                }
            ),
            [(FunctionType.HullIntegrity, 2)] = new FunctionData<double>(
                "Hull Integrity",
                "Composite Shell",
                "Carbon-composite material with high structural integrity used to protect the ship.",
                150,
                new List<FunctionExchange>
                {
                    new(ElementType.Iron, 20),
                    new(ElementType.Aluminum, 16),
                    new(ElementType.Carbon, 8),
                    new(ElementType.Nickel, 6)
                }
            ),
            [(FunctionType.HullIntegrity, 3)] = new FunctionData<double>(
                "Hull Integrity",
                "Radiation Shield",
                "Uses nuclear force to repel radiation and reflect most high-energy beams.",
                300,
                new List<FunctionExchange>
                {
                    new(ElementType.Iron, 20),
                    new(ElementType.Aluminum, 16),
                    new(ElementType.Carbon, 10),
                    new(ElementType.Nickel, 12),
                    new(ElementType.Uranium, 4),
                    new(ElementType.Antimatter, 1)
                }
            ),
            [(FunctionType.AirCapacity, 1)] = new FunctionData<double>(
                "Air Capacity",
                "Atmosphere Regulator",
                "Diffusion system for releasing breathable air into the ship.",
                100, // How much air the PLayer's ship can hold
                new List<FunctionExchange>
                {
                    new(ElementType.Copper, 22),
                    new(ElementType.Aluminum, 10),
                    new(ElementType.CarbonDioxide, 9)
                }
            ),
            [(FunctionType.AirCapacity, 2)] = new FunctionData<double>(
                "Air Capacity",
                "Smart Circulation",
                "Systematic checks and adjustments on air composition keeps it clean and low toxicity.",
                250,
                new List<FunctionExchange>
                {
                    new(ElementType.Copper, 20),
                    new(ElementType.Aluminum, 10),
                    new(ElementType.CarbonDioxide, 10),
                    new(ElementType.Nitrogen, 5),
                    new(ElementType.Silicon, 5)
                }
            ),
            [(FunctionType.AirCapacity, 3)] = new FunctionData<double>(
                "Air Capacity",
                "Nanobot Purification",
                "Releases nanobot tech into atmosphere to purify air molecules at the microscopic level.",
                400,
                new List<FunctionExchange>
                {
                    new(ElementType.Copper, 18),
                    new(ElementType.Aluminum, 9),
                    new(ElementType.CarbonDioxide, 11),
                    new(ElementType.Nitrogen, 8),
                    new(ElementType.Silicon, 8),
                    new(ElementType.Sulfur, 6),
                    new(ElementType.Antimatter, 1)
                }
            )
        };
    }

    public interface IFunction
    {
        string FunctionName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
    }

    public interface IFunctionInit : IFunction
    {
        public int Level { get; set; }
    }

    public interface IFunctionInit<T> : IFunctionInit
    {
        public T Attribute { get; set; }
    }

    public interface IFunctionData : IFunction
    {
        List<FunctionExchange> FunctionCosts { get; }
    }

    public sealed class FunctionData<T> : IFunctionData
    {
        public string FunctionName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public T Attribute { get; set; }
        public List<FunctionExchange> FunctionCosts { get; set; }

        public FunctionData(
            string functionName,
            string displayName,
            string description,
            T attribute,
            List<FunctionExchange> functionCosts
        )
        {
            FunctionName = functionName;
            DisplayName = displayName;
            Description = description;
            Attribute = attribute;
            FunctionCosts = functionCosts;
        }
    }

    public sealed record FunctionExchange(
        ElementType ElementType, // Which element is required to upgrade the function
        int InputAmount // Input amount of element needed to upgrade the function
    );

    public class SystemScanner : IFunctionInit<int>
    {
        public int Level { get; set; }
        public string FunctionName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public int Attribute { get; set; }

        public SystemScanner()
        {
            Level = 1;
            FunctionName = Player.FunctionCatalog[(FunctionType.SystemScanner, Level)].FunctionName; 
            DisplayName = Player.FunctionCatalog[(FunctionType.SystemScanner, Level)].DisplayName;
            Description = Player.FunctionCatalog[(FunctionType.SystemScanner, Level)].Description;
            Attribute = ((FunctionData<int>)Player.FunctionCatalog[(FunctionType.SystemScanner, Level)]).Attribute;
        }
    }

    public class CelestialScanner : IFunctionInit<List<PlanetType>>
    {
        public int Level { get; set; }
        public string FunctionName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public List<PlanetType> Attribute { get; set; }

        public CelestialScanner()
        {
            Level = 1;
            FunctionName = Player.FunctionCatalog[(FunctionType.CelestialScanner, Level)].FunctionName; 
            DisplayName = Player.FunctionCatalog[(FunctionType.CelestialScanner, Level)].DisplayName;
            Description = Player.FunctionCatalog[(FunctionType.CelestialScanner, Level)].Description;
            Attribute = ((FunctionData<List<PlanetType>>)Player.FunctionCatalog[(FunctionType.CelestialScanner, Level)]).Attribute;
        }
    }

    public class RockMiner : IFunctionInit<Tuple<int, int>>
    {
        public int Level { get; set; }
        public string FunctionName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public Tuple<int, int> Attribute { get; set; }

        public RockMiner()
        {
            Level = 1;
            FunctionName = Player.FunctionCatalog[(FunctionType.RockMiner, Level)].FunctionName; 
            DisplayName = Player.FunctionCatalog[(FunctionType.RockMiner, Level)].DisplayName;
            Description = Player.FunctionCatalog[(FunctionType.RockMiner, Level)].Description;
            Attribute = ((FunctionData<Tuple<int, int>>)Player.FunctionCatalog[(FunctionType.RockMiner, Level)]).Attribute;
        }
    }

    public class GasSiphon : IFunctionInit<Tuple<int, int>>
    {
        public int Level { get; set; }
        public string FunctionName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public Tuple<int, int> Attribute { get; set; }

        public GasSiphon()
        {
            Level = 1;
            FunctionName = Player.FunctionCatalog[(FunctionType.GasSiphon, Level)].FunctionName; 
            DisplayName = Player.FunctionCatalog[(FunctionType.GasSiphon, Level)].DisplayName;
            Description = Player.FunctionCatalog[(FunctionType.GasSiphon, Level)].Description;
            Attribute = ((FunctionData<Tuple<int, int>>)Player.FunctionCatalog[(FunctionType.GasSiphon, Level)]).Attribute;
        }
    }

    public class CollectClaw : IFunctionInit<Tuple<int, int>>
    {
        public int Level { get; set; }
        public string FunctionName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public Tuple<int, int> Attribute { get; set; }

        public CollectClaw()
        {
            Level = 1;
            FunctionName = Player.FunctionCatalog[(FunctionType.CollectClaw, Level)].FunctionName; 
            DisplayName = Player.FunctionCatalog[(FunctionType.CollectClaw, Level)].DisplayName;
            Description = Player.FunctionCatalog[(FunctionType.CollectClaw, Level)].Description;
            Attribute = ((FunctionData<Tuple<int, int>>)Player.FunctionCatalog[(FunctionType.CollectClaw, Level)]).Attribute;
        }
    }

    public class CargoCapacity : IFunctionInit<int>
    {
        public int Level { get; set; }
        public string FunctionName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public int Attribute { get; set; }

        public CargoCapacity()
        {
            Level = 1;
            FunctionName = Player.FunctionCatalog[(FunctionType.CargoCapacity, Level)].FunctionName; 
            DisplayName = Player.FunctionCatalog[(FunctionType.CargoCapacity, Level)].DisplayName;
            Description = Player.FunctionCatalog[(FunctionType.CargoCapacity, Level)].Description;
            Attribute = ((FunctionData<int>)Player.FunctionCatalog[(FunctionType.CargoCapacity, Level)]).Attribute;
        }
    }

    public class FuelCapacity : IFunctionInit<double>
    {
        public int Level { get; set; }
        public string FunctionName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public double Attribute { get; set; }

        public FuelCapacity()
        {
            Level = 1;
            FunctionName = Player.FunctionCatalog[(FunctionType.FuelCapacity, Level)].FunctionName; 
            DisplayName = Player.FunctionCatalog[(FunctionType.FuelCapacity, Level)].DisplayName;
            Description = Player.FunctionCatalog[(FunctionType.FuelCapacity, Level)].Description;
            Attribute = ((FunctionData<double>)Player.FunctionCatalog[(FunctionType.FuelCapacity, Level)]).Attribute;
        }
    }

    public class HullIntegrity : IFunctionInit<double>
    {
        public int Level { get; set; }
        public string FunctionName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public double Attribute { get; set; }

        public HullIntegrity()
        {
            Level = 1;
            FunctionName = Player.FunctionCatalog[(FunctionType.HullIntegrity, Level)].FunctionName; 
            DisplayName = Player.FunctionCatalog[(FunctionType.HullIntegrity, Level)].DisplayName;
            Description = Player.FunctionCatalog[(FunctionType.HullIntegrity, Level)].Description;
            Attribute = ((FunctionData<double>)Player.FunctionCatalog[(FunctionType.HullIntegrity, Level)]).Attribute;
        }
    }

    public class AirCapacity : IFunctionInit<double>
    {
        public int Level { get; set; }
        public string FunctionName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public double Attribute { get; set; }

        public AirCapacity()
        {
            Level = 1;
            FunctionName = Player.FunctionCatalog[(FunctionType.AirCapacity, Level)].FunctionName; 
            DisplayName = Player.FunctionCatalog[(FunctionType.AirCapacity, Level)].DisplayName;
            Description = Player.FunctionCatalog[(FunctionType.AirCapacity, Level)].Description;
            Attribute = ((FunctionData<double>)Player.FunctionCatalog[(FunctionType.AirCapacity, Level)]).Attribute;
        }
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