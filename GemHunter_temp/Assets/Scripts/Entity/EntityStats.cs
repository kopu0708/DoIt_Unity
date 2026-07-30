using UnityEngine;

[System.Serializable]
public class EntityStats 
{
    [Header("Level, Exp")]
    public int level; // 레벨
    public long exp; // 경험치 

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

