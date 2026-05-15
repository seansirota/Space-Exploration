using System;

namespace SpaceExploration
{
    class Element
    {
        public static async Task<bool> TransactElements(Dictionary<ElementType, int> elements, bool verbose = false, bool validate = false, bool skipValidation = false)
        {
            int have;
            int delta;
            int cargo = Player.ElementAmounts.Values.Sum();
            int cap = ((CargoCapacity)Player.Functions[FunctionType.CargoCapacity]).FunctionAttributes[((CargoCapacity)Player.Functions[FunctionType.CargoCapacity]).Level];
            string? name;
            bool invalidResponse;
            bool response = false;

            if (cargo + elements.Values.Sum() > cap)
            {
                Console.WriteLine($"You don't have enough cargo capacity for this transaction.");
                await Task.Delay(2000);
                return response;
            }

            if (cargo + elements.Values.Sum() < 0)
            {
                Console.WriteLine($"You don't have enough resources to complete this transaction.");
                await Task.Delay(2000);
                return response;
            }
            
            foreach (KeyValuePair<ElementType, int> element in elements)
            {
                if (skipValidation)
                    break;

                have = Player.ElementAmounts[element.Key];
                delta = element.Value;
                name = ElementCatalog[element.Key].DisplayName;

                if (delta < 0)
                {
                    if (verbose)
                    {
                        Console.WriteLine($"You have {have} units of {name} and will give up {delta} units.");
                        await Task.Delay(500);
                    }

                    if (have < -delta)
                    {
                        Console.WriteLine($"You don't have enough {name}.");
                        await Task.Delay(2000);
                        return response;
                    }
                }
                else
                {
                    if (verbose)
                    {
                        Console.WriteLine($"You have {have} units of {name} and will get {delta} units.");
                        await Task.Delay(500);
                    }

                    if (cargo + delta > cap)
                    {
                        do
                        {
                            invalidResponse = false;

                            Console.WriteLine($"Your current cargo capacity of {cargo} won't have room for {delta - (cap - cargo)} units of {name}, the rest will be discarded.");
                            Console.WriteLine("Proceed anyway? (Y/N)");
                            string? playerEntry = Console.ReadLine();

                            if (playerEntry?.Equals("Y", StringComparison.OrdinalIgnoreCase) == true)
                            {
                                if (verbose)
                                {
                                    Console.WriteLine("Proceeding with transaction...");
                                    await Task.Delay(2000);
                                }
                            }
                            else if (playerEntry?.Equals("N", StringComparison.OrdinalIgnoreCase) == true)
                            {
                                Console.WriteLine("Canceling transaction...");
                                await Task.Delay(2000);
                                return response;
                            }
                            else
                            {
                                Console.WriteLine("Invalid command. Try again.");
                                await Task.Delay(2000);
                                invalidResponse = true;
                            }
                        } while (invalidResponse);
                    }
                }
            }

            if (validate)
                return true;

            foreach (KeyValuePair<ElementType, int> element in elements)
            {
                have = Player.ElementAmounts[element.Key];
                delta = element.Value;
                name = ElementCatalog[element.Key].DisplayName;
                cargo = Player.ElementAmounts.Values.Sum();
                cap = ((CargoCapacity)Player.Functions[FunctionType.CargoCapacity]).FunctionAttributes[((CargoCapacity)Player.Functions[FunctionType.CargoCapacity]).Level];

                if (verbose)
                {
                    Console.WriteLine($"Change of {delta} units of {name}...");
                    await Task.Delay(500);
                }

                if (cargo + delta < cap)
                    Player.ElementAmounts[element.Key] += delta;
                else
                    Player.ElementAmounts[element.Key] = cap - cargo;
            }

            await Task.Delay(500);
            response = true;
            return response;
        }

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
    }

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