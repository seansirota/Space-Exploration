using System;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Runtime;
using System.Threading.Tasks;
using Spectre.Console;

namespace SpaceExploration
{
    class GameAction
    {
        private const int ExtractAttempts = 1;
        public static bool gameComplete;
        public static int gameMode = 1;
        public static int prevGameMode = 1;

        public static async Task InitiateWorld()
        {
            AnsiConsole.Profile.Width = 160;
            StarSystem.GenerateNewSystems();
            while (!gameComplete)
            {
                if (gameMode == 1)
                    await MainMenu();
                else if (gameMode == 2)
                    await SystemMenu();
                else if (gameMode == 3)
                    await VisitMenu();
                else if (gameMode == 4)
                    await CheckFunctions();
            }
        }

        public static async Task MainMenu()
        {
            bool invalidResponse;
            string? currentStarSystem = (Player.CurrentSystem is not null) ? StarSystem.Systems[Player.CurrentSystem ?? 0].Name.ToString() : "None";

            do
            {
                invalidResponse = false;
                Console.WriteLine($"""
                Current star system: {currentStarSystem}
                Current fuel level: {Math.Round(Player.ResourceAmounts[ResourceType.Fuel], 2)} / {Player.GetFunctionAttribute<double>(FunctionType.FuelCapacity)}
                """);
                Console.WriteLine("""
                Choose an action:
                1/J: Jump to new star system.
                2/V: Visit current star system.
                3/I: Check inventory.
                4/F: Check ship resources and functions.
                5/N: Leave a note on the current star system.
                6/B: View logbook.
                """);

                string? playerEntry = Console.ReadLine();

                if (playerEntry == "1" || playerEntry?.Equals("J", StringComparison.OrdinalIgnoreCase) == true)
                    await JumpSystem();
                else if (playerEntry == "2" || playerEntry?.Equals("V", StringComparison.OrdinalIgnoreCase) == true)
                {
                    if (Player.CurrentSystem is null)
                    {
                        Console.WriteLine("No current star system selected.");
                        await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
                        invalidResponse = true;
                    }
                    else
                    {
                        gameMode = 2;
                        Console.WriteLine("Flying into star system...");
                        await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
                        await SystemMenu();
                    }
                }
                else if (playerEntry == "3" || playerEntry?.Equals("I", StringComparison.OrdinalIgnoreCase) == true)
                    await CheckInventory();
                else if (playerEntry == "4" || playerEntry?.Equals("F", StringComparison.OrdinalIgnoreCase) == true)
                {
                    prevGameMode = gameMode;
                    gameMode = 4;
                    await CheckFunctions();
                }
                else if (playerEntry == "5" || playerEntry?.Equals("N", StringComparison.OrdinalIgnoreCase) == true)
                    invalidResponse = await AddNote();
                else if (playerEntry == "6" || playerEntry?.Equals("B", StringComparison.OrdinalIgnoreCase) == true)
                    await ViewLogbook();
                else
                {
                    Console.WriteLine("Invalid command. Try again.");
                    await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
                    invalidResponse = true;
                }
            } while (invalidResponse);
        }

        public static async Task SystemMenu()
        {
            bool invalidResponse;
            string? currentStarSystem = (Player.CurrentSystem is not null) ? StarSystem.Systems[Player.CurrentSystem ?? 0].Name.ToString() : "None";

            do
            {
                invalidResponse = false;
                Console.WriteLine($"""
                Current star system: {currentStarSystem}
                Current fuel level: {Math.Round(Player.ResourceAmounts[ResourceType.Fuel], 2)} / {Player.GetFunctionAttribute<double>(FunctionType.FuelCapacity)}
                """);
                Console.WriteLine("""
                Choose an action:
                1/L: Look back out to star systems.
                2/V: View stars and planets in current star system.
                3/I: Check inventory.
                4/F: Check ship resources and functions.
                5/N: Leave a note on the current star system.
                6/B: View logbook.
                """);

                string? playerEntry = Console.ReadLine();

                if (playerEntry == "1" || playerEntry?.Equals("L", StringComparison.OrdinalIgnoreCase) == true)
                {
                    gameMode = 1;
                    Console.WriteLine("Returning to galaxy view...");
                    await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
                }
                else if (playerEntry == "2" || playerEntry?.Equals("V", StringComparison.OrdinalIgnoreCase) == true)
                    await VisitStarPlanet();
                else if (playerEntry == "3" || playerEntry?.Equals("I", StringComparison.OrdinalIgnoreCase) == true)
                    await CheckInventory();
                else if (playerEntry == "4" || playerEntry?.Equals("F", StringComparison.OrdinalIgnoreCase) == true)
                {
                    prevGameMode = gameMode;
                    gameMode = 4;
                    await CheckFunctions();
                }
                else if (playerEntry == "5" || playerEntry?.Equals("N", StringComparison.OrdinalIgnoreCase) == true)
                    invalidResponse = await AddNote();
                else if (playerEntry == "6" || playerEntry?.Equals("B", StringComparison.OrdinalIgnoreCase) == true)
                    await ViewLogbook();
                else
                {
                    Console.WriteLine("Invalid command. Try again.");
                    await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
                    invalidResponse = true;
                }
            } while (invalidResponse);
        }

