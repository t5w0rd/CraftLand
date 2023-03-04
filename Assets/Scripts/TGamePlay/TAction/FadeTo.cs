using System;
using UnityEngine;

namespace TGamePlay.TAction
{
    public class FadeTo : ActionInterval
    {
        protected readonly SpriteRenderer spriteRenderer;
        protected float end;
        protected internal float start;

        public FadeTo(float duration, SpriteRenderer spriteRenderer, float opacity) :
            base(duration)
        {
            this.spriteRenderer = spriteRenderer;
            end = opacity;
        }

        protected float TargetOpacity
        {
            get => spriteRenderer.color.a;
            set
            {
                Color color = spriteRenderer.color;
                color.a = value;
                spriteRenderer.color = color;
            }
        }

        public override Action Clone()
        {
            return new FadeTo(Duration, spriteRenderer, end);
        }

        public override Action Reverse()
        {
            throw new NotSupportedException();
        }

        public override void Start()
        {
            base.Start();
            start = TargetOpacity;
        }

        public override void Update(float time)
        {
            TargetOpacity = start + (end - start) * time;
        }
    }
}
