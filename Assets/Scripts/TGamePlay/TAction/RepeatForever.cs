namespace TGamePlay.TAction
{
    public class RepeatForever : ActionInterval
    {
        protected readonly ActionInterval inner;

        public RepeatForever(ActionInterval action) :
            base(0)
        {
            inner = action;
        }

        public override Action Clone()
        {
            return new RepeatForever(inner.Clone() as ActionInterval);
        }

        public override Action Reverse()
        {
            return new RepeatForever(inner.Reverse() as ActionInterval);
        }

        public override bool Done => false;

        public override void Start()
        {
            base.Start();
            inner.Start();
        }

        public override void Step(float dt)
        {
            inner.Step(dt);
            if (inner.Done)
            {
                float diff = inner.Elapsed - inner.Duration;
                if (diff > inner.Duration)
                    diff %= inner.Duration;
                inner.Start();
                inner.Step(0.0f);
                inner.Step(diff);
            }
        }
    }
}
