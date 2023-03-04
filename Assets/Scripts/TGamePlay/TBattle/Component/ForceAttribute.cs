using System;

namespace TGamePlay.TBattle
{
    [Flags]
    public enum ForceFlags
    {
        Force1 = 1,
        Force2 = 1 << 1,
        Force3 = 1 << 2,
        Force4 = 1 << 3,
        Force5 = 1 << 4,
        Force6 = 1 << 5,
        Force7 = 1 << 6,
        Force8 = 1 << 7
    }

    public class ForceAttribute : Component
    {
        private ForceFlags allyMask;

        public ForceAttribute(ForceFlags force)
        {
            Force = force;
            allyMask = Force;
        }

        public ForceAttribute(int force)
        {
            Force = ForceIndexToFlags(force);
            allyMask = Force;
        }

        private ForceFlags ForceIndexToFlags(int force)
        {
            switch (force)
            {
                case 1:
                    return ForceFlags.Force1;
                case 2:
                    return ForceFlags.Force2;
                case 3:
                    return ForceFlags.Force3;
                case 4:
                    return ForceFlags.Force4;
                case 5:
                    return ForceFlags.Force5;
                case 6:
                    return ForceFlags.Force6;
                case 7:
                    return ForceFlags.Force7;
                case 8:
                    return ForceFlags.Force8;
                default:
                    return 0;
            }
        }

        private int ForceFlagsToIndex(ForceFlags force)
        {
            switch (force)
            {
                case ForceFlags.Force1:
                    return 1;
                case ForceFlags.Force2:
                    return 2;
                case ForceFlags.Force3:
                    return 3;
                case ForceFlags.Force4:
                    return 4;
                case ForceFlags.Force5:
                    return 5;
                case ForceFlags.Force6:
                    return 6;
                case ForceFlags.Force7:
                    return 7;
                case ForceFlags.Force8:
                    return 8;
                default:
                    return 0;
            }
        }

        public int ForceIndex { get => ForceFlagsToIndex(Force); }

        public ForceFlags Force { get; }

        public void AllyWith(ForceAttribute force)
        {
            allyMask |= force.Force;
            force.allyMask |= Force;
        }

        public void SeparateFrom(ForceAttribute force)
        {
            allyMask ^= force.Force;
            force.allyMask ^= Force;
        }

        public bool IsMyAlly(ForceAttribute force) => (force.Force == Force) || (allyMask & force.Force) != 0;

        public bool IsMyEnemy(ForceAttribute force) => !IsMyAlly(force);
    }
}
