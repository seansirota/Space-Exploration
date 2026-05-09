using System;

namespace SpaceExploration
{
    class Element
    {
        public enum ElementName
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