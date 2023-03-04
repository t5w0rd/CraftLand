using UnityEngine;

namespace TGamePlay.TAction
{
    public class ScaleBy : ScaleTo
    {
        public ScaleBy(float duration, Transform transform, float sx, float sy, float sz) :
            base(duration, transform, sx, sy, sz)
        { }

        public ScaleBy(float duration, Transform transform, float s) :
            this(duration, transform, s, s, s)
        { }

        public ScaleBy(float duration, Transform transform, float sx, float sy) :
            this(duration, transform, sx, sy, 1.0f)
        { }

        public override void Start()
        {
            base.Start();
            delta.x = start.x * end.x - start.x;
            delta.y = start.y * end.y - start.y;
            delta.z = start.z * end.z - start.z;
        }

        public override Action Clone()
        {
            return new ScaleBy(Duration, transform, end.x, end.y, end.z);
        }

        public override Action Reverse()
        {
            return new ScaleBy(Duration, transform, 1 / end.x, 1 / end.y, 1 / end.z);
        }
    }
}
