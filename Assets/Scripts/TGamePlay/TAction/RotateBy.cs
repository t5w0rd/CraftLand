using UnityEngine;

namespace TGamePlay.TAction
{
    public class RotateBy : ActionInterval
    {
        protected readonly Transform transform;
        protected float delta;
        protected float start;

        public RotateBy(float duration, Transform transform, float deltaAngle)
            : base(duration)
        {
            this.transform = transform;
            delta = deltaAngle;
        }

        protected float TargetRotation
        {
            get => transform.localRotation.eulerAngles.z;
            set
            {
                Vector3 rotation = transform.localRotation.eulerAngles;
                rotation.z = value;
                transform.localRotation = Quaternion.Euler(rotation);
            }
        }

        public override Action Clone()
        {
            return new RotateBy(Duration, transform, delta);
        }

        public override Action Reverse()
        {
            return new RotateBy(Duration, transform, -delta);
        }

        public override void Start()
        {
            base.Start();
            start = TargetRotation;
        }

        public override void Update(float time)
        {
            TargetRotation = start + delta * time;
        }
    }
}
