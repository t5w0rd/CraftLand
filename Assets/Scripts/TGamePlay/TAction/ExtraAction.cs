namespace TGamePlay.TAction
{
    public class ExtraAction : FiniteTimeAction
    {
        public override Action Clone()
        {
            return new ExtraAction();
        }

        public override Action Reverse()
        {
            return new ExtraAction();
        }

        public override void Step(float dt) { }

        public override void Update(float time) { }
    }
}
