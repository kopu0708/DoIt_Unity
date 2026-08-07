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
    public float Value => Mathf.Clamp(defaultValue + bonusValue, minValue, maxValue); // Value 속성은 스탯의 최종값 

    public float DefaultValue // 기본값을 사용하거나 수정할 때 호출하며 
    {
        get => defaultValue;

        set // set을 호출할 때는 메서드를 호출해 각 이벤트에 등록된 메서드를 자동으로 호출한다.
        {
            float prev = Value;  
            defaultValue = Mathf.Clamp(value, minValue, maxValue);
            TryInvokeValueChangedEvent(prev, Value);
        }
    }

    public float BonusValue
    {
        get => bonusValue;
        set => bonusValue = value;
    }

    private void TryInvokeValueChangedEvent(float prev, float current)
    {
        if(!Mathf.Approximately(prev, current))
        {
            OnValueChanged?.Invoke(this, prev, current);

            if (Mathf.Approximately(current, maxValue)) // Approximately는 값이 바뀌었는지 안 바뀌었는지 검사
                OnValueMax?.Invoke(this, prev, maxValue);
            else if (Mathf.Approximately(current, minValue))
                OnValueMin?.Invoke(this, prev, minValue);
        }
    }
}
