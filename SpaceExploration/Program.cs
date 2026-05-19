using System;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SpaceExploration
{
    class Program
    {
        public static readonly Random Rand = new Random();
        public static bool Verbose = true;
        public static bool TextShorten = false;
        public const int BaseSpeed = 250;
        public static int LongTextMultiplier = 8;
        public static int ShortTextMultiplier = 2;
        public static int SortPattern = 1;
        public static string SortText = "alphabetical";
        public static Dictionary<int, string> SortMethods = new Dictionary<int, string>()
        {
            { 1, "Historical" },
            { 2, "Alphabetical" },
            { 3, "Spatial" }
        };
        public static bool IncreasedRowLimit = false;
        public static int PageRowLimit = 10;
        public static Dictionary<int, Tuple<string, object>> Automations { get; set; } = new Dictionary<int, Tuple<string, object>>()
        {
            { 1, new("Enable verbose output of transactions.", Verbose) },
            { 2, new("Enable increased speed of loading texts.", TextShorten) },
            { 3, new("Switch between logbook sorting modes.", SortPattern) },
            { 4, new("Enable more records displayed at once in logbook.", IncreasedRowLimit) }
        };

        static async Task Main(string[] args)
        {
            await GameAction.InitiateWorld();
        }
    }
}