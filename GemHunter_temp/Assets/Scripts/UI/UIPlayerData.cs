using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIPlayerData : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textLevel;
    [SerializeField]
    private Image fillGaugeEXP;
    [SerializeField]
    private PlayerBase entity;

    private void Awake()
    {
        entity.Stats.CurrentExp.OnValueChanged += UpdateEXP;
    }

    private void UpdateEXP(Stat stat, float prev, float current)
    {
        textLevel.text = $"Lv.{entity.Stats.GetStat(StatType.Level).Value}";
        fillGaugeEXP.fillAmount = entity.Stats.CurrentExp.Value /
            entity.Stats.GetStat(StatType.Experience).Value;
    }
}
