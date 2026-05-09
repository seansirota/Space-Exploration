using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SpaceExploration
{
    class Program
    {
        public static readonly Random Rand = new Random();

        static async Task Main(string[] args)
        {
            await GameAction.InitiateWorld();
        }
    }
}