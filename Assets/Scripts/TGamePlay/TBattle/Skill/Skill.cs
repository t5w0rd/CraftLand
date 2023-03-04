namespace TGamePlay.TBattle
{
    /// <summary>
    /// Skill一定跟Unit有关。容器是SkillAttribute
    /// </summary>
    public abstract class Skill : Object
    {
        protected Skill(int id, string name) : base(id, name)
        {
        }

        public ISkillContainer Container { get; protected set; }

        public virtual void AddToContainer(ISkillContainer container)
        {
            Container = container;
            OnAdd();
        }

        public virtual void RemoveFromContainer()
        {
            OnRemove();
            Container = null;
        }

        protected virtual void OnAdd()
        {
        }

        protected virtual void OnRemove()
        {
        }

        #region Interval
        public float Interval { get; set; }

        public float IntervalElapsed { get; set; }

        public virtual void OnInterval()
        {
        }
        #endregion

        #region CoolDown
        public void UpdateCoolDown(float delta)
        {
            if (coolDown.Value < coolDown.MaxValue)
            {
                coolDown.Value += delta;
            }
        }

        protected CoefFloatMaxValue coolDown;

        public bool IsReady => coolDown.Value >= coolDown.MaxValue;

        public void ResetCD()
        {
            coolDown.Value = coolDown.MaxValue;
        }

        public void CoolDown(float from)
        {
            coolDown.Value = coolDown.MaxValue * from;
        }
        #endregion
    }
}
