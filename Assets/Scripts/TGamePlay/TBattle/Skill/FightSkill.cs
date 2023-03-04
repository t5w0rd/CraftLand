namespace TGamePlay.TBattle
{
    public abstract class FightSkill : Skill, FightAttribute.IEventListener
    {
        private readonly FightAttribute.Event events;

        public FightSkill(int id, string name, FightAttribute.Event events) : base(id, name)
        {
            this.events = events;
        }

        public Unit Unit => (Container as SkillAttribute)?.Unit;
        public Unit RootUnit { get; internal set; }

        public FightAttribute Fight { get; private set; }

        public override void AddToContainer(ISkillContainer container)
        {
            Container = container;
            RootUnit = Unit;
            Fight = Unit.GetComponent<FightAttribute>();

            Fight?.RegisterEventListener(events, this);
            OnAdd();
        }

        public override void RemoveFromContainer()
        {
            OnRemove();
            Fight?.UnRegisterEventListener(events, this);

            Container = null;
            RootUnit = null;
            Fight = null;
        }

        public virtual void OnAttack(ref DamageInfo value)
        {
            throw new System.NotImplementedException();
        }

        public virtual void OnDamage(ref DamageInfo value)
        {
            throw new System.NotImplementedException();
        }

        public virtual void OnPreDamage(ref DamageInfo value)
        {
            throw new System.NotImplementedException();
        }

        public virtual void OnUpdate(float delta)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnAdd()
        {
            
        }

        protected override void OnRemove()
        {
        }
    }
}
