using System;
using System.Collections;
using UnityEngine;

namespace TGamePlay.TAction
{
    public abstract class Action
    {
        public IEnumerator Play()
        {
            Start();
            while (!Done)
            {
                Step(Time.deltaTime);
                yield return null;
            }
            End();
        }

        public virtual bool Done => true;

        public virtual Action Clone()
        {
            throw new NotImplementedException();
        }

        public virtual Action Reverse()
        {
            throw new NotImplementedException();
        }

        public virtual void Start() { }

        public virtual void End() { }

        public virtual void Step(float dt)
        {
            throw new NotImplementedException();
        }

        public virtual void Update(float time)
        {
            throw new NotImplementedException();
        }
    }
}
