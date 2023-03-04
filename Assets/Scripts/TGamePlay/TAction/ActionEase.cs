using System;

namespace TGamePlay.TAction
{
    public class ActionEase : ActionInterval
    {
        protected readonly ActionInterval inner;

        public ActionEase(ActionInterval action) :
            base(action.Duration)
        {
            inner = action;
        }

        public override Action Clone()
        {
            throw new NotImplementedException();
        }

        public override Action Reverse()
        {
            throw new NotImplementedException();
        }

        public override void Start()
        {
            base.Start();
            inner.Start();
        }

        public override void Update(float time)
        {
            inner.Update(time);
        }
    }
}
