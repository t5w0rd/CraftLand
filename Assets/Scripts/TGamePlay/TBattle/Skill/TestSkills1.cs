using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TGamePlay.TBattle
{
    public class TestOnDamageSkill : FightSkill
    {
        protected Coefficient coef;

        public TestOnDamageSkill(int id, string name, Coefficient coef) : base(id, name, FightAttribute.Event.OnDamage)
        {
            this.coef = coef;
        }

        public override void OnDamage(ref DamageInfo value)
        {
            value.Value = coef.Value(value.Value);
        }
    }

    public class TestOnAttackSkill : FightSkill
    {
        protected Coefficient coef;
        protected float chance;

        public TestOnAttackSkill(int id, string name, Coefficient coef, float chance) : base(id, name, FightAttribute.Event.OnAttack)
        {
            this.coef = coef;
            this.chance = chance;
        }

        public override void OnAttack(ref DamageInfo value)
        {
            if (Random.Range(0f, 1f) < chance)
            {
                value.Value = coef.Value(value.Value);
            }
        }
    }
}
