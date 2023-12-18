using System.Collections;
using System.Collections.Generic;
using TGamePlay.TBattle;
using UnityEngine;

public class DefaultRewardLibary
{
    private static DefaultRewardLibary instance = null;
    private static DefaultRewardLibary Instance => instance ??= new DefaultRewardLibary();

    private readonly Dictionary<string, Reward> rewards = new();

    public DefaultRewardLibary()
    {
        RewardRandom random;
        RewardAll all;

        random = new();
        random.AddItem("TestPosion", 1, 50);
        random.AddItem("TestScroll", 1, 50);
        rewards["normal_0"] = random;

        random = new();
        random.AddItem("TestPosion", 1, 30);
        random.AddNone(70);
        rewards["normal_1"] = random;

        random = new();
        random.AddItem("TestScroll", 1, 30);
        random.AddNone(70);
        rewards["normal_2"] = random;

        all = new();
        all.AddReward(rewards["normal_0"]);
        all.AddReward(rewards["normal_1"]);
        all.AddReward(rewards["normal_2"]);
        rewards["normal_3"] = all;
    }

    public Reward this[string name] => rewards.TryGetValue(name, out var value) ? value : null;

    public static Reward GetReward(string name)
    {
        return Instance[name];
    }
}
