using System;
using System.Threading.Tasks;
using Spectre.Console;

namespace SpaceExploration
{
    class GameAction
    {
        public static bool gameComplete;
        public static int gameMode = 1;

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
            }
        }

        public static async Task MainMenu()
        {
            bool invalidResponse;
            string? currentStarSystem = (Player.currentSystem is not null) ? StarSystem.Systems[Player.currentSystem ?? 0].Name.ToString() : "None";

            do
            {
                invalidResponse = false;
                Console.WriteLine($"""
                Current star system: {currentStarSystem}
                Current fuel level: {Math.Round(Player.Fuel, 2)} / {Player.FuelCap.FunctionAttributes[Player.FuelCap.Level]}
                """);
                Console.WriteLine("""
                Choose an action:
                1/J: Jump to new star system.
                2/V: Visit current star system.
                3/I: Check inventory.
                4/F: Check functions.
                5/N: Leave a note on the current star system.
                6/B: View logbook.
                """);

                string? playerEntry = Console.ReadLine();

                if (playerEntry == "1" || playerEntry?.Equals("J", StringComparison.OrdinalIgnoreCase) == true)
                    await JumpSystem();
                else if (playerEntry == "2" || playerEntry?.Equals("V", StringComparison.OrdinalIgnoreCase) == true)
                {
                    if (Player.currentSystem is null)
                    {
                        Console.WriteLine("No current star system selected.");
                        await Task.Delay(2000);
                        invalidResponse = true;
                    }
                    else
                    {
                        gameMode = 2;
                        Console.WriteLine("Flying into star system...");
                        await Task.Delay(2000);
                        await SystemMenu();
                    }
                }
                else if (playerEntry == "3" || playerEntry?.Equals("I", StringComparison.OrdinalIgnoreCase) == true)
                    CheckInventory();
                else if (playerEntry == "4" || playerEntry?.Equals("F", StringComparison.OrdinalIgnoreCase) == true)
                    CheckFunctions();
                else if (playerEntry == "5" || playerEntry?.Equals("N", StringComparison.OrdinalIgnoreCase) == true)
                    invalidResponse = await AddNote();
                else if (playerEntry == "6" || playerEntry?.Equals("B", StringComparison.OrdinalIgnoreCase) == true)
                    ViewLogbook();
                else
                {
                    Console.WriteLine("Invalid command. Try again.");
                    await Task.Delay(2000);
                    invalidResponse = true;
                }
            } while (invalidResponse);
        }

        public static async Task SystemMenu()
        {
            bool invalidResponse;
            string? currentStarSystem = (Player.currentSystem is not null) ? StarSystem.Systems[Player.currentSystem ?? 0].Name.ToString() : "None";

            do
            {
                invalidResponse = false;
                Console.WriteLine($"""
                Current star system: {currentStarSystem}
                Current fuel level: {Math.Round(Player.Fuel, 2)} / {Player.FuelCap.FunctionAttributes[Player.FuelCap.Level]}
                """);
                Console.WriteLine("""
                Choose an action:
                1/L: Look back out to star systems.
                2/V: View stars and planets in current star system.
                3/I: Check inventory.
                4/F: Check functions.
                5/N: Leave a note on the current star system.
                6/B: View logbook.
                """);

                string? playerEntry = Console.ReadLine();

                if (playerEntry == "1" || playerEntry?.Equals("L", StringComparison.OrdinalIgnoreCase) == true)
                {
                    gameMode = 1;
                    Console.WriteLine("Returning to galaxy view...");
                    await Task.Delay(2000);
                }
                else if (playerEntry == "2" || playerEntry?.Equals("V", StringComparison.OrdinalIgnoreCase) == true)
                    await VisitStarPlanet();
                else if (playerEntry == "3" || playerEntry?.Equals("I", StringComparison.OrdinalIgnoreCase) == true)
                    CheckInventory();
                else if (playerEntry == "4" || playerEntry?.Equals("F", StringComparison.OrdinalIgnoreCase) == true)
                    CheckFunctions();
                else if (playerEntry == "5" || playerEntry?.Equals("N", StringComparison.OrdinalIgnoreCase) == true)
                    invalidResponse = await AddNote();
                else if (playerEntry == "6" || playerEntry?.Equals("B", StringComparison.OrdinalIgnoreCase) == true)
                    ViewLogbook();
                else
                {
                    Console.WriteLine("Invalid command. Try again.");
                    await Task.Delay(2000);
                    invalidResponse = true;
                }
            } while (invalidResponse);
        }

        public static async Task VisitMenu()
        {
            bool invalidResponse;
            string? currentStarSystem = (Player.currentSystem is not null) ? StarSystem.Systems[Player.currentSystem ?? 0].Name.ToString() : "None";
            string? currentObject = (Player.currentObject is not null) ? Player.currentObject.Name : "None";
            double fuel = Player.currentObject switch
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
                Current fuel level: {Math.Round(Player.Fuel, 2)} / {Player.FuelCap.FunctionAttributes[Player.FuelCap.Level]}
                """);
                Console.WriteLine($"""
                Choose an action:
                1/R: Return to star system. ({Math.Round(fuel, 2)} fuel units required)
                2/E: Extract resources.
                3/I: Check inventory.
                4/F: Check functions.
                5/N: Leave a note on the current star system.
                6/B: View logbook.
                """);

                string? playerEntry = Console.ReadLine();

                if (playerEntry == "1" || playerEntry?.Equals("R", StringComparison.OrdinalIgnoreCase) == true)
                    await ReturnToStarSystem();
                else if (playerEntry == "2" || playerEntry?.Equals("E", StringComparison.OrdinalIgnoreCase) == true)
                    ExtractResources();
                else if (playerEntry == "3" || playerEntry?.Equals("I", StringComparison.OrdinalIgnoreCase) == true)
                    CheckInventory();
                else if (playerEntry == "4" || playerEntry?.Equals("F", StringComparison.OrdinalIgnoreCase) == true)
                    CheckFunctions();
                else if (playerEntry == "5" || playerEntry?.Equals("N", StringComparison.OrdinalIgnoreCase) == true)
                    invalidResponse = await AddNote();
                else if (playerEntry == "6" || playerEntry?.Equals("B", StringComparison.OrdinalIgnoreCase) == true)
                    ViewLogbook();
                else
                {
                    Console.WriteLine("Invalid command. Try again.");
                    await Task.Delay(2000);
                    invalidResponse = true;
                }
            } while (invalidResponse);
        }

        public static async Task JumpSystem()
        {
            string? currentStarSystem = (Player.currentSystem is not null) ? StarSystem.Systems[Player.currentSystem ?? 0].Name.ToString() : "None";

            Console.WriteLine($"""
            Loading nearby star systems...
            Current star system: {currentStarSystem}
            Current fuel level: {Math.Round(Player.Fuel, 2)} / {Player.FuelCap.FunctionAttributes[Player.FuelCap.Level]}
            """);
            await Task.Delay(2000);
            
            List<int> nearbySystems = StarSystem.GetNearbySystems();

            Table starSystemTable = new Table().Border(TableBorder.Rounded).ShowHeaders();
            starSystemTable.AddColumn(new TableColumn("Option").NoWrap());
            starSystemTable.AddColumn(new TableColumn("Name").NoWrap());
            starSystemTable.AddColumn(new TableColumn("Fuel Cost").NoWrap());
            starSystemTable.AddColumn(new TableColumn("Direction").NoWrap());
            starSystemTable.AddColumn(new TableColumn("Comments").NoWrap());

            int count = 1;
            string option;
            string name;
            double fuel;
            string direction;
            string? note;

            foreach (int systemID in nearbySystems)
            {
                option = count++.ToString();
                name = StarSystem.Systems[systemID].Name;
                fuel = Math.Round(StarSystem.GetDistance(StarSystem.Systems[systemID]), 2);
                direction = StarSystem.GetDirection(StarSystem.Systems[systemID]);
                note = StarSystem.Systems[systemID].Note;
                note ??= "";

                starSystemTable.AddRow(option, name, $"{fuel} units", direction, note);
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

                destinationSystem = StarSystem.Systems[nearbySystems[result - 1]];

                if (StarSystem.GetDistance(destinationSystem) > Player.Fuel)
                {
                    Console.WriteLine("Not enough fuel to jump to that star system. Choose another option.");
                    await Task.Delay(2000);
                    invalidResponse = true;
                    continue;
                }

                Console.WriteLine($"Jumping to the {destinationSystem.Name} star system...");
                await Task.Delay(2000);

            } while (invalidResponse);

            if (destinationSystem is null)
                return;

            Player.Fuel -= StarSystem.GetDistance(StarSystem.Systems[destinationSystem.ID]);
            Player.currentSystem = destinationSystem.ID;
            Player.X = destinationSystem.X;
            Player.Y = destinationSystem.Y;
            destinationSystem.Visited = true;

            StarSystem.GenerateNewSystems();
        }

        public static async Task VisitStarPlanet()
        {
            if (Player.currentSystem == null)
            {
                Console.WriteLine("You're not located within a star system.");
                await Task.Delay(2000);
                return;
            }

            Console.WriteLine($"""
            Loading stars and planets within {StarSystem.Systems[Player.currentSystem ?? 0].Name}...
            Current fuel level: {Math.Round(Player.Fuel, 2)} / {Player.FuelCap.FunctionAttributes[Player.FuelCap.Level]}
            """);
            await Task.Delay(2000);

            if (Player.currentSystem is not int currentSystem)
                return;

            List<Star> stars = StarSystem.Systems[currentSystem].Stars;
            List<Planet> planets = StarSystem.Systems[currentSystem].Planets;

            Table bodiesTable = new Table();
            bodiesTable.AddColumn(new TableColumn("Option").NoWrap());
            bodiesTable.AddColumn(new TableColumn("Object").NoWrap());
            bodiesTable.AddColumn(new TableColumn("Name").NoWrap());
            bodiesTable.AddColumn(new TableColumn("Fuel Cost").NoWrap());
            bodiesTable.AddColumn(new TableColumn("Type").NoWrap());
            bodiesTable.AddColumn(new TableColumn("Mass").NoWrap());
            bodiesTable.AddColumn(new TableColumn("Temperature").NoWrap());

            int count = 1;
            string option;
            int starBorder;
            string? name;
            int fuel;
            string? type;
            double mass;
            int temperature;
            List<CelObjectGeneric> celObjects = new List<CelObjectGeneric>();

            foreach (Star star in stars)
            {
                option = count++.ToString();
                name = star.Name;
                fuel = Star.StarCatalog[star.Type].FuelCost;
                type = Star.StarCatalog[star.Type].DisplayName;
                mass = Math.Round(star.Mass, 2);
                temperature = star.Temperature;

                bodiesTable.AddRow(option, "Star", name!, $"{fuel} units", type!, $"{mass} SM", $"{temperature} K");
                celObjects.Add(star);
            }

            starBorder = count - 1;

            foreach (Planet planet in planets)
            {
                option = count++.ToString();
                name = planet.Name;
                fuel = Planet.PlanetCatalog[planet.Type].FuelCost;
                type = null;
                mass = Math.Round(planet.Mass, 2);

                type = GetPlanetTypeName(planet);
                bodiesTable.AddRow(option, "Planet", name!, $"{fuel} units", type, $"{mass} PM", string.Empty);
                celObjects.Add(planet);
            }

            string? playerEntry;
            bool invalidResponse;
            CelObjectGeneric destinationBody;
            string? destinationName;
            string? destinationType = null;

            do
            {
                AnsiConsole.Write(bodiesTable);
                Console.WriteLine("Enter a number to choose a star or planet to visit. Enter C to cancel.");
                playerEntry = Console.ReadLine();

                invalidResponse = false;

                if (playerEntry?.Equals("C", StringComparison.OrdinalIgnoreCase) == true)
                {
                    Console.WriteLine("Canceling. Returning to star and planet selection menu...");
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

                destinationBody = celObjects[result - 1];

                if (
                    result <= starBorder && Star.StarCatalog[((Star)destinationBody).Type].FuelCost > Player.Fuel ||
                    result > starBorder && Planet.PlanetCatalog[((Planet)destinationBody).Type].FuelCost > Player.Fuel
                )
                {
                    Console.WriteLine("Not enough fuel to visit that star or planet. Choose another option.");
                    await Task.Delay(2000);
                    invalidResponse = true;
                    continue;
                }

                if (destinationBody is Planet)
                {
                    destinationType = GetPlanetTypeName((Planet)destinationBody);

                    if (destinationType == "???")
                    {
                        Console.WriteLine("Current celestial sensors aren't advanced enough to set reliable course to this planet.");
                        await Task.Delay(2000);
                        invalidResponse = true;
                        continue;
                    }
                }

                destinationName = destinationBody.Name;

                Console.WriteLine($"Flying to {destinationName}...");
                await Task.Delay(2000);

                if (result <= starBorder)
                    Player.Fuel -= Star.StarCatalog[((Star)destinationBody).Type].FuelCost;
                else
                    Player.Fuel -= Planet.PlanetCatalog[((Planet)destinationBody).Type].FuelCost;

                Player.currentObject = destinationBody;
                destinationBody.Visited = true;
                gameMode = 3;
            } while (invalidResponse);
        }

        public static void CheckInventory()
        {
            
        }

        public static void CheckFunctions()
        {
            
        }

        public static async Task<bool> AddNote()
        {
            if (Player.currentSystem is not int currentSystem)
            {
                Console.WriteLine("Not a valid star system. Returning to main selection menu...");
                await Task.Delay(2000);
                return true;
            }
            Console.WriteLine("Enter a note for this star system below.");

            StarSystem.Systems[currentSystem].Note = Console.ReadLine();
            Console.WriteLine("Note saved. Returning to main selection menu...");
            await Task.Delay(2000);
            return false;
        }

        public static void ViewLogbook()
        {
            
        }

        public static void ExtractResources()
        {
            
        }

        public static async Task ReturnToStarSystem()
        {
            double fuel = Player.currentObject switch
            {
                Star star => Star.StarCatalog[star.Type].FuelCost,
                Planet planet => Planet.PlanetCatalog[planet.Type].FuelCost,
                _ => 0
            } * 2;

            if (fuel > Player.Fuel)
            {
                Console.WriteLine("You don't have enough fuel to leave the planet or star.");
                await Task.Delay(2000);
                return;
            }

            CelObjectGeneric? current = Player.currentObject;

            if (current is null)
            {
                Console.WriteLine("You're not located at any planet or star.");
                return;
            }

            Player.Fuel -= fuel;
            Player.currentObject = null;
            Console.WriteLine($"Now leaving {current.Name}...");
            await Task.Delay(2000);
            gameMode = 2;
        }

        private static string GetPlanetTypeName(Planet planet)
        {
            string? type = null;

            for (int i = 0; i < Player.CelScan.Level; i++)
            {
                type = Player.CelScan.FunctionAttributes[i + 1].Contains(planet.Type) ? Planet.PlanetCatalog[planet.Type].DisplayName : null;

                if (type is not null)
                    break;
            }

            type ??= "???";

            return type;
        }

        private static async Task TransactElements(Dictionary<Element.ElementType, int> elements, bool verbose)
        {
            int have;
            int delta;
            int cargo = Player.ElementAmounts.Values.Sum();
            int cap = Player.CargoCap.FunctionAttributes[Player.CargoCap.Level];
            string? name;
            bool invalidResponse;

            if (cargo + elements.Values.Sum() > cap)
            {
                Console.WriteLine($"You don't have enough cargo capacity for this transaction.");
                await Task.Delay(2000);
                return;
            }
            
            foreach (KeyValuePair<Element.ElementType, int> element in elements)
            {
                have = Player.ElementAmounts[element.Key];
                delta = element.Value;
                name = Element.ElementCatalog[element.Key].DisplayName;

                if (delta < 0)
                {
                    if (verbose)
                            Console.WriteLine($"You have {have} units of {name} and need {delta} units.");

                    if (have < -delta)
                    {
                        Console.WriteLine($"You don't have enough {name}.");
                        await Task.Delay(2000);
                        return;
                    }
                }
                else
                {
                    if (verbose)
                            Console.WriteLine($"You have {have} units of {name} and will get {delta} units.");

                    if (cargo + delta > cap)
                    {
                        do
                        {
                            invalidResponse = false;

                            Console.WriteLine($"Your current cargo capacity of {cargo} won't have room for {delta - cap - cargo} units of {name}, the rest will be discarded.");
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
                                return;
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

            foreach (KeyValuePair<Element.ElementType, int> element in elements)
            {
                have = Player.ElementAmounts[element.Key];
                delta = element.Value;
                name = Element.ElementCatalog[element.Key].DisplayName;

                if (verbose)
                {
                    Console.WriteLine($"Change of {delta} units of {name}...");
                    await Task.Delay(1000);
                }

                Player.ElementAmounts[element.Key] += delta;
            }

            if (verbose)
                Console.WriteLine("Transaction complete.");
        }
    }
}