using System.Collections.Generic;
using UnityEngine;

namespace TGamePlay.TBattle
{
    public struct RewardItem
    {
        public string name;
        public int count;

        public RewardItem(string name, int count)
        {
            this.name = name;
            this.count = count;
        }
    }

    public abstract class Reward
    {
        public virtual List<RewardItem> GetItems()
        {
            throw new System.NotImplementedException();
        }

        public List<RewardItem> GetItemsMerged()
        {
            List<RewardItem> mergeItems = new();
            Dictionary<string, int> index = new();

            var items = GetItems();
            for (int i = 0; i < items.Count; i++)
            {
                if (index.TryGetValue(items[i].name, out int pos))
                {
                    var item = mergeItems[pos];
                    item.count += items[i].count;
                    mergeItems[pos] = item;
                }
                else
                {
                    index.Add(items[i].name, mergeItems.Count);
                    mergeItems.Add(items[i]);
                }
            }
            return mergeItems;
        }
    }

    public class RewardSingle : Reward
    {
        public RewardItem item;

        public RewardSingle(string name, int count)
        {
            item.name = name;
            item.count = count;
        }

        public override List<RewardItem> GetItems()
        {
            return new() { item };
        }
    }

    public class RewardAll : Reward
    {
        public List<Reward> items = new();

        public void AddItem(string name, int count)
        {
            items.Add(new RewardSingle(name, count));
        }

        public void AddItems(RewardAll items)
        {
            this.items.Add(items);
        }

        public void AddRandom(RewardRandom random)
        {
            items.Add(random);
        }

        public void AddReward(Reward reward)
        {
            items.Add(reward);
        }

        public override List<RewardItem> GetItems()
        {
            List<RewardItem> rewardItems = new();
            for (int i = 0; i < items.Count; i++)
            {
                rewardItems.AddRange(items[i].GetItems());
            }
            return rewardItems;
        }
    }

    public class RewardRandom : Reward
    {
        public class RewardNone : Reward
        {
            public override List<RewardItem> GetItems()
            {
                return new();
            }
        }

        private static readonly RewardNone rewardNone = new();

        private struct WeightReward
        {
            public int weight;
            public Reward item;
        }

        private readonly List<WeightReward> items = new();
        private int sumWeight = 0;

        public void AddItem(int weight, string name, int count)
        {
            AddReward(weight, new RewardSingle(name, count));
        }

        public void AddItems(int weight, RewardAll items)
        {
            AddReward(weight, items);
        }

        public void AddRandom(int weight, RewardRandom random)
        {
            AddReward(weight, random);
        }

        public void AddNone(int weight)
        {
            AddReward(weight, rewardNone);
        }

        public void AddReward(int weight, Reward reward)
        {
            items.Add(new WeightReward
            {
                weight = weight,
                item = reward,
            });
            sumWeight += weight;
        }

        public override List<RewardItem> GetItems()
        {
            if (items.Count == 0)
            {
                return new();
            }

            int i = 0;
            for (int rnd = Random.Range(0, sumWeight); rnd >= items[i].weight; rnd -= items[i - 1].weight)
            {
                i++;
            }

            return items[i].item.GetItems();
        }
    }
}
