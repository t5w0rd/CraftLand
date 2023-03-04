using System.Collections.Generic;

namespace TGamePlay.TBattle
{
    public class RewardAttribute : Component
    {
        private readonly Reward rewards;

        public RewardAttribute(Reward rewards)
        {
            this.rewards = rewards;
        }

        public List<RewardItem> GetItems()
        {
            return rewards.GetItems();
        }

        public List<RewardItem> GetItemsMerged()
        {
            return rewards.GetItemsMerged();
        }
    }
}