        public static async Task VisitMenu()
        {
            bool invalidResponse;
            string? currentStarSystem = (Player.CurrentSystem is not null) ? StarSystem.Systems[Player.CurrentSystem ?? 0].Name.ToString() : "None";
            string? currentObject = (Player.CurrentObject is not null) ? Player.CurrentObject.Name : "None";
            double fuel = Player.CurrentObject switch
            {
                Star star => Star.StarCatalog[star.Type].FuelCost,
                Planet planet => Planet.PlanetCatalog[planet.Type].FuelCost,
                _ => 0
            } * 2;

            do
            {
                invalidResponse = false;
                Console.WriteLine($"""
                Current star system: {currentStarSystem}
                Current star or planet: {currentObject}
                Current fuel level: {Math.Round(Player.ResourceAmounts[ResourceType.Fuel], 2)} / {Player.GetFunctionAttribute<double>(FunctionType.FuelCapacity)}
                """);
                Console.WriteLine($"""
                Choose an action:
                1/R: Return to star system. ({Math.Round(fuel, 2)} fuel units required)
                2/E: Extract resources.
                3/I: Check inventory.
                4/F: Check ship resources and functions.
                5/N: Leave a note on the current star system.
                6/B: View logbook.
                """);

                string? playerEntry = Console.ReadLine();

                if (playerEntry == "1" || playerEntry?.Equals("R", StringComparison.OrdinalIgnoreCase) == true)
                    await ReturnToStarSystem();
                else if (playerEntry == "2" || playerEntry?.Equals("E", StringComparison.OrdinalIgnoreCase) == true)
                    await ExtractResources();
                else if (playerEntry == "3" || playerEntry?.Equals("I", StringComparison.OrdinalIgnoreCase) == true)
                    await CheckInventory();
                else if (playerEntry == "4" || playerEntry?.Equals("F", StringComparison.OrdinalIgnoreCase) == true)
                {
                    prevGameMode = gameMode;
                    gameMode = 4;
                    await CheckFunctions();
                }
                else if (playerEntry == "5" || playerEntry?.Equals("N", StringComparison.OrdinalIgnoreCase) == true)
                    invalidResponse = await AddNote();
                else if (playerEntry == "6" || playerEntry?.Equals("B", StringComparison.OrdinalIgnoreCase) == true)
                    await ViewLogbook();
                else
                {
                    Console.WriteLine("Invalid command. Try again.");
                    await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
                    invalidResponse = true;
                }
            } while (invalidResponse);
        }

