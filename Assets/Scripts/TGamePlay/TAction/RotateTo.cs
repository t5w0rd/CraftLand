using System;
using UnityEngine;

namespace TGamePlay.TAction
{
    public class RotateTo : RotateBy
    {
        protected float end;

        public RotateTo(float duration, Transform transform, float dstAngle) :
            base(duration, transform, 0)
        {
            end = dstAngle;
        }

        public override Action Clone()
        {
            return new RotateTo(Duration, transform, end);
        }

        public override Action Reverse()
        {
            throw new NotSupportedException();
        }

        public override void Start()
        {
            base.Start();
            delta = end - TargetRotation;
        }
    }
}
