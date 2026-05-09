using System;

namespace SpaceExploration
{
    class Element
    {
        public static readonly Dictionary<ElementType, ElementTypeData> ElementCatalog = new Dictionary<ElementType, ElementTypeData>()
        {
            [ElementType.Hydrogen] = new("Hydrogen"),
            [ElementType.Helium] = new("Helium"),
            [ElementType.Carbon] = new("Carbon"),
            [ElementType.Nitrogen] = new("Nitrogen"),
            [ElementType.Oxygen] = new("Oxygen"),
            [ElementType.Magnesium] = new("Magnesium"),
            [ElementType.Aluminum] = new("Aluminum"),
            [ElementType.Silicon] = new("Silicon"),
            [ElementType.Sulfur] = new("Sulfur"),
            [ElementType.Chlorine] = new("Chlorine"),
            [ElementType.Titanium] = new("Titanium"),
            [ElementType.Iron] = new("Iron"),
            [ElementType.Nickel] = new("Nickel"),
            [ElementType.Copper] = new("Copper"),
            [ElementType.Uranium] = new("Uranium"),
            [ElementType.Water] = new("Water"),
            [ElementType.Methane] = new("Methane"),
            [ElementType.Ammonia] = new("Ammonia"),
            [ElementType.CarbonDioxide] = new("Carbon Dioxide"),
            [ElementType.Antimatter] = new("Antimatter")
        };

        public sealed record ElementTypeData
        (
            string? DisplayName
        );

        public enum ElementType
        {
            // Elements
            Hydrogen,           // Basic Fuel
            Helium,             // Basic Fuel
            Carbon,             // Life
            Nitrogen,           // Life
            Oxygen,             // Life
            Magnesium,          // Basic Construction
            Aluminum,           // Basic Construction
            Silicon,            // Basic Construction
            Sulfur,             // Chemicals
            Chlorine,           // Chemicals
            Titanium,           // Advanced Construction
            Iron,               // Basic Construction
            Nickel,             // Advanced Construction
            Copper,             // Advanced Construction
            Uranium,            // Advanced Fuel

            // Compounds
            Water,              // Life
            Methane,            // Basic Fuel
            Ammonia,            // Chemicals
            CarbonDioxide,      // Life
            Antimatter          // Advanced Fuel
        }
    }
}