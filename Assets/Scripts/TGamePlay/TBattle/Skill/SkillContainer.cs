using System.Collections.Generic;

namespace TGamePlay.TBattle
{
    public class SkillContainer : ISkillContainer
    {
        private readonly Dictionary<int, Skill> skills = new();

        public SkillContainer() { }

        public int SkillCount => skills.Count;

        public void AddSkill(Skill skill)
        {
            skills.Add(skill.ID, skill);
            skill.AddToContainer(this);
        }

        public Skill FindSkill(int id)
        {
            bool ok = skills.TryGetValue(id, out Skill obj);
            return ok ? obj : null; ;
        }

        public T FindSkill<T>(int id) where T : Skill
        {
            return FindSkill(id) as T;
        }

        public void RemoveSkill(int id)
        {
            if (!skills.TryGetValue(id, out Skill skill))
            {
                return;
            }

            skill.RemoveFromContainer();
            _ = skills.Remove(id);
        }

        public void UpdateSkillCoolDown(float delta)
        {
            foreach (Skill skill in skills.Values)
            {
                skill.UpdateCoolDown(delta);

                if (skill.Interval > 0.0f)
                {
                    skill.IntervalElapsed += delta;

                    while (skill.IntervalElapsed >= skill.Interval)
                    {
                        skill.OnInterval();
                        if (skill.Interval > 0.0f)
                        {
                            skill.IntervalElapsed -= skill.Interval;
                        }
                        else
                        {
                            skill.IntervalElapsed = 0f;
                            break;
                        }
                    }
                }
            }
        }
    }
}
