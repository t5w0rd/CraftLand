using System.Collections.Generic;

namespace TGamePlay.TBattle
{
    /// <summary>
    /// 战斗势力
    /// </summary>
    public class BattleForce
    {
        private readonly List<BattleGroup> groups = new();
        private int allyMask;

        /// <summary>
        /// 初始化战斗势力
        /// </summary>
        /// <param name="force">势力编号。范围0~29</param>
        public BattleForce(byte force)
        {
            Force = force;
            allyMask = 1 << force;
        }

        /// <summary>
        /// 势力编号。范围0~29
        /// </summary>
        public byte Force { get; }

        /// <summary>
        /// 添加战斗组
        /// </summary>
        /// <param name="group">战斗组</param>
        public void AddGroup(BattleGroup group)
        {
            groups.Add(group);
        }

        /// <summary>
        /// 移除战斗组
        /// </summary>
        /// <param name="group">战斗组</param>
        public void RemoveGroup(BattleGroup group)
        {
            groups.Remove(group);
        }

        /// <summary>
        /// 与一个战斗势力结盟。双方将互为盟友
        /// </summary>
        /// <param name="force">战斗势力</param>
        public void AllyWith(BattleForce force)
        {
            allyMask |= 1 << force.Force;
            force.allyMask |= 1 << Force;
        }

        /// <summary>
        /// 与一个战斗势力解除联盟状态。双方均不再互为盟友
        /// </summary>
        /// <param name="force">战斗势力</param>
        public void SeparateFrom(BattleForce force)
        {
            allyMask ^= 1 << force.Force;
            force.allyMask ^= 1 << Force;
        }

        /// <summary>
        /// 是否是盟友
        /// </summary>
        /// <param name="force">战斗势力</param>
        /// <returns></returns>
        public bool IsAlly(BattleForce force)
        {
            return (allyMask & (1 << force.Force)) != 0;
        }
    }
}