        public static async Task JumpSystem()
        {
            string? currentStarSystem = (Player.CurrentSystem is not null) ? StarSystem.Systems[Player.CurrentSystem ?? 0].Name.ToString() : "None";

            Console.WriteLine($"""
            Loading nearby star systems...
            Current star system: {currentStarSystem}
            Current fuel level: {Math.Round(Player.ResourceAmounts[ResourceType.Fuel], 2)} / {Player.GetFunctionAttribute<double>(FunctionType.FuelCapacity)}
            """);
            await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
            
            List<int> nearbySystems = StarSystem.GetNearbySystems();

            Table starSystemTable = new Table().Border(TableBorder.Rounded).ShowHeaders();
            starSystemTable.AddColumn(new TableColumn("Option").NoWrap());
            starSystemTable.AddColumn(new TableColumn("Name").NoWrap());
            starSystemTable.AddColumn(new TableColumn("Fuel Cost").NoWrap());
            starSystemTable.AddColumn(new TableColumn("Direction").NoWrap());
            starSystemTable.AddColumn(new TableColumn("Visited?").NoWrap());
            starSystemTable.AddColumn(new TableColumn("Comments").NoWrap());

            int count = 1;
            int option;
            string name;
            double fuel;
            string direction;
            bool visited;
            string? note;

            foreach (int systemID in nearbySystems)
            {
                option = count++;
                name = StarSystem.Systems[systemID].Name;
                fuel = Math.Round(StarSystem.GetDistance(StarSystem.Systems[systemID]), 2);
                direction = StarSystem.GetDirection(StarSystem.Systems[systemID]);
                visited = StarSystem.Systems[systemID].Visited;
                note = StarSystem.Systems[systemID].Note;
                note ??= "";

                starSystemTable.AddRow(option.ToString(), name, $"{fuel} units", direction, visited.ToString(), note);
            }
            
            string? playerEntry;
            bool invalidResponse;
            StarSystem? destinationSystem = null;

            do
            {
                AnsiConsole.Write(starSystemTable);
                Console.WriteLine("Enter a number to choose a star system to jump to. Enter C to cancel.");
                playerEntry = Console.ReadLine();
                invalidResponse = false;

                if (playerEntry?.Equals("C", StringComparison.OrdinalIgnoreCase) == true)
                {
                    Console.WriteLine("Canceling. Returning to main selection menu...");
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

                destinationSystem = StarSystem.Systems[nearbySystems[result - 1]];

                if (StarSystem.GetDistance(destinationSystem) > Player.ResourceAmounts[ResourceType.Fuel])
                {
                    Console.WriteLine("Not enough fuel to jump to that star system. Choose another option.");
                    await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
                    invalidResponse = true;
                    continue;
                }

                Console.WriteLine($"Jumping to the {destinationSystem.Name} star system...");
                await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);

            } while (invalidResponse);

            if (destinationSystem is null)
                return;

            Player.ResourceAmounts[ResourceType.Fuel] -= StarSystem.GetDistance(StarSystem.Systems[destinationSystem.ID]);
            Player.CurrentSystem = destinationSystem.ID;
            Player.X = destinationSystem.X;
            Player.Y = destinationSystem.Y;
            destinationSystem.Visited = true;

            StarSystem.GenerateNewSystems();
        }

        public static async Task VisitStarPlanet()
        {
            if (Player.CurrentSystem == null)
            {
                Console.WriteLine("You're not located within a star system.");
                await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
                return;
            }

            Console.WriteLine($"""
            Loading stars and planets within {StarSystem.Systems[Player.CurrentSystem ?? 0].Name}...
            Current fuel level: {Math.Round(Player.ResourceAmounts[ResourceType.Fuel], 2)} / {Player.GetFunctionAttribute<double>(FunctionType.FuelCapacity)}
            """);
            await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);

            if (Player.CurrentSystem is not int currentSystem)
                return;

            List<Star> stars = StarSystem.Systems[currentSystem].Stars;
            List<Planet> planets = StarSystem.Systems[currentSystem].Planets;

            Table bodiesTable = new Table().Border(TableBorder.Rounded).ShowHeaders();
            bodiesTable.AddColumn(new TableColumn("Option").NoWrap());
            bodiesTable.AddColumn(new TableColumn("Object").NoWrap());
            bodiesTable.AddColumn(new TableColumn("Name").NoWrap());
            bodiesTable.AddColumn(new TableColumn("Fuel Cost").NoWrap());
            bodiesTable.AddColumn(new TableColumn("Type").NoWrap());
            bodiesTable.AddColumn(new TableColumn("Mass").NoWrap());
            bodiesTable.AddColumn(new TableColumn("Temperature").NoWrap());
            bodiesTable.AddColumn(new TableColumn("Visited?").NoWrap());

            int count = 1;
            int option;
            int starBorder;
            string? name;
            int fuel;
            string? type;
            double mass;
            int temperature;
            bool visited;
            List<CelObjectGeneric> celObjects = new List<CelObjectGeneric>();

            foreach (Star star in stars)
            {
                option = count++;
                name = star.Name;
                fuel = Star.StarCatalog[star.Type].FuelCost;
                type = Star.StarCatalog[star.Type].DisplayName;
                mass = Math.Round(star.Mass, 2);
                temperature = star.Temperature;
                visited = star.Visited;

                bodiesTable.AddRow(option.ToString(), "Star", name!, $"{fuel} units", type!, $"{mass} SM", $"{temperature} K", visited.ToString());
                celObjects.Add(star);
            }

