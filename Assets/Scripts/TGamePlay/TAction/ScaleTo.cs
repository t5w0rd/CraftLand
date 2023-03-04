using System;
using System.Collections;
using UnityEngine;

namespace TGamePlay.TAction
{
    public class ScaleTo : ActionInterval
    {
        protected readonly Transform transform;
        protected Vector3 start;
        protected Vector3 end;
        protected Vector3 delta;

        public ScaleTo(float duration, Transform transform, float sx, float sy, float sz) :
            base(duration)
        {
            this.transform = transform;
            end.x = sx;
            end.y = sy;
            end.z = sz;
        }

        public ScaleTo(float duration, Transform transform, float s) :
            this(duration, transform, s, s, s)
        { }

        public ScaleTo(float duration, Transform transform, float sx, float sy) :
            this(duration, transform, sx, sy, 1.0f)
        { }

        protected Vector2 TargetScale
        {
            get => transform.localScale;
            set
            {
                Vector3 scale = transform.localScale;
                scale.x = value.x;
                scale.y = value.y;
                transform.localScale = scale;
            }
        }

        public override Action Clone()
        {
            return new ScaleTo(Duration, transform, end.x, end.y, end.z);
        }

        public override Action Reverse()
        {
            throw new NotSupportedException();
        }

        public override void Start()
        {
            base.Start();
            start = TargetScale;
            delta = end - start;
        }

        public override void Update(float time)
        {
            TargetScale = start + delta * time;
        }
    }
}
