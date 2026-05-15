using System;
using Spectre.Console;

namespace SpaceExploration
{
    class Ship
    {
        public static async Task ConvertResources()
        {
            bool stayInMenu = false;

            do
            {
                Console.WriteLine("Showing all ship resources...");
                await Task.Delay(2000);

                Table resourceTable = new Table().Border(TableBorder.Rounded).ShowHeaders();
                resourceTable.AddColumn(new TableColumn("Option").NoWrap());
                resourceTable.AddColumn(new TableColumn("Resource").NoWrap());
                resourceTable.AddColumn(new TableColumn("Owned").NoWrap());
                resourceTable.AddColumn(new TableColumn("Maximum").NoWrap());

                int count = 1;
                string option;
                string resourceName;
                double ownedResource;
                double maxResource;

                foreach (KeyValuePair<ResourceType, double> resource in Player.ResourceAmounts)
                {
                    option = count++.ToString();
                    resourceName = Player.ResourceCatalog[resource.Key].DisplayName!;
                    ownedResource = resource.Value;

                    if (resource.Key == ResourceType.Air)
                    {
                        maxResource = Player.GetFunction<int>(Player.ResourceCatalog[resource.Key].Function);
                        resourceTable.AddRow(option.ToString(), resourceName.ToString(), $"{(int)ownedResource} units", $"{(int)maxResource} units");
                    }
                        
                    else
                    {
                        maxResource = Player.GetFunction<double>(Player.ResourceCatalog[resource.Key].Function);
                        resourceTable.AddRow(option.ToString(), resourceName.ToString(), $"{Math.Round(ownedResource, 2)} units", $"{Math.Round(maxResource, 0)} units");
                    }
                }

                string? playerEntry;
                bool invalidResponse;
                ResourceType? rawResourceOption = null;

                do
                {
                    AnsiConsole.Write(resourceTable);
                    Console.WriteLine("Enter a number to choose a resource to produce. Enter X to exit back to the previous menu.");
                    playerEntry = Console.ReadLine();
                    invalidResponse = false;

                    if (playerEntry?.Equals("X", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        Console.WriteLine("Exiting resource menu...");
                        await Task.Delay(2000);
                        return;
                    }

                    if (!int.TryParse(playerEntry, out int result) || result < 1 || result >= count)
                    {
                        Console.WriteLine("Invalid option. Try again.");
                        await Task.Delay(2000);
                        invalidResponse = true;
                        continue;
                    }

                    rawResourceOption = Player.ResourceAmounts.ElementAt(result - 1).Key;
                } while (invalidResponse);

                if (rawResourceOption is not ResourceType resourceOption)
                    return;

                Table conversionTable = new Table().Border(TableBorder.Rounded).ShowHeaders();
                conversionTable.AddColumn(new TableColumn("Option").NoWrap());
                conversionTable.AddColumn(new TableColumn("Element").NoWrap());
                conversionTable.AddColumn(new TableColumn("Amount").NoWrap());
                conversionTable.AddColumn(new TableColumn("Owned").NoWrap());
                conversionTable.AddColumn(new TableColumn("Yield").NoWrap());

                count = 1;
                string elementName;
                int ownedAmount;
                int yieldAmount;
                ElementType? rawElementOption = null;

                Console.WriteLine("Displaying available element options for resource conversion...");
                await Task.Delay(2000);

                foreach (Player.ResourceExchange resourceExchange in Player.ResourceCatalog[resourceOption].ElementCosts)
                {
                    option = count++.ToString();
                    elementName = Element.ElementCatalog[resourceExchange.ElementType].DisplayName!;
                    ownedAmount = Player.ElementAmounts[resourceExchange.ElementType];
                    yieldAmount = resourceExchange.OutputAmount;

                    conversionTable.AddRow(option.ToString(), elementName, "1 unit", $"{ownedAmount} units", $"{yieldAmount} units");
                }

                do
                {
                    AnsiConsole.Write(conversionTable);
                    Console.WriteLine("Enter a number to choose an element to exchange. Enter C to exit back to the previous menu.");
                    playerEntry = Console.ReadLine();
                    invalidResponse = false;

                    if (playerEntry?.Equals("X", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        Console.WriteLine("Cancelling resource conversion...");
                        await Task.Delay(2000);
                        return;
                    }

                    if (!int.TryParse(playerEntry, out int result) || result < 1 || result >= count)
                    {
                        Console.WriteLine("Invalid option. Try again.");
                        await Task.Delay(2000);
                        invalidResponse = true;
                        continue;
                    }

                    rawElementOption = Player.ResourceCatalog[resourceOption].ElementCosts.ElementAt(result - 1).ElementType;
                } while (invalidResponse);

                if (rawElementOption is not ElementType elementOption)
                    return;

                int resourceMultiplier = 1;

                do
                {
                    Console.WriteLine("How many units of this resource do you wish to produce?");
                    playerEntry = Console.ReadLine();
                    invalidResponse = false;

                    if (!int.TryParse(playerEntry, out int result))
                    {
                        Console.WriteLine("Invalid option. Try again.");
                        await Task.Delay(2000);
                        invalidResponse = true;
                        continue;
                    }

                    resourceMultiplier = result;
                } while (invalidResponse);

                Console.WriteLine("Converting resources...");
                await Task.Delay(2000);

                bool validated = false;
                bool converted = false;
                int outputAmount = Player.ResourceCatalog[resourceOption].ElementCosts.Find(r => r.ElementType == elementOption)!.OutputAmount;

                validated = await Element.TransactElements(new Dictionary<ElementType, int>() { {elementOption, -1 * resourceMultiplier} }, false, true) &&
                            await TransactResources(new Dictionary<ResourceType, int>() { {resourceOption, outputAmount * resourceMultiplier} }, false, true);

                if (validated)
                    converted = await Element.TransactElements(new Dictionary<ElementType, int>() { {elementOption, -1 * resourceMultiplier} }, true, false, true) &&
                                await TransactResources(new Dictionary<ResourceType, int>() { {resourceOption, outputAmount * resourceMultiplier} }, true, false, true);


                Console.WriteLine(converted ? "Completed conversion." : "Conversion canceled.");
                await Task.Delay(2000);

                do
                {
                    Console.WriteLine("Do you wish to continuing viewing resources? (Y/N)");
                    playerEntry = Console.ReadLine();
                    invalidResponse = false;

                    if (playerEntry?.Equals("Y", StringComparison.OrdinalIgnoreCase) == true)
                        stayInMenu = true;
                    else if (playerEntry?.Equals("N", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        Console.WriteLine("Exiting resource menu...");
                        await Task.Delay(2000);
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Invalid command. Try again.");
                        await Task.Delay(2000);
                        invalidResponse = true;
                    }
                } while (invalidResponse);
            } while (stayInMenu);
        }


        public static async Task UpgradeFunctions()
        {
            Console.WriteLine("Showing all ship functions...");
            await Task.Delay(2000);

            Table functionTable = new Table().Border(TableBorder.Rounded).ShowHeaders();
            functionTable.AddColumn(new TableColumn("Option").NoWrap());
            functionTable.AddColumn(new TableColumn("Function").NoWrap());
            functionTable.AddColumn(new TableColumn("Level").NoWrap());
            functionTable.AddColumn(new TableColumn("Description").NoWrap());

        }

        public static async Task ConfigureAutomations()
        {
            
        }

        public static async Task<bool> TransactResources(Dictionary<ResourceType, int> resources, bool verbose = false, bool validate = false, bool skipValidation = false)
        {
            double have;
            double delta;
            double cap;
            string? name;
            bool invalidResponse;
            bool response = false;

            foreach (KeyValuePair<ResourceType, int> resource in resources)
            {
                if (skipValidation)
                    break;

                if (resource.Key == ResourceType.Air)
                    cap = Player.GetFunction<int>(Player.ResourceCatalog[resource.Key].Function);
                else
                    cap = Player.GetFunction<double>(Player.ResourceCatalog[resource.Key].Function);

                have = Player.ResourceAmounts[resource.Key];
                delta = resource.Value;
                name = Player.ResourceCatalog[resource.Key].DisplayName;

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

                    if (have + delta > cap)
                    {
                        do
                        {
                            invalidResponse = false;

                            Console.WriteLine($"Your total {name} capacity is {cap} units and can't store {Math.Round(delta - (cap - have), 2)} units, the rest will be discarded.");
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

            foreach (KeyValuePair<ResourceType, int> resource in resources)
            {
                if (resource.Key == ResourceType.Air)
                    cap = Player.GetFunction<int>(Player.ResourceCatalog[resource.Key].Function);
                else
                    cap = Player.GetFunction<double>(Player.ResourceCatalog[resource.Key].Function);
                    
                have = Player.ResourceAmounts[resource.Key];
                delta = resource.Value;
                name = Player.ResourceCatalog[resource.Key].DisplayName;

                if (verbose)
                {
                    Console.WriteLine($"Change of {delta} units of {name}...");
                    await Task.Delay(500);
                }

                if (have + delta < cap)
                    Player.ResourceAmounts[resource.Key] += delta;
                else
                    Player.ResourceAmounts[resource.Key] = cap;
            }

            await Task.Delay(500);
            response = true;
            return response;
        }
    }
}