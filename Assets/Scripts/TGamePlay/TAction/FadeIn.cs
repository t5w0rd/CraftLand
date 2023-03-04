using UnityEngine;

namespace TGamePlay.TAction
{
    public class FadeIn : FadeTo
    {
        protected internal FadeTo reverseAction;

        public FadeIn(float duration, SpriteRenderer spriteRenderer) :
            base(duration, spriteRenderer, 1.0f)
        { }

        public override void Start()
        {
            base.Start();

            if (reverseAction != null)
            {
                end = reverseAction.start;
            }
            else
            {
                end = 1.0f;
            }

            start = TargetOpacity;
        }

        public override Action Clone()
        {
            return new FadeIn(Duration, spriteRenderer);
        }

        public override Action Reverse()
        {
            FadeOut action = new(Duration, spriteRenderer);
            action.reverseAction = this;
            return action;
        }
    }
}
