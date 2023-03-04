using System.Collections.Generic;

namespace TGamePlay.TBattle
{
    public class BattleGround
    {
        private readonly List<BattleForce> forces;

        public BattleGround()
        {
            forces = new List<BattleForce>();
        }

        public void AddForce(BattleForce force)
        {
            forces.Add(force);
        }

        public void RemoveForce(BattleForce force)
        {
            forces.Remove(force);
        }

        public List<BattleForce> Forces => forces;
    }
}
