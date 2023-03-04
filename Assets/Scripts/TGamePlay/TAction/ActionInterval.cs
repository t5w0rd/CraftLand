using System;

namespace TGamePlay.TAction
{
    public class ActionInterval : FiniteTimeAction
    {
        protected bool firstFrame;

        public ActionInterval(float duration)
        {
            Duration = duration == 0 ? float.Epsilon : duration;
            Elapsed = 0;
            firstFrame = true;
        }

        public override bool Done => Elapsed >= Duration;

        public float Elapsed { get; protected set; }

        public override void Start()
        {
            Elapsed = 0;
            firstFrame = true;
        }

        public override void Step(float dt)
        {
            if (firstFrame)
            {
                firstFrame = false;
                //elapsed = 0;
                Elapsed = dt;
            }
            else
            {
                Elapsed += dt;
            }
            float updateDt = Math.Max(0.0f, Math.Min(1.0f, Elapsed / Math.Max(Duration, float.Epsilon)));
            Update(updateDt);
        }
    }
}
