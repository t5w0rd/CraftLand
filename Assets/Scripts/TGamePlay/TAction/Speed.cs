namespace TGamePlay.TAction
{
    public class Speed : Action
    {
        protected readonly ActionInterval inner;

        public Speed(ActionInterval action, float speed)
        {
            inner = action;
            ActionSpeed = speed;
        }

        public float ActionSpeed { get; set; }

        public override Action Clone()
        {
            return new Speed(inner.Clone() as ActionInterval, ActionSpeed);
        }

        public override Action Reverse()
        {
            return new Speed(inner.Reverse() as ActionInterval, ActionSpeed);
        }

        public override bool Done => inner.Done;

        public override void Start()
        {
            base.Start();
            inner.Start();
        }

        public override void Step(float dt)
        {
            inner.Step(dt * ActionSpeed);
        }
    }
}
