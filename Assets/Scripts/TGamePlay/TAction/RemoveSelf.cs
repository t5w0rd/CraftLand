using UnityEngine;

namespace TGamePlay.TAction
{
    public class RemoveSelf : ActionInstant
    {
        protected readonly GameObject gameObject;

        public RemoveSelf(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public override void Update(float time)
        {
            Object.Destroy(gameObject);
        }

        public override Action Clone()
        {
            return new RemoveSelf(gameObject);
        }

        public override Action Reverse()
        {
            return new RemoveSelf(gameObject);
        }
    }
}
