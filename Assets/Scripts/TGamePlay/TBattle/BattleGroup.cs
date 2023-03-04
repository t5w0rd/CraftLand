using System.Collections.Generic;

namespace TGamePlay.TBattle
{
    public class BattleGroup
    {
        private readonly List<Unit> units;

        public BattleGroup()
        {
            units = new List<Unit>();
        }

        public void AddUnit(Unit u)
        {
            units.Add(u);
        }

        public void RemoveUnit(Unit u)
        {
            units.Remove(u);
        }

        public List<Unit> Units => units;
    }
}
