using System;

namespace TGamePlay.TAction
{
    public class Ease : ActionEase
    {
        public delegate float EaseFunction(float time);

        public Ease(ActionInterval action, EaseFunction func) :
            base(action)
        {
            Function = func;
        }

        public EaseFunction Function { get; set; }

        public override Action Clone()
        {
            return new Ease(inner.Clone() as ActionInterval, Function);
        }

        public override Action Reverse()
        {
            return new Ease(inner.Reverse() as ActionInterval, Function);
        }

        public override void Update(float time)
        {
            inner.Update(Function != null ? Function(time) : time);
        }

        protected const float _PI2 = (float)(Math.PI / 2);

        public static float FuncSineIn(float time)
        {
            return (float)(-Math.Cos(time * _PI2) + 1);
        }

        public static float FuncSineOut(float time)
        {
            return (float)Math.Sin(time * _PI2);
        }

        public static float FuncSineInOut(float time)
        {
            return (float)(-0.5f * (Math.Cos(_PI2 * time) - 1));
        }

        public static float FuncQuadIn(float time)
        {
            return time * time;
        }

        public static float FuncQuadOut(float time)
        {
            return -time * (time - 2);
        }

        public static float FuncQuadInOut(float time)
        {
            time *= 2;
            if (time < 1)
            {
                return 0.5f * time * time;
            }
            --time;
            return -0.5f * (time * (time - 2) - 1);
        }

        public static float FuncCubicIn(float time)
        {
            return time * time * time;
        }

        public static float FuncCubicOut(float time)
        {
            --time;
            return time * time * time + 1;
        }

        public static float FuncCubicInOut(float time)
        {
            time *= 2;
            if (time < 1)
            {
                return 0.5f * time * time * time;
            }
            time -= 2;
            return 0.5f * (time * time * time + 2);
        }

        public static float FuncQuartIn(float time)
        {
            return time * time * time * time;
        }

        public static float FuncQuartOut(float time)
        {
            --time;
            return -time * time * time * time + 1;
        }

        public static float FuncQuartInOut(float time)
        {
            time *= 2;
            if (time < 1)
            {
                return 0.5f * time * time * time * time;
            }
            time -= 2;
            return -0.5f * (time * time * time * time - 2);
        }

        public static float FuncQuintIn(float time)
        {
            return time * time * time * time * time;
        }

        public static float FuncQuintOut(float time)
        {
            --time;
            return time * time * time * time * time + 1;
        }

        public static float FuncQuintInOut(float time)
        {
            time *= 2;
            if (time < 1)
            {
                return 0.5f * time * time * time * time * time;
            }
            time -= 2;
            return 0.5f * (time * time * time * time * time + 2);
        }

        public static float FuncExpoIn(float time)
        {
            return (float)(time == 0 ? 0 : Math.Pow(2, 10 * (time / 1 - 1)) - 1 * 0.001f);
        }

        public static float FuncExpoOut(float time)
        {
            return (float)(time == 1 ? 1 : (-Math.Pow(2, -10 * time / 1) + 1));
        }

        public static float FuncExpoInOut(float time)
        {
            time *= 2;
            if (time < 1)
            {
                time = (float)(0.5f * Math.Pow(2, 10 * (time - 1)));
            }
            else
            {
                time = (float)(0.5f * (-Math.Pow(2, -10 * (time - 1)) + 2));
            }

            return time;
        }

        public static float FuncCircIn(float time)
        {
            return (float)(1 - Math.Sqrt(1 - time * time));
        }

        public static float FuncCircOut(float time)
        {
            --time;
            return (float)Math.Sqrt(1 - time * time);
        }

        public static float FuncCircInOut(float time)
        {
            time *= 2;
            if (time < 1)
            {
                return (float)(-0.5f * (Math.Sqrt(1 - time * time) - 1));
            }
            time -= 2;
            return (float)(0.5f * (Math.Sqrt(1 - time * time) + 1));
        }

        public static float FuncBackIn(float time)
        {
            const float overshoot = 1.70158f;
            return time * time * ((overshoot + 1) * time - overshoot);
        }

        public static float FuncBackOut(float time)
        {
            const float overshoot = 1.70158f;
            --time;
            return time * time * ((overshoot + 1) * time + overshoot) + 1;
        }

        public static float FuncBackInOut(float time)
        {
            const float overshoot = 1.70158f * 1.525f;
            time *= 2;
            if (time < 1)
            {
                return (time * time * ((overshoot + 1) * time - overshoot)) / 2;
            }
            else
            {
                time -= 2;
                return (time * time * ((overshoot + 1) * time + overshoot)) / 2 + 1;
            }
        }

        protected static float BounceTime(float time)
        {
            if (time < 1 / 2.75)
            {
                return 7.5625f * time * time;
            }
            else if (time < 2 / 2.75)
            {
                time -= 1.5f / 2.75f;
                return 7.5625f * time * time + 0.75f;
            }
            else if (time < 2.5 / 2.75)
            {
                time -= 2.25f / 2.75f;
                return 7.5625f * time * time + 0.9375f;
            }

            time -= 2.625f / 2.75f;
            return 7.5625f * time * time + 0.984375f;
        }

        public static float FuncBounceIn(float time)
        {
            return 1 - BounceTime(1 - time);
        }

        public static float FuncBounceOut(float time)
        {
            return BounceTime(time);
        }

        public static float FuncBounceInOut(float time)
        {
            float newT;
            if (time < 0.5f)
            {
                time *= 2;
                newT = (1 - BounceTime(1 - time)) * 0.5f;
            }
            else
            {
                newT = BounceTime(time * 2 - 1) * 0.5f + 0.5f;
            }

            return newT;
        }
    }
}
