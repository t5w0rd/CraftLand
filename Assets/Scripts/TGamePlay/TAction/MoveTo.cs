using System;
using UnityEngine;

namespace TGamePlay.TAction
{
    public class MoveTo : MoveBy
    {
        protected Vector3 end;

        public MoveTo(float duration, Transform transform, Vector3 position) :
            base(duration, transform, Vector3.zero)
        {
            end = position;
        }

        public override Action Clone()
        {
            return new MoveTo(Duration, transform, end);
        }

        public override Action Reverse()
        {
            throw new NotSupportedException();
        }

        public override void Start()
        {
            base.Start();
            delta = end - start;
        }
    }

}