            starBorder = count - 1;

            foreach (Planet planet in planets)
            {
                option = count++;
                name = planet.Name;
                fuel = Planet.PlanetCatalog[planet.Type].FuelCost;
                type = null;
                mass = Math.Round(planet.Mass, 2);
                visited = planet.Visited;

                type = GetPlanetTypeName(planet);
                bodiesTable.AddRow(option.ToString(), "Planet", name!, $"{fuel} units", type, $"{mass} PM", string.Empty, visited.ToString());
                celObjects.Add(planet);
            }

            string? playerEntry;
            bool invalidResponse;
            CelObjectGeneric destinationBody;
            string? destinationName;
            string? destinationType;

            do
            {
                AnsiConsole.Write(bodiesTable);
                Console.WriteLine("Enter a number to choose a star or planet to visit. Enter C to cancel.");
                playerEntry = Console.ReadLine();
                invalidResponse = false;

                if (playerEntry?.Equals("C", StringComparison.OrdinalIgnoreCase) == true)
                {
                    Console.WriteLine("Canceling. Returning to star and planet selection menu...");
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

                destinationBody = celObjects[result - 1];

                if (
                    result <= starBorder && Star.StarCatalog[((Star)destinationBody).Type].FuelCost > Player.ResourceAmounts[ResourceType.Fuel] ||
                    result > starBorder && Planet.PlanetCatalog[((Planet)destinationBody).Type].FuelCost > Player.ResourceAmounts[ResourceType.Fuel]
                )
                {
                    Console.WriteLine("Not enough fuel to visit that star or planet. Choose another option.");
                    await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
                    invalidResponse = true;
                    continue;
                }

                if (destinationBody is Planet)
                {
                    destinationType = GetPlanetTypeName((Planet)destinationBody);

                    if (destinationType == "???")
                    {
                        Console.WriteLine("Current celestial sensors aren't advanced enough to set reliable course to this planet.");
                        await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
                        invalidResponse = true;
                        continue;
                    }
                }

                destinationName = destinationBody.Name;

                Console.WriteLine($"Flying to {destinationName}...");
                await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);

                if (result <= starBorder)
                    Player.ResourceAmounts[ResourceType.Fuel] -= Star.StarCatalog[((Star)destinationBody).Type].FuelCost;
                else
                    Player.ResourceAmounts[ResourceType.Fuel] -= Planet.PlanetCatalog[((Planet)destinationBody).Type].FuelCost;

                Player.CurrentObject = destinationBody;
                destinationBody.Visited = true;
                gameMode = 3;
            } while (invalidResponse);
        }

        public static async Task CheckInventory()
        {
            bool stayInMenu = false;

            do
            {
                Console.WriteLine($"""
                Now loading inventory...
                Current cargo capacity: {Player.ElementAmounts.Values.Sum()} / {Player.GetFunctionAttribute<int>(FunctionType.CargoCapacity)}
                """);
                await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);

                Table elementsTable = new Table().Border(TableBorder.Rounded).ShowHeaders();
                elementsTable.AddColumn(new TableColumn("Option").NoWrap());
                elementsTable.AddColumn(new TableColumn("Element").NoWrap());
                elementsTable.AddColumn(new TableColumn("Amount").NoWrap());

                int count = 1;
                int option;
                string? element;
                int amount;

                foreach (KeyValuePair<ElementType, int> elementType in Player.ElementAmounts)
                {
                    option = count++;
                    element = Element.ElementCatalog[elementType.Key].DisplayName;
                    amount = elementType.Value;
                    
                    elementsTable.AddRow(option.ToString(), element!, $"{amount} units");
                }

                string? playerEntry;
                bool invalidResponse;
                ElementType? rawOption = null;

                do
                {
                    AnsiConsole.Write(elementsTable);
                    Console.WriteLine("Enter a number to choose an element to discard. Enter X to exit back to the previous menu.");
                    playerEntry = Console.ReadLine();
                    invalidResponse = false;

                    if (playerEntry?.Equals("X", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        Console.WriteLine("Exiting inventory menu...");
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

                    rawOption = Player.ElementAmounts.ElementAt(result - 1).Key;
                } while (invalidResponse);

                if (rawOption is not ElementType discardOption)
                    return;

                Dictionary<ElementType, int> discardElement = new Dictionary<ElementType, int>();
                int discardAmount;

                do
                {
                    Console.WriteLine($"How many units of {Element.ElementCatalog[discardOption].DisplayName} do you wish to discard?");
                    playerEntry = Console.ReadLine();
                    invalidResponse = false;

                    if (!int.TryParse(playerEntry, out int result) || result < 0 || result > Player.ElementAmounts[discardOption])
                    {
                        Console.WriteLine("Invalid option. Try again.");
                        await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
                        invalidResponse = true;
                        continue;
                    }

                    discardAmount = result;
                    discardElement[discardOption] = -discardAmount;
                } while (invalidResponse);

                Console.WriteLine("Discarding elements...");
                await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
                bool discarded = await Element.TransactElements(discardElement, Program.Verbose);
                Console.WriteLine(discarded ? "Completed disposal." : "Discard canceled.");
                await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);

                do
                {
                    Console.WriteLine("Do you wish to continuing checking out the inventory? (Y/N)");
                    playerEntry = Console.ReadLine();
                    invalidResponse = false;

                    if (playerEntry?.Equals("Y", StringComparison.OrdinalIgnoreCase) == true)
                        stayInMenu = true;
                    else if (playerEntry?.Equals("N", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        Console.WriteLine("Exiting inventory menu...");
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

        public static async Task CheckFunctions()
        {
            bool invalidResponse;

            do
            {
                invalidResponse = false;

                Console.WriteLine($"""
                Viewing player and ship information...
                Current money: {Player.Money} chromids
                Current fuel level: {Math.Round(Player.ResourceAmounts[ResourceType.Fuel], 2)} / {Player.GetFunctionAttribute<double>(FunctionType.FuelCapacity)}
                Current cargo capacity: {Player.ElementAmounts.Values.Sum()} / {Player.GetFunctionAttribute<int>(FunctionType.CargoCapacity)}
                Current hull integrity: {Math.Round(Player.ResourceAmounts[ResourceType.Hull], 2)} / {Player.GetFunctionAttribute<double>(FunctionType.HullIntegrity)}
                Current air level: {Math.Round(Player.ResourceAmounts[ResourceType.Air], 2)} / {Player.GetFunctionAttribute<double>(FunctionType.AirCapacity)}
                """);
                await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
                Console.WriteLine($"""
                Choose an action:
                1/N: Convert elements into ship resources.
                2/U: View and upgrade ship functions.
                3/A: Configure ship automations.
                4/X: Exit ship functions menu.
                """);

                string? playerEntry = Console.ReadLine();

                if (playerEntry == "1" || playerEntry?.Equals("N", StringComparison.OrdinalIgnoreCase) == true)
                    await Ship.ConvertResources();
                else if (playerEntry == "2" || playerEntry?.Equals("U", StringComparison.OrdinalIgnoreCase) == true)
                    await Ship.UpgradeFunctions();
                else if (playerEntry == "3" || playerEntry?.Equals("A", StringComparison.OrdinalIgnoreCase) == true)
                    await Ship.ConfigureAutomations();
                else if (playerEntry == "4" || playerEntry?.Equals("X", StringComparison.OrdinalIgnoreCase) == true)
                {
                    Console.WriteLine("Exiting resources and functions menu...");
                    await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
                    gameMode = prevGameMode;
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid command. Try again.");
                    await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
                    invalidResponse = true;
                }
            } while (invalidResponse);
        }

        public static async Task<bool> AddNote()
        {
            if (Player.CurrentSystem is not int currentSystem)
            {
                Console.WriteLine("Not a valid star system. Returning to main selection menu...");
                await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
                return true;
            }
            Console.WriteLine("Enter a note for this star system below.");

            StarSystem.Systems[currentSystem].Note = Console.ReadLine();
            Console.WriteLine("Note saved. Returning to main selection menu...");
            await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
            return false;
        }

        public static async Task ViewLogbook()
        {
            
        }

        public static async Task ExtractResources()
        {
            CelObjectGeneric? currentObject = Player.CurrentObject;
            (int propertySkip, object? catalogData, Dictionary<int, ElementType>? indexData) = currentObject switch
            {
                Star star => (7, (object?)Star.StarCatalog[star.Type], (Dictionary<int, ElementType>?)Star.StarIndex),
                Planet planet => (6, (object?)Planet.PlanetCatalog[planet.Type], (Dictionary<int, ElementType>?)Planet.PlanetIndex),
                _ => (0, null, null)
            };

            if (catalogData is null)
                return;

            Dictionary<ElementType, int> elements = new Dictionary<ElementType, int>();
            PropertyInfo[] properties = catalogData.GetType().GetProperties();
            int hit;
            int minAmount;
            int maxAmount;
            int amount;

            for (int i = propertySkip; i < properties.Length; i++)
            {
                object? rawChance = properties[i].GetValue(catalogData);
                object? rawElementType = indexData![i];

                if (rawChance is int chance && rawElementType is ElementType elementType)
                {
                    for (int j = 0; j < ExtractAttempts; j++)
                    {
                        hit = Program.Rand.Next(1, 101);
                        if (hit > chance)
                            continue;

                        if (
                            elementType is ElementType.Carbon ||
                            elementType is ElementType.Magnesium ||
                            elementType is ElementType.Aluminum ||
                            elementType is ElementType.Silicon ||
                            elementType is ElementType.Titanium ||
                            elementType is ElementType.Iron ||
                            elementType is ElementType.Nickel ||
                            elementType is ElementType.Copper
                        )
                        {
                            minAmount = Player.GetFunctionAttribute<Tuple<int, int>>(FunctionType.RockMiner).Item1;
                            maxAmount = Player.GetFunctionAttribute<Tuple<int, int>>(FunctionType.RockMiner).Item2;
                        }
                        else if (
                            elementType is ElementType.Hydrogen ||
                            elementType is ElementType.Helium ||
                            elementType is ElementType.Nitrogen ||
                            elementType is ElementType.Oxygen ||
                            elementType is ElementType.Sulfur ||
                            elementType is ElementType.Chlorine ||
                            elementType is ElementType.Water ||
                            elementType is ElementType.Methane ||
                            elementType is ElementType.Ammonia ||
                            elementType is ElementType.CarbonDioxide
                        )
                        {
                            minAmount = Player.GetFunctionAttribute<Tuple<int, int>>(FunctionType.GasSiphon).Item1;
                            maxAmount = Player.GetFunctionAttribute<Tuple<int, int>>(FunctionType.GasSiphon).Item2;
                        }
                        else
                        {
                            minAmount = Player.GetFunctionAttribute<Tuple<int, int>>(FunctionType.CollectClaw).Item1;
                            maxAmount = Player.GetFunctionAttribute<Tuple<int, int>>(FunctionType.CollectClaw).Item2;
                        }

                        amount = Program.Rand.Next(minAmount, maxAmount + 1);
                        if (elements.ContainsKey(elementType))
                            elements[elementType] += amount;
                        else
                            elements[elementType] = amount;
                    }
                }
            }

            Console.WriteLine("Beginning extraction of resources...");
            await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
            bool extracted = await Element.TransactElements(elements, Program.Verbose);
            Console.WriteLine(extracted ? "Completed resource extraction." : "Extraction canceled.");
            await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
        }

        public static async Task ReturnToStarSystem()
        {
            double fuel = Player.CurrentObject switch
            {
                Star star => Star.StarCatalog[star.Type].FuelCost,
                Planet planet => Planet.PlanetCatalog[planet.Type].FuelCost,
                _ => 0
            } * 2;

            if (fuel > Player.ResourceAmounts[ResourceType.Fuel])
            {
                Console.WriteLine("You don't have enough fuel to leave the planet or star.");
                await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
                return;
            }

            CelObjectGeneric? current = Player.CurrentObject;

            if (current is null)
            {
                Console.WriteLine("You're not located at any planet or star.");
                return;
            }

            Player.ResourceAmounts[ResourceType.Fuel] -= fuel;
            Player.CurrentObject = null;
            Console.WriteLine($"Now leaving {current.Name}...");
            await Task.Delay(Program.BaseSpeed * Program.LongTextMultiplier);
            gameMode = 2;
        }

        private static string GetPlanetTypeName(Planet planet)
        {
            string? type = null;
            int level = Player.Functions[FunctionType.CelestialScanner].Level;
            List<PlanetType> planetTypes;

            for (int i = 0; i < level; i++)
            {
                planetTypes = ((FunctionData<List<PlanetType>>)Player.FunctionCatalog[(FunctionType.CelestialScanner, i + 1)]).Attribute;
                type = planetTypes.Contains(planet.Type) ? Planet.PlanetCatalog[planet.Type].DisplayName : null;

                if (type is not null)
                    break;
            }

            type ??= "???";

            return type;
        }
    }
}