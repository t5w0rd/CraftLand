using System;

namespace TGamePlay.TBattle
{
    public class FightAttribute : Component, IUnitComponent
    {
        private CoefValue atk;
        private CoefValue def;

        #region IUnitComponent
        public Unit Unit => Container as Unit;
        #endregion

        protected override void OnAdd()
        {
            var hp = GetComponent<HPAttribute>();
            if (hp == null)
            {
                hp = Container.AddComponent(new HPAttribute(100));
            }
            HP = hp;
            Unit.Fight = this;
        }

        protected override void OnRemove()
        {
            Unit.Fight = null;
        }

        public HPAttribute HP { get; private set; }

        public int ATK => atk.Value;

        public int ATKRaw
        {
            get => atk.ValueRaw;
            set => atk.ValueRaw = value;
        }

        public Coefficient ATKCoef { get => atk.Coef; }

        public void ChangeATKCoefA(int delta) => atk.Coef.ChangeA(delta);

        public void ChangeATKCoefB(int delta) => atk.Coef.ChangeB(delta);

        public int DEF => def.Value;

        public int DEFRaw
        {
            get => def.ValueRaw;
            set => def.ValueRaw = value;
        }

        public Coefficient DEFCoef { get => def.Coef; }

        public void ChangeDEFCoefA(int delta) => def.Coef.ChangeA(delta);

        public void ChangeDEFCoefB(int delta) => def.Coef.ChangeB(delta);

        public DamageInfo Attack()
        {
            DamageInfo value = new(Unit, atk.Value);

            OnAttack?.Invoke(ref value);

            return value;
        }

        public DamageInfo PreDamage(DamageInfo damage)
        {
            OnPreDamage?.Invoke(ref damage);

            return damage;
        }

        public int Damage(DamageInfo damage)
        {
            OnDamage?.Invoke(ref damage);

            var defValue = def.Value * 1.0f;
            var dmgValue = (int)(damage.Value * (1.0f - defValue / (defValue + 500.0f)));
            HP.HP -= dmgValue;
            return dmgValue;
        }

        public void Update(float delta)
        {
            OnUpdate?.Invoke(delta);
        }

        #region EventSystem
        [Flags]
        public enum Event
        {
            None = 0,
            OnAttack = 1,
            OnPreDamage = 1 << 1,
            OnDamage = 1 << 2,
            OnUpdate = 1 << 3,
        }

        public interface IEventListener
        {
            void OnAttack(ref DamageInfo value);
            void OnPreDamage(ref DamageInfo value);
            void OnDamage(ref DamageInfo value);
            void OnUpdate(float delta);
        }

        public delegate void Modifier<T>(ref T value);

        private event Modifier<DamageInfo> OnAttack;
        private event Modifier<DamageInfo> OnPreDamage;
        private event Modifier<DamageInfo> OnDamage;
        private event Action<float> OnUpdate;

        public void RegisterEventListener(Event events, IEventListener listener)
        {
            if (events.HasFlag(Event.OnAttack))
            {
                OnAttack += listener.OnAttack;
            }

            if (events.HasFlag(Event.OnPreDamage))
            {
                OnPreDamage += listener.OnPreDamage;
            }

            if (events.HasFlag(Event.OnDamage))
            {
                OnDamage += listener.OnDamage;
            }

            if (events.HasFlag(Event.OnUpdate))
            {
                OnUpdate += listener.OnUpdate;
            }
        }

        public void UnRegisterEventListener(Event events, IEventListener listener)
        {
            if (events.HasFlag(Event.OnAttack))
            {
                OnAttack -= listener.OnAttack;
            }

            if (events.HasFlag(Event.OnPreDamage))
            {
                OnPreDamage -= listener.OnPreDamage;
            }

            if (events.HasFlag(Event.OnDamage))
            {
                OnDamage -= listener.OnDamage;
            }

            if (events.HasFlag(Event.OnUpdate))
            {
                OnUpdate -= listener.OnUpdate;
            }
        }
        #endregion
    }
}
