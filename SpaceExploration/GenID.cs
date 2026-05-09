using System;

namespace SpaceExploration
{
    class GenID
    {
        private static int runningID = 1;

        public static int CreateGenID()
        {
            return runningID++;
        }
    }
}