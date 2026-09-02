using UnityEngine;
using System.Collections.Generic;

public enum SkillType { Buff = 0, Emission, Sustained,} 
public enum SkillElement { None = -1, Ice = 100, Fire, Wind, Light, Dark, Count = 5}

[CreateAssetMenu(fileName = "NewSkill", menuName = "SkillAsset", order = 0)]

public class SkillTemplate : ScriptableObject
{
    [Header("공통")]
    public string skillName; // 스킬 이름
    public SkillType skillType; // 스킬 타입
    public SkillElement element; //스킬 속성
    public int maxLevel; // 스킬 최대 레벨

    [TextArea(1, 30)]
    public string description; //스킬 설명 
    public Sprite disableIcon; // 스킬 미습득 아이콘
    public Sprite enableIcon; // 스킬 습득 아이콘 

    [Header("버프 스킬")]
    public List<Stat> buffStatList; // 버프 스킬

    [Header("공격 스킬")]
    public List<Stat> attackBaseStats; // 공격 스킬 스탯 정보
    public List<Stat> attackBuffStats; // 레벨업 시 추가 스탯
    public GameObject projectile; // 발사체 프리팹

}
