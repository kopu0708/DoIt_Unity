using UnityEngine;

[CreateAssetMenu]
public class ChapterData : ScriptableObject
{
    [SerializeField]
    private ChapterDataTable chapterDataTable;
    [SerializeField]
    private StageDataTable stageDataTable;
    public ChapterDataTable ChapterDataTable => chapterDataTable;
    public StageDataTable StageDataTable => stageDataTable;
}

[System.Serializable]
public struct ChapterDataTable
{
    public Sprite spriteChapter; // 챕터 배경 이미지
    public Color colorChapter; // Debug.. 현재는 챕터 이미지가 없어서 색상 변경
    public string chapterName; // 챕터 이름
}

[System.Serializable]
public struct StageDataTable
{
    public int maxStage; // 최대 스테이지 
    public int baseEnemyCount; // 기본 적 숫자
    public int baseEnemyLevel; // 기본 적 레벨
    public GameObject[] enemyPrefabs; // 등장하는 적 프리팹 배열 
}