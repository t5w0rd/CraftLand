using System;
using System.Collections;
using UnityEngine;

namespace TGamePlay.TAction
{
    public class Repeat : ActionInterval
    {
        protected uint times;
        protected uint total;
        protected float nextDt;
        protected bool actionInstant;
        protected readonly FiniteTimeAction inner;

        public Repeat(FiniteTimeAction action, uint times) :
            base(action.Duration * times)
        {
            inner = action;
            this.times = times;
            actionInstant = inner is ActionInstant;
            total = 0;
        }

        public override Action Clone()
        {
            return new Repeat(inner.Clone() as FiniteTimeAction, inner as ActionInstant != null ? times + 1 : times);
        }

        public override Action Reverse()
        {
            return new Repeat(inner.Reverse() as FiniteTimeAction, inner as ActionInstant != null ? times + 1 : times);
        }

        public override bool Done => total == times;

        public override void Start()
        {
            total = 0;
            nextDt = inner.Duration / Duration;
            base.Start();
            inner.Start();
        }

        public override void Update(float time)
        {
            if (time >= nextDt)
            {
                while (time > nextDt && total < times)
                {
                    inner.Update(1.0f);
                    ++total;

                    //_innerAction.stop();
                    inner.Start();
                    nextDt = inner.Duration / Duration * (total + 1);
                }

                // fix for issue #1288, incorrect end value of repeat
                if (Math.Abs(time - 1.0f) < float.Epsilon && total < times)
                {
                    ++total;
                }

                // don't set an instant action back or update it, it has no use because it has no duration
                if (!actionInstant)
                {
                    if (total == times)
                    {
                        //_innerAction.stop();
                    }
                    else
                    {
                        // issue #390 prevent jerk, use right update
                        inner.Update(time - (nextDt - inner.Duration / Duration));
                    }
                }
            }
            else
            {
                inner.Update((time * times) % 1.0f);
            }
        }
    }
}
