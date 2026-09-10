using UnityEngine;
using System.Collections.Generic;

public class UISkillList : MonoBehaviour
{
    [SerializeField]
    private UISkillIcon skillIconPrefab; // 아이콘 하나의 '틀'이 되는 프리팹 
    [SerializeField]
    private Transform[] skillElementType; // UI를 띄울 위치를 저장하는 배열 (속성별)
    [SerializeField]
    private Transform skillElementalBonus; // 스킬 속성 보너스 UI 위치

    private Dictionary<string, UISkillIcon> skillIcons; // 스킬 이름 -> 그 스킬의 UI 아이콘

    public void SetUp(Dictionary<string, SkillTemplate> skills,  // 버프와 공격 스킬의 이름과 정보를
        Dictionary<string, SkillTemplate> elementalBonus) // 속성 보너스 스킬의 정보를 넘겨받는다.
    {
        skillIcons = new Dictionary<string, UISkillIcon>();     

        foreach( var item in skills)
        {
            SpawnIcon(item.Value, skillElementType[(int)item.Value.element - 100]); // 속성들은 100번대로 했었으니 스킬 속성값을 가져와 - 100으로 배열 인덱스로 변환 
        }

        foreach(var item in elementalBonus)
        {
            SpawnIcon(item.Value, skillElementalBonus); // 이건 배열이 아니라 고정된 하나의 칸
        }
    }

    public void LevelUp(SkillBase skill)
    {
        if (skillIcons.ContainsKey(skill.SkillName)) // 딕션너리에서 이름으로 스킬 찾기
        {
            skillIcons[skill.SkillName].LevelUp(skill.CurrentLevel, skill.EnableIcon); //레벨이 올랐으면 활성 아이콘으로 바꾸고 레벨 숫자 표시
        }
    }


    private void SpawnIcon(SkillTemplate skill, Transform parent) // 부모 밑에 아이콘 생성
    {
        var clone = Instantiate(skillIconPrefab, parent); // parent를 부모로 지정해서 그 자식으로 아이콘 생성 (Instantiate(prefab, parent) 오버로드)
        clone.transform.localScale = Vector3.one; // 크기 보정(부모 자식 관계 맺을 때 스케일 틀어지는 걸 방지)
        clone.Setup(skill.disableIcon); // 아직 습득 전이니 "비활성 아이콘" 이미지로 세팅

        skillIcons.Add(skill.skillName, clone);    // 나중에 찾을 수 있도록 딕셔너리에 등록
    }
}
