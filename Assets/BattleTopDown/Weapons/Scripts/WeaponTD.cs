using TGamePlay.TBattle;
using UnityEngine;

public abstract class WeaponTD : MonoBehaviour
{
    public RectTransform hitRect;
    public string weaponName;
    public int atk = 10;

    public float speedRate = 1f;
    public static int speedRateHash = Animator.StringToHash("WeaponSpeedRate");

    public int kind { get; protected set; }
    public int unitID { get; internal set; }
    [HideInInspector] public DamageInfo damageInfo;
}
