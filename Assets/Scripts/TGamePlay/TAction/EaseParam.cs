using System;

namespace TGamePlay.TAction
{
    public class EaseParam : ActionEase
    {
        public delegate float EaseParamFunction(float time, float param);

        public EaseParam(ActionInterval action, EaseParamFunction func, float param)
            : base(action)
        {
            Function = func;
            Param = param;
        }

        protected EaseParamFunction Function { get; set; }

        public float Param { get; set; }

        public override Action Clone()
        {
            return new EaseParam(inner.Clone() as ActionInterval, Function, Param);
        }

        public override Action Reverse()
        {
            return new EaseParam(inner.Reverse() as ActionInterval, Function, Param);
        }

        public override void Update(float time)
        {
            inner.Update(Function != null ? Function(time, Param) : time);
        }

        public static float FuncPowIn(float time, float param)
        {
            return (float)Math.Pow(time, param);
        }

        public static float FuncPowOut(float time, float param)
        {
            return (float)Math.Pow(time, 1 / param);
        }

        public static float FuncPowInOut(float time, float param)
        {
            time *= 2;
            if (time < 1)
            {
                return (float)(0.5f * Math.Pow(time, param));
            }
            else
            {
                return ((float)(1 - 0.5f * Math.Pow(2 - time, param)));
            }
        }

        protected const float _PIx2 = (float)(Math.PI * 2);

        public const float defElasticPeriod = 0.3f;

        public static float FuncElasticIn(float time, float period)
        {
            float newT;
            if (time == 0 || time == 1)
            {
                newT = time;
            }
            else
            {
                float s = period / 4;
                --time;
                newT = (float)(-Math.Pow(2, 10 * time) * Math.Sin((time - s) * _PIx2 / period));
            }

            return newT;
        }

        public static float FuncElasticOut(float time, float period)
        {
            float newT;
            if (time == 0 || time == 1)
            {
                newT = time;
            }
            else
            {
                float s = period / 4;
                newT = (float)(Math.Pow(2, -10 * time) * Math.Sin((time - s) * _PIx2 / period) + 1);
            }

            return newT;
        }

        public static float FuncElasticInOut(float time, float period)
        {
            float newT;
            if (time == 0 || time == 1)
            {
                newT = time;
            }
            else
            {
                time *= 2;
                if (period == 0)
                {
                    period = 0.3f * 1.5f;
                }

                float s = period / 4;

                --time;
                if (time < 0)
                {
                    newT = (float)(-0.5f * Math.Pow(2, 10 * time) * Math.Sin((time - s) * _PIx2 / period));
                }
                else
                {
                    newT = (float)(Math.Pow(2, -10 * time) * Math.Sin((time - s) * _PIx2 / period) * 0.5f + 1);
                }
            }

            return newT;
        }
    }
}
