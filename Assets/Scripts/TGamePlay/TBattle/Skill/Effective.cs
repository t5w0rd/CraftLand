using System;

namespace TGamePlay.TBattle
{
    [Flags]
    public enum EffectiveFlags
    {
        Self = 1,
        Ally = 1 << 1,
        Enemy = 1 << 2
    }

    public struct Effective
    {
        private EffectiveFlags effectiveFlags;

        public void AddFlags(params EffectiveFlags[] flags)
        {
            for (int i = 0; i < flags.Length; i++)
            {
                effectiveFlags |= flags[i];
            }
        }
    }
}
