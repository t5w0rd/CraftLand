namespace TGamePlay.TAction
{
    public class ActionInstant : FiniteTimeAction
    {
        public override void Step(float dt)
        {
            Update(1.0f);
        }

        public override void Update(float time) { }
    }
}
