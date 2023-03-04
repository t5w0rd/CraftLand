using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TGamePlay.TBattle
{
    public class Aura : FightSkill
    {
        protected Coefficient coef;

        public Aura(int id, string name) : base(id, name, FightAttribute.Event.OnUpdate)
        {
        }        
    }
}
