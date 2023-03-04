using System;
using UnityEngine;

namespace TGamePlay.TAction
{
    public class Spawn : ActionInterval
    {
        protected readonly FiniteTimeAction first;
        protected readonly FiniteTimeAction second;

        public Spawn(params FiniteTimeAction[] actions) :
            base(0)
        {
            int count = actions.Length;
            if (count == 0)
            {
                Debug.Assert(false, "actions can't be empty");
                first = new ExtraAction();
                second = new ExtraAction();
            }
            else
            {
                first = actions[0];
                if (count == 1)
                {
                    second = new DelayTime(first.Duration);
                }
                else
                {
                    // else size > 1
                    for (int i = 1; i < count - 1; ++i)
                    {
                        first = new Spawn(first, actions[i]);
                    }
                    second = actions[count - 1];

                    float delta = first.Duration - second.Duration;
                    if (delta > 0)
                    {
                        second = new Sequence(second, new DelayTime(delta));
                    }
                    else if (delta < 0)
                    {
                        first = new Sequence(first, new DelayTime(-delta));
                    }
                }
            }
            Duration = Math.Max(first.Duration, second.Duration);
        }

        public override Action Clone()
        {
            return new Spawn(first.Clone() as FiniteTimeAction, second.Clone() as FiniteTimeAction);
        }

        public override Action Reverse()
        {
            return new Spawn(second.Reverse() as FiniteTimeAction, first.Reverse() as FiniteTimeAction);
        }

        public override void Start()
        {
            base.Start();
            first.Start();
            second.Start();
        }

        public override void Update(float time)
        {
            first.Update(time);
            second.Update(time);
        }
    }
}
