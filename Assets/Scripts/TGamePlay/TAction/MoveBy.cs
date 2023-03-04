using UnityEngine;

namespace TGamePlay.TAction
{
    public class MoveBy : ActionInterval
    {
        protected readonly Transform transform;
        protected Vector3 delta;
        protected Vector3 start;

        public MoveBy(float duration, Transform transform, Vector3 delta) :
            base(duration)
        {
            this.transform = transform;
            this.delta = delta;
        }

        public override Action Clone()
        {
            return new MoveBy(Duration, transform, delta);
        }

        public override Action Reverse()
        {
            return new MoveBy(Duration, transform, -delta);
        }

        public override void Start()
        {
            base.Start();
            start = transform.position;
        }

        public override void Update(float time)
        {
            transform.position = start + delta * time;
        }
    }
}
