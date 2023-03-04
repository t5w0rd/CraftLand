using UnityEngine;

namespace TGamePlay.TAction
{
    public class FadeOut : FadeTo
    {
        public FadeOut(float duration, SpriteRenderer spriteRenderer) :
            base(duration, spriteRenderer, 0)
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
                end = 0;
            }

            start = TargetOpacity;
        }

        public override Action Clone()
        {
            return new FadeOut(Duration, spriteRenderer);
        }

        public override Action Reverse()
        {
            FadeIn action = new(Duration, spriteRenderer);
            action.reverseAction = this;
            return action;
        }

        protected internal FadeTo reverseAction;
    }
}
