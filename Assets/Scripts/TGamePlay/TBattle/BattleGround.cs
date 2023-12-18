using System.Collections.Generic;

namespace TGamePlay.TBattle
{
    public class BattleGround
    {
        private readonly List<BattleForce> forces = new();

        public BattleGround()
        {
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
