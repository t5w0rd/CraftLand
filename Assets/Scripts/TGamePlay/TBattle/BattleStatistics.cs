using System.Collections.Generic;

namespace TGamePlay.TBattle
{
    public struct StrIntPair
    {
        public StrIntPair(string s, int i)
        {
            this.s = s;
            this.i = i;
        }

        public string s;
        public int i;
    }

    public class StatItem
    {
        public Dictionary<string, int> integers = new();
        public Dictionary<string, float> floats = new();
        public SortedSet<string> strings = new();
        public List<StrIntPair> pairs = new();
    }

    public class BattleStatistics
    {
        private readonly Dictionary<string, StatItem> items = new();

        public void AddStrIntPair(string path, string s, int i)
        {
            this[path].pairs.Add(new StrIntPair(s, i));
        }

        public List<StrIntPair> GetPairs(string path)
        {
            return this[path].pairs;
        }

        public List<StrIntPair> GetPairsMerged(string path)
        {
            List<StrIntPair> mergeItems = new();
            Dictionary<string, int> index = new();

            var items = GetPairs(path);
            for (int i = 0; i < items.Count; i++)
            {
                if (index.TryGetValue(items[i].s, out int pos))
                {
                    var item = mergeItems[pos];
                    item.i += items[i].i;
                    mergeItems[pos] = item;
                }
                else
                {
                    index.Add(items[i].s, mergeItems.Count);
                    mergeItems.Add(items[i]);
                }
            }
            return mergeItems;
        }

        public int ChangeIntegerValue(string path, string name, int delta)
        {
            var ints = this[path].integers;
            return ints[name] = ints.TryGetValue(name, out var value) ? value + delta : delta;
        }

        public int GetInteger(string path, string name)
        {
            var ints = this[path].integers;
            return ints.TryGetValue(name, out var value) ? value : 0;
        }

        public float ChangeFloatValue(string path, string name, float delta)
        {
            var flts = this[path].floats;
            return flts[name] = flts.TryGetValue(name, out var value) ? value + delta : delta;
        }

        public float GetFloat(string path, string name)
        {
            var flts = this[path].floats;
            return flts.TryGetValue(name, out var value) ? value : 0.0f;
        }

        public void AddUniqString(string path, string s)
        {
            this[path].strings.Add(s);
        }

        public SortedSet<string> GetUniqStrings(string path)
        {
            return this[path].strings;
        }

        private StatItem this[string path]
        {
            get
            {
                if (!items.TryGetValue(path, out StatItem item))
                {
                    item = new();
                    items.Add(path, item);
                }
                return item;
            }
        }
    }
}
