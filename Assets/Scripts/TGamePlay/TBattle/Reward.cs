using System.Collections.Generic;
using UnityEngine;

namespace TGamePlay.TBattle
{
    /// <summary>
    /// 奖励项
    /// </summary>
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

    /// <summary>
    /// 奖励。抽象类，通过继承该类、覆盖GetItems方法，用来实现有趣的奖励方式
    /// </summary>
    public abstract class Reward
    {
        /// <summary>
        /// 获取奖励项列表。通过覆盖该方法，实现自定义获取奖励项列表方式
        /// </summary>
        /// <returns>奖励项列表</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual List<RewardItem> GetItems()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 获取归并奖励项列表。列表中相同名字的奖励项已经被合并，其数量被加和
        /// </summary>
        /// <returns>奖励项列表</returns>
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

    /// <summary>
    /// 单个奖励。里面只有一个奖励项
    /// </summary>
    public class RewardSingle : Reward
    {
        private RewardItem item;

        public RewardSingle(string name, int count)
        {
            item.name = name;
            item.count = count;
        }

        /// <summary>
        /// 获取奖励项列表。然而里面只会有一个奖励项
        /// </summary>
        /// <returns>奖励项列表</returns>
        public override List<RewardItem> GetItems()
        {
            return new() { item };
        }
    }

    /// <summary>
    /// 全部奖励。正如其名，里面的奖励全都会给你！呃...大概
    /// <para>可以构造复杂的奖励形式，示例：奖励2~3个奖励项，必奖励100个银币，5%的概率额外奖励1个金币，20%的概率额外奖励1个宝石，宝石的颜色从红、绿、蓝中随机</para>
    /// <code>
    /// RewardAll ra = new();
    /// 
    /// // 奖励项1：100个银币
    /// ra.AddItem("silver_coin", 100)
    /// 
    /// // 奖励项2：5%概率1个金币
    /// RewardRandom rrGold = new();
    /// rrGold.AddNone(95);
    /// rrGold.AddItem("gold_coin", 1, 5);
    /// ra.AddReward(rrGold);
    /// 
    /// // 奖励项3：20%概率1个宝石，宝石的颜色从红、绿、蓝中随机
    /// RewardRandom rrGem = new();
    /// rrGem.AddNone(80);
    /// RewardRandom rrColor = new();
    /// rrColor.AddItem("red_gem", 1, 1);
    /// rrColor.AddItem("green_gem", 1, 1);
    /// rrColor.AddItem("blue_gem", 1, 1);
    /// rrGem.AddReward(rrColor, 20);
    /// ra.AddReward(rrGem);
    /// </code>
    /// </summary>
    public class RewardAll : Reward
    {
        private readonly List<Reward> items = new();

        /// <summary>
        /// 添加一个奖励。奖励用不嫌多！
        /// </summary>
        /// <param name="reward"></param>
        public void AddReward(Reward reward)
        {
            items.Add(reward);
        }

        /// <summary>
        /// 添加一个奖励项
        /// </summary>
        /// <param name="name"></param>
        /// <param name="count"></param>
        public void AddItem(string name, int count)
        {
            items.Add(new RewardSingle(name, count));
        }

        /// <summary>
        /// 获取奖励项列表。会递归获取全部的奖励项
        /// </summary>
        /// <returns>奖励项列表</returns>
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

    /// <summary>
    /// 带权随机奖励
    /// </summary>
    public class RewardRandom : Reward
    {
        private class RewardNone : Reward
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

        /// <summary>
        /// 添加一个带权奖励
        /// </summary>
        /// <param name="reward"></param>
        /// <param name="weight">权重</param>
        public void AddReward(Reward reward, int weight)
        {
            items.Add(new WeightReward
            {
                item = reward,
                weight = weight,
            });
            sumWeight += weight;
        }

        /// <summary>
        /// 添加一个带权奖励项
        /// </summary>
        /// <param name="name"></param>
        /// <param name="count"></param>
        /// <param name="weight">权重</param>
        public void AddItem(string name, int count, int weight)
        {
            AddReward(new RewardSingle(name, count), weight);
        }

        /// <summary>
        /// 添加一个带权空奖励。如果你在谋划那种“有5%的概率获得中奖券，95%的概率一无所获”的坑钱策略，那么请使用这个
        /// </summary>
        /// <param name="weight">权重</param>
        public void AddNone(int weight)
        {
            AddReward(rewardNone, weight);
        }

        /// <summary>
        /// 获取奖励项列表。根据权重随机获取一个奖励的奖励项列表
        /// </summary>
        /// <returns></returns>
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
