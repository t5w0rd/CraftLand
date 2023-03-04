namespace TGamePlay.TAction
{
    public class FiniteTimeAction : Action
    {
        public FiniteTimeAction()
        {
            Duration = 0.0f;
        }

        public float Duration { get; protected set; }
    }
}
