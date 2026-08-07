using UnityEngine;
using System.Linq;

[System.Serializable]
public struct EntityStats 
{
    [Header("Level, Exp")]
    public int level; // 레벨
    public long exp; // 경험치 

    [Header("Current HP")]
    [SerializeField]
    private Stat currentHP;

    [Header("Stats")]
    [SerializeField]
    private Stat[] stats;

    public readonly Stat CurrentHP => currentHP;
    public readonly Stat GetStat(Stat stat) =>
        stats.FirstOrDefault(s => s.StatType == stat.StatType);
    public readonly Stat GetStat(StatType statType) =>
        stats.FirstOrDefault(s => s.StatType == statType);

    [Header("Attack")]
    public float damage; // 공격력
    public float cooldownTime; // 기본 공격 쿨타임
    public float criticalChance; // 치명타 확률
    public float criticlaMultiplier;// 크리티컬 공격력

    [Header("Defense")]
    public float MaxHp; // 최대 체력
    public float currentHp; // 현재 체력
    public float evasion; // 회피율
}

