namespace TGamePlay.TBattle
{
    public interface ISkillContainer
    {
        void AddSkill(Skill prop);

        void RemoveSkill(int id);

        Skill FindSkill(int id);
        T FindSkill<T>(int id) where T : Skill;

        int SkillCount { get; }

        void UpdateSkillCoolDown(float delta);
    }
}
