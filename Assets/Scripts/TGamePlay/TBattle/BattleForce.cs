using System.Collections.Generic;

namespace TGamePlay.TBattle
{
    public class BattleForce
    {
        private readonly List<BattleGroup> groups;
        private int allyMask;

        public BattleForce(byte force)
        {
            groups = new List<BattleGroup>();
            Force = force;
            allyMask = 1 << force;
        }

        public byte Force { get; }

        public void AddGroup(BattleGroup group)
        {
            groups.Add(group);
        }

        public void RemoveGroup(BattleGroup group)
        {
            groups.Remove(group);
        }

        public void AllyWith(BattleForce force)
        {
            allyMask |= 1 << force.Force;
            force.allyMask |= 1 << Force;
        }

        public void SeparateFrom(BattleForce force)
        {
            allyMask ^= 1 << force.Force;
            force.allyMask ^= 1 << Force;
        }
    }
}
