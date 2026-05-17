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
                await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);

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
                    maxResource = Player.GetFunctionAttribute<double>(Player.ResourceCatalog[resource.Key].Function);

                    resourceTable.AddRow(option.ToString(), resourceName.ToString(), $"{Math.Round(ownedResource, 2)} units", $"{Math.Round(maxResource, 0)} units");
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
                        await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
                        return;
                    }

                    if (!int.TryParse(playerEntry, out int result) || result < 1 || result >= count)
                    {
                        Console.WriteLine("Invalid option. Try again.");
                        await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
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
                await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);

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
                    Console.WriteLine("Enter a number to choose an element to exchange. Enter C to cancel resource conversion.");
                    playerEntry = Console.ReadLine();
                    invalidResponse = false;

                    if (playerEntry?.Equals("C", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        Console.WriteLine("Cancelling resource conversion...");
                        await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
                        return;
                    }

                    if (!int.TryParse(playerEntry, out int result) || result < 1 || result >= count)
                    {
                        Console.WriteLine("Invalid option. Try again.");
                        await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
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
                        await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
                        invalidResponse = true;
                        continue;
                    }

                    resourceMultiplier = result;
                } while (invalidResponse);

                Console.WriteLine("Converting resources...");
                await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);

                bool validated = false;
                bool converted = false;
                int outputAmount = Player.ResourceCatalog[resourceOption].ElementCosts.Find(r => r.ElementType == elementOption)!.OutputAmount;

                validated = await Element.TransactElements(new Dictionary<ElementType, int>() { {elementOption, -1 * resourceMultiplier} }, false, true) &&
                            await TransactResources(new Dictionary<ResourceType, int>() { {resourceOption, outputAmount * resourceMultiplier} }, false, true);

                if (validated)
                    converted = await Element.TransactElements(new Dictionary<ElementType, int>() { {elementOption, -1 * resourceMultiplier} }, Program.Verbose, false, true) &&
                                await TransactResources(new Dictionary<ResourceType, int>() { {resourceOption, outputAmount * resourceMultiplier} }, Program.Verbose, false, true);


                Console.WriteLine(converted ? "Completed conversion." : "Conversion canceled.");
                await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);

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
                        await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Invalid command. Try again.");
                        await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
                        invalidResponse = true;
                    }
                } while (invalidResponse);
            } while (stayInMenu);
        }


        public static async Task UpgradeFunctions()
        {
            bool stayInMenu = false;

            do
            {
                Console.WriteLine("Showing all ship functions...");
                await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);

                Table functionTable = new Table().Border(TableBorder.Rounded).ShowHeaders();
                functionTable.AddColumn(new TableColumn("Option").NoWrap());
                functionTable.AddColumn(new TableColumn("Function").NoWrap());
                functionTable.AddColumn(new TableColumn("Name").NoWrap());
                functionTable.AddColumn(new TableColumn("Level").NoWrap());
                functionTable.AddColumn(new TableColumn("Description").NoWrap());

                int count = 1;
                string option;
                string functionName;
                string displayName;
                int level;
                string description;

                foreach (KeyValuePair<FunctionType, IFunctionInit> functionType in Player.Functions)
                {
                    option = count++.ToString();
                    functionName = functionType.Value.FunctionName;
                    displayName = functionType.Value.DisplayName;
                    level = functionType.Value.Level;
                    description = functionType.Value.Description;

                    functionTable.AddRow(option.ToString(), functionName, displayName, level.ToString(), description);
                }

                string? playerEntry;
                bool invalidResponse;
                FunctionType? rawFunctionOption = null;
                int functionLevel = 0;

                do
                {
                    AnsiConsole.Write(functionTable);
                    Console.WriteLine("Enter a number to choose a function to build or upgrade. Enter X to exit back to the previous menu.");
                    playerEntry = Console.ReadLine();
                    invalidResponse = false;

                    if (playerEntry?.Equals("X", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        Console.WriteLine("Exiting function menu...");
                        await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
                        return;
                    }

                    if (!int.TryParse(playerEntry, out int result) || result < 1 || result >= count)
                    {
                        Console.WriteLine("Invalid option. Try again.");
                        await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
                        invalidResponse = true;
                        continue;
                    }

                    rawFunctionOption = Player.Functions.ElementAt(result - 1).Key;
                    functionLevel = Player.Functions.ElementAt(result - 1).Value.Level;
                } while (invalidResponse);

                if (rawFunctionOption is not FunctionType functionOption)
                    return;
                
                Table functionCostTable = new Table().Border(TableBorder.Rounded).ShowHeaders();
                functionCostTable.AddColumn(new TableColumn("Element").NoWrap());
                functionCostTable.AddColumn(new TableColumn("Amount").NoWrap());
                functionCostTable.AddColumn(new TableColumn("Owned").NoWrap());

                string elementName;
                ElementType elementType;
                int ownedAmount;
                int requiredAmount;
                bool upgrade = functionLevel > 0;
                string upgradeText = upgrade ? "upgrade" : "build";
                Dictionary<ElementType, int> elementsPayment = new Dictionary<ElementType, int>();

                Console.WriteLine($"Displaying element amounts needed to {upgradeText} function...");
                await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);

                foreach (FunctionExchange functionExchange in Player.FunctionCatalog[(functionOption, functionLevel)].FunctionCosts)
                {
                    elementType = functionExchange.ElementType;
                    elementName = Element.ElementCatalog[elementType].DisplayName!;
                    ownedAmount = Player.ElementAmounts[elementType];
                    requiredAmount = functionExchange.InputAmount;

                    functionCostTable.AddRow(elementName, requiredAmount.ToString(), ownedAmount.ToString());
                    elementsPayment.Add(elementType, -requiredAmount);
                }

                do
                {
                    AnsiConsole.Write(functionCostTable);
                    Console.WriteLine($"These are the required element amounts to {upgradeText} this function. Do you wish to proceed? (Y/N)");
                    playerEntry = Console.ReadLine();
                    invalidResponse = false;

                    if (playerEntry?.Equals("Y", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        if (functionLevel == 3)
                        {
                            Console.WriteLine("This function has already reached the maximum level and can't be upgraded further. Exiting function menu...");
                            await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
                            return;
                        }
                    }
                    else if (playerEntry?.Equals("N", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        Console.WriteLine("Exiting function menu...");
                        await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Invalid command. Try again.");
                        await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
                        invalidResponse = true;
                    }
                } while (invalidResponse);

                Console.WriteLine($"Starting {upgradeText} of function...");
                await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);

                bool validated;
                bool upgraded = false;

                validated = await Element.TransactElements(elementsPayment, false, true);

                if (validated)
                {
                    upgraded = await Element.TransactElements(elementsPayment, Program.Verbose, false, true);
                    Player.UpdateFunction(functionOption);
                }

                Console.WriteLine(upgraded ? "Function completed." : "Function canceled.");
                await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);

                do
                {
                    Console.WriteLine("Do you wish to continuing viewing functions? (Y/N)");
                    playerEntry = Console.ReadLine();
                    invalidResponse = false;

                    if (playerEntry?.Equals("Y", StringComparison.OrdinalIgnoreCase) == true)
                        stayInMenu = true;
                    else if (playerEntry?.Equals("N", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        Console.WriteLine("Exiting function menu...");
                        await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Invalid command. Try again.");
                        await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
                        invalidResponse = true;
                    }
                } while (invalidResponse);
            } while (stayInMenu);
        }

        public static async Task ConfigureAutomations()
        {
            bool stayInMenu = false;

            do
            {
                Console.WriteLine("Displaying all ship automations...");
                await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);

                Table automationsTable = new Table().Border(TableBorder.Rounded).ShowHeaders();
                automationsTable.AddColumn(new TableColumn("Option").NoWrap());
                automationsTable.AddColumn(new TableColumn("Description").NoWrap());
                automationsTable.AddColumn(new TableColumn("Status").NoWrap());

                int count = 1;
                string option;
                string description;
                string? status;

                foreach (KeyValuePair<int, Tuple<string, object>> automation in Program.Automations)
                {
                    option = count++.ToString();
                    description = automation.Value.Item1;
                    status = (automation.Key != 3) ? automation.Value.Item2.ToString() : Program.SortMethods[(int)automation.Value.Item2];

                    automationsTable.AddRow(option, description, status!);
                }

                string? playerEntry;
                bool invalidResponse;

                do
                {
                    AnsiConsole.Write(automationsTable);
                    Console.WriteLine("Enter a number to choose which automation to toggle. Enter X to exit back to the previous menu.");
                    playerEntry = Console.ReadLine();
                    invalidResponse = false;

                    if (playerEntry?.Equals("X", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        Console.WriteLine("Exiting ship automations menu...");
                        await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
                        return;
                    }

                    if (!int.TryParse(playerEntry, out int result) || result < 1 || result >= count)
                    {
                        Console.WriteLine("Invalid option. Try again.");
                        await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
                        invalidResponse = true;
                        continue;
                    }

                    UpdateAutomations(result);
                    Console.WriteLine("Updated automation setting.");
                    await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
                    stayInMenu = true;
                } while(invalidResponse);
            } while (stayInMenu);
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

                have = Player.ResourceAmounts[resource.Key];
                delta = resource.Value;
                cap = Player.GetFunctionAttribute<double>(Player.ResourceCatalog[resource.Key].Function);
                name = Player.ResourceCatalog[resource.Key].DisplayName;

                if (delta < 0)
                {
                    if (verbose)
                    {
                        Console.WriteLine($"You have {have} units of {name} and will give up {delta} units.");
                        await Task.Delay(Program.BaseSpeed * Program.ShortTextMultiplier);
                    }

                    if (have < -delta)
                    {
                        Console.WriteLine($"You don't have enough {name}.");
                        await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
                        return response;
                    }
                }
                else
                {
                    if (verbose)
                    {
                        Console.WriteLine($"You have {have} units of {name} and will get {delta} units.");
                        await Task.Delay(Program.BaseSpeed * Program.ShortTextMultiplier);
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
                                    await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
                                }
                            }
                            else if (playerEntry?.Equals("N", StringComparison.OrdinalIgnoreCase) == true)
                            {
                                Console.WriteLine("Canceling transaction...");
                                await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
                                return response;
                            }
                            else
                            {
                                Console.WriteLine("Invalid command. Try again.");
                                await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
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
                have = Player.ResourceAmounts[resource.Key];
                delta = resource.Value;
                cap = Player.GetFunctionAttribute<double>(Player.ResourceCatalog[resource.Key].Function);
                name = Player.ResourceCatalog[resource.Key].DisplayName;

                if (verbose)
                {
                    Console.WriteLine($"Change of {delta} units of {name}...");
                    await Task.Delay(Program.BaseSpeed * Program.ShortTextMultiplier);
                }

                if (have + delta < cap)
                    Player.ResourceAmounts[resource.Key] += delta;
                else
                    Player.ResourceAmounts[resource.Key] = cap;
            }

            await Task.Delay(Program.BaseSpeed * Program.ShortTextMultiplier);
            response = true;
            return response;
        }

        private static void UpdateAutomations(int automation)
        {
            object newValue;

            switch(automation) {
                case 1:
                    Program.Verbose = !Program.Verbose;
                    newValue = Program.Verbose;
                    break;
                case 2:
                    Program.TextShorten = !Program.TextShorten;
                    if (!Program.TextShorten)
                    {
                        Program.LongTextMultiplier *= 2;
                        Program.ShortTextMultiplier *= 2;
                    }
                    else
                    {
                        Program.LongTextMultiplier /= 2;
                        Program.ShortTextMultiplier /= 2;
                    }
                    newValue = Program.TextShorten;
                    break;
                case 3:
                    if (Program.SortPattern < 3)
                    {
                        Program.SortPattern++;
                    }
                    else
                        Program.SortPattern = 1;
                    newValue = Program.SortPattern;
                    break;
                default:
                    newValue = 0;
                    break;
            }

            Program.Automations[automation] = new Tuple<string, object>(Program.Automations[automation].Item1, newValue);
        }
    }
}