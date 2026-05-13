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
                        resourceTable.AddRow(option.ToString(), resourceName.ToString(), $"{Math.Round(ownedResource, 0)} units", $"{Math.Round(maxResource, 0)} units");
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
                conversionTable.AddColumn(new TableColumn("Yield").NoWrap());

                count = 1;
                string elementName;
                int yieldAmount;
                ElementType? rawElementOption = null;

                Console.WriteLine("Displaying available element options for resource conversion...");
                await Task.Delay(2000);

                foreach (Player.ResourceExchange resourceExchange in Player.ResourceCatalog[resourceOption].ElementCosts)
                {
                    option = count++.ToString();
                    elementName = Element.ElementCatalog[resourceExchange.ElementType].DisplayName!;
                    yieldAmount = resourceExchange.OutputAmount;

                    conversionTable.AddRow(option.ToString(), elementName, "1 unit", $"{yieldAmount} units");
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

                Console.WriteLine("Converting resources...");
                await Task.Delay(2000);
                bool converted = await Element.TransactElements(new Dictionary<ElementType, int>() { {elementOption, -1} }, true);
                Console.WriteLine(converted ? "Completed conversion." : "Conversion canceled.");
                Player.ResourceAmounts[resourceOption] += Player.ResourceCatalog[resourceOption].ElementCosts.Find(r => r.ElementType == elementOption)!.OutputAmount; // To improve
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
            
        }

        public static async Task ConfigureAutomations()
        {
            
        }
    }
}