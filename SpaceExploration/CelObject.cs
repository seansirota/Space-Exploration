using System;

namespace SpaceExploration
{
    abstract class CelObjectGeneric
    {
        public abstract string? Name { get; set; }
        public abstract double Mass { get; set; }
        public abstract bool Visited { get; set; }
        public abstract bool EventActive { get; set; }
        public abstract int Cooldown { get; set; }
    }

    abstract class CelObject<T> : CelObjectGeneric
    {
        public override string? Name { get; set; }
        public abstract T Type { get; set; }
        public override double Mass { get; set; }
        public override bool Visited { get; set; }
        public override bool EventActive { get; set; }
        public override int Cooldown { get; set; }
    }
}