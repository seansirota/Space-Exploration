using System;
using System.Numerics;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;

namespace SpaceExploration
{
    partial class Player
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

        public static Dictionary<FunctionType, IFunctionInit> Functions = new Dictionary<FunctionType, IFunctionInit>()
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
            [ResourceType.Fuel] = GetFunctionAttribute<double>(FunctionType.FuelCapacity),
            [ResourceType.Hull] = GetFunctionAttribute<double>(FunctionType.HullIntegrity),
            [ResourceType.Air] = GetFunctionAttribute<double>(FunctionType.AirCapacity)
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
    }

    public enum ResourceType
    {
        Fuel,
        Hull,
        Air
    }
}