using System;
using System.Collections;
using UnityEngine;

namespace TGamePlay.TAction
{
    public class Sequence : ActionInterval
    {
        protected readonly FiniteTimeAction[] actions = new FiniteTimeAction[2];
        protected float split;
        protected int last;

        public Sequence(params FiniteTimeAction[] actions)
            : base(0)
        {
            int count = actions.Length;
            if (count == 0)
            {
                Debug.Assert(false, "actions can't be empty");
                this.actions[0] = new ExtraAction();
                this.actions[1] = new ExtraAction();
            }
            else
            {
                this.actions[0] = actions[0];
                if (count == 1)
                {
                    this.actions[1] = new ExtraAction();
                }
                else
                {
                    // else size > 1
                    for (int i = 1; i < count - 1; i++)
                    {
                        this.actions[0] = new Sequence(this.actions[0], actions[i]);
                    }
                    this.actions[1] = actions[count - 1];
                }
            }
            Duration = this.actions[0].Duration + this.actions[1].Duration;
        }

        public override void Start()
        {
            if (Duration > float.Epsilon)
            {
                split = actions[0].Duration > float.Epsilon ? (actions[0].Duration / Duration) : 0;
            }
            base.Start();
            last = -1;
        }

        public override void Update(float time)
        {
            int found;
            float newt;
            if (time < split)
            {
                // action[0]
                found = 0;
                if (split != 0)
                {
                    newt = time / split;
                }
                else
                {
                    newt = 1;
                }
            }
            else
            {
                // action[1]
                found = 1;
                if (split == 1)
                {
                    newt = 1;
                }
                else
                {
                    newt = (time - split) / (1 - split);
                }
            }

            if (found == 1)
            {

                if (last == -1)
                {
                    // action[0] was skipped, execute it.
                    actions[0].Start();
                    actions[0].Update(1.0f);
                    //_actions[0].stop();
                }
                else if (last == 0)
                {
                    // switching to action 1. stop action 0.
                    actions[0].Update(1.0f);
                    //_actions[0].stop();
                }
            }
            else if (found == 0 && last == 1)
            {
                // Reverse mode ?
                // FIXME: Bug. this case doesn't contemplate when _last==-1, found=0 and in "reverse mode"
                // since it will require a hack to know if an action is on reverse mode or not.
                // "step" should be overridden, and the "reverseMode" value propagated to inner Sequences.
                actions[1].Update(0);
                //_actions[1].stop();
            }
            // Last action found and it is done.
            if (found == last && actions[found].Done)
            {
                return;
            }

            // Last action found and it is done
            if (found != last)
            {
                actions[found].Start();
            }

            actions[found].Update(newt);
            last = found;
        }
    }
}
