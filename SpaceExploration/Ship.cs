using System;
using Spectre.Console;

namespace SpaceExploration
{
    class Ship
    {
        public static async Task ConvertResources()
        {
            Console.WriteLine("Showing all ship resources...");
            await Task.Delay(2000);

            Table resourceTable = new Table().Border(TableBorder.Rounded).ShowHeaders();
            resourceTable.AddColumn(new TableColumn("Option").NoWrap());
            resourceTable.AddColumn(new TableColumn("Resource").NoWrap());
            resourceTable.AddColumn(new TableColumn("Owned").NoWrap());
            resourceTable.AddColumn(new TableColumn("Maximum").NoWrap());
            resourceTable.AddColumn(new TableColumn("Element").NoWrap());
            resourceTable.AddColumn(new TableColumn("Amount").NoWrap());
        }

        public static async Task UpgradeFunctions()
        {
            
        }

        public static async Task ConfigureAutomations()
        {
            
        }
    }
}