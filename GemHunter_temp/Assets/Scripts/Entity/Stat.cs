using System.Data;
using UnityEngine;

public enum StatType {Damage = 0, CooldownTime, CriticalChance, CriticalMultiplier, HP, Evasion, }
[System.Serializable]
public class Stat
{
    public delegate void ValueChangedHandler(Stat stat, float prev, float current);
    public event ValueChangedHandler OnValueChanged;
    public event ValueChangedHandler OnValueMax;
    public event ValueChangedHandler OnValueMin;

    [SerializeField]
    private StatType statType;
    [SerializeField]
    private float maxValue;
    [SerializeField]
    private float minValue;
    [SerializeField]
    private float defaultValue;
    [SerializeField]
    private float bonusValue;

    public StatType StatType => statType;

}
