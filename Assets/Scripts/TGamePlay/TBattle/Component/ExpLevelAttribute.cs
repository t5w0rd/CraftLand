using System.Collections.Generic;

namespace TGamePlay.TBattle
{
    public class ExpLevelAttribute : Component
    {
        private int level;
        private int maxLevel;
        private int exp;
        private int baseExp;
        private int maxExp;
        private readonly List<int> maxExpTable;

        public ExpLevelAttribute(int[] maxExpTable)
        {
            this.maxExpTable = new(maxExpTable);
            maxLevel = this.maxExpTable.Count;
            level = 0;
            UpdateExpRange();
            AddExp(0);
        }

        public ExpLevelAttribute(int[] maxExpTable, int level, int maxLevel, int exp)
        {
            this.maxExpTable = new(maxExpTable);
            MaxLevel = maxLevel;
            Level = level;
            UpdateExpRange();

            this.exp = exp < maxExp ? exp : maxExp;
            AddExp(0);
        }

        public void AddExp(int exp)
        {
            for (bool lu = AddExpUntilLevelUp(exp, out exp); lu; lu = AddExpUntilLevelUp(exp, out exp)) { }
        }

        public bool AddExpUntilLevelUp(int exp, out int left)
        {
            if (level >= maxLevel || exp < 0)
            {
                left = 0;
                return false;
            }

            int leftExp = this.exp + exp - maxExp;
            if (leftExp < 0)
            {
                this.exp += exp;
                left = 0;
                return false;
            }

            this.exp = maxExp;
            Level++;
            if (level == maxLevel)
            {
                this.exp = baseExp;
                left = 0;
            }
            else
            {
                left = leftExp;
            }
            return true;
        }

        public int Level
        {
            get => level;
            set
            {
                int oldLevel = level;

                if (value < 0)
                {
                    level = 0;
                }
                else if (value > maxLevel)
                {
                    level = maxLevel;
                }
                else
                {
                    level = value;
                }

                int changed = level - oldLevel;

                if (changed != 0)
                {
                    UpdateExpRange();
                }
            }
        }

        public int MaxLevel
        {
            get => maxLevel;
            set
            {
                if (maxLevel < 0)
                {
                    maxLevel = 0;
                }
                else if (maxLevel > maxExpTable.Count)
                {
                    maxLevel = maxExpTable.Count;
                }
                else
                {
                    maxLevel = value;
                }
                Level = level;
            }
        }

        public int Exp { get => exp; }

        public int BaseExp { get => baseExp; }

        public int MaxExp { get => maxExp; }

        private void GetExpRange(int level, out int baseExp, out int maxExp)
        {
            if (level == 0)
            {
                baseExp = 0;
            }
            else
            {
                baseExp = maxExpTable[level - 1];
            }

            if (level == maxLevel)
            {
                maxExp = baseExp + 1;
            }
            else
            {
                maxExp = maxExpTable[level];
            }
        }

        public void UpdateExpRange()
        {
            GetExpRange(level, out baseExp, out maxExp);
        }
    }
}
