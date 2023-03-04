namespace TGamePlay.TAction
{
    public delegate void Function();

    public class CallFunc : ActionInstant
    {
        public Function Function { get; protected set; }

        public CallFunc(Function func)
        {
            Function = func;
        }

        public override Action Clone()
        {
            return new CallFunc(Function);
        }

        public override Action Reverse()
        {
            return Clone();
        }

        protected virtual void Execute()
        {
            Function.Invoke();
        }

        public override void Update(float time)
        {
            Execute();
        }
    }
}
