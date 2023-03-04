using UnityEngine;

namespace TGamePlay.TAction
{
    public class Animation
    {
        protected internal object[] framesData;

        public Animation(Sprite[] frames, float delayPerUnit, uint loops = 1)
        {
            Frames = frames.Clone() as Sprite[];
            DelayPerUnit = delayPerUnit;
            Loops = loops;
            RestoreOriginalFrame = false;
            framesData = new object[Frames.Length];
        }

        public void SetFrameData(int index, object data)
        {
            Debug.Assert(index >= 0 && index < framesData.Length);
            framesData[index] = data;
        }

        public object GetFrameData(int index)
        {
            Debug.Assert(index >= 0 && index < framesData.Length);
            return framesData[index];
        }

        public Sprite[] Frames { get; protected set; }

        public float DelayPerUnit { get; set; }

        public float Duration => Frames.Length * DelayPerUnit;

        public bool RestoreOriginalFrame { get; set; }

        public uint Loops { get; set; }

        public virtual Animation Clone()
        {
            Animation a = new(Frames, DelayPerUnit, Loops);
            a.RestoreOriginalFrame = RestoreOriginalFrame;
            a.framesData = framesData.Clone() as object[];
            return a;
        }
    }

    public class Animate : ActionInterval
    {
        private readonly SpriteRenderer spriteRenderer;
        protected readonly float[] splitTimes;
        protected int nextFrame;
        protected Sprite origFrame;
        protected int currFrameIndex;
        protected uint executedLoops;

        public delegate void Function(int index, ref object data);

        public Animate(SpriteRenderer spriteRenderer, Animation animation, Function onSpecial = null) :
            base(animation.Duration * animation.Loops)
        {
            this.spriteRenderer = spriteRenderer;
            KeyFrameFunction = onSpecial;
            nextFrame = 0;
            Animation = animation;
            origFrame = null;
            executedLoops = 0;

            splitTimes = new float[animation.Frames.Length];

            float accumUnitsOfTime = 0;
            float newUnitOfTimeValue = animation.Duration / animation.Frames.Length;

            for (int i = 0; i < animation.Frames.Length; ++i)
            {
                float value = (accumUnitsOfTime * newUnitOfTimeValue) / animation.Duration;
                accumUnitsOfTime += 1.0f;
                splitTimes[i] = value;
            }
        }

        public Animation Animation { get; }

        public Function KeyFrameFunction { get; set; }

        public override Action Clone()
        {
            return new Animate(spriteRenderer, Animation.Clone(), KeyFrameFunction);
        }

        public override Action Reverse()
        {
            Sprite[] frames = Animation.Frames.Clone() as Sprite[];
            System.Array.Reverse(frames);

            Animation newAnim = new(frames, Animation.DelayPerUnit);
            newAnim.RestoreOriginalFrame = Animation.RestoreOriginalFrame;
            return new Animate(spriteRenderer, newAnim, KeyFrameFunction);
        }

        public override void Start()
        {
            base.Start();

            if (Animation.RestoreOriginalFrame)
            {
                origFrame = spriteRenderer.sprite;
            }

            nextFrame = 0;
            executedLoops = 0;
        }

        public override void Update(float time)
        {
            // if t==1, ignore. animation should finish with t==1
            if (time < 1.0f)
            {
                time *= Animation.Loops;

                // new loop?  If so, reset frame counter
                uint loopNumber = (uint)time;
                if (loopNumber > executedLoops)
                {
                    nextFrame = 0;
                    ++executedLoops;
                }

                // new t for animations
                time %= 1.0f;
            }

            Sprite[] frames = Animation.Frames;
            int numberOfFrames = frames.Length;
            for (int i = nextFrame; i < numberOfFrames; ++i)
            {
                float splitTime = splitTimes[i];
                if (splitTime <= time)
                {
                    currFrameIndex = i;
                    Sprite frameToDisplay = frames[currFrameIndex];
                    spriteRenderer.sprite = frameToDisplay;

                    if (KeyFrameFunction != null)
                    {
                        object data = Animation.GetFrameData(currFrameIndex);
                        if (data != null)
                        {
                            KeyFrameFunction(currFrameIndex, ref Animation.framesData[currFrameIndex]);
                        }
                    }
                    nextFrame = i + 1;
                }
                else
                {
                    // Issue 1438. Could be more than one frame per tick, due to low frame rate or frame delta < 1/FPS
                    break;
                }
            }
        }
    }
}
