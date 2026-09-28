using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private ChapterData[] chapters;
    [SerializeField]
    private EnemySpawner enemySpawner;
    [SerializeField]
    private TextMeshPro textStageNumber;
    [SerializeField]
    private UIRewardResult uIRewardResult;
    [SerializeField]
    private PlayerBase player;
    private float enemyCountScale = 0.15f;
    private int currentChapter;
    private int maxStage;
    private int currentStage = 0;
    private int baseEnemyCount = 10;
    private void Start()
    {
        currentChapter = PlayerPrefs.GetInt(Contants.ChapterIndex);
        maxStage = chapters[currentChapter].StageDataTable.maxStage;

        // 현재 스테이지에 남은 적 숫자가 0이면 다음 스테이지로 넘어감
        EnemySpawner.exitEvent.AddListener(SetupStage);
        // 스테이지 설정, 스테이지에 등장하는 적 생성
        SetupStage();
    }

    public void SetupStage()
    {
        currentStage++;

        if(currentStage > maxStage)
        {
            GameClear();
            return;
        }

        //맵에 출력하는 currentStage Text UI 갱신
        textStageNumber.text = $"STAGE {currentStage:D2}";
        // 스테이지에 따라 등장하는 적 숫자 연산/생성
        enemySpawner.SpawnEnemys((int)(baseEnemyCount + currentStage * enemyCountScale));
    }
    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale; 
    }

    public void GameClear()
    {
        SetTimeScale(0); // 게임을 클리어 했을때 TimeScale을 0으로 해 게임을 멈춤

        long baseExp = (long)(currentStage * (5 + (currentChapter + 1) * 1.2f)); // 기본 경험치 보상은 식으로
        long bonusExp = (long)Mathf.Pow(2, (currentChapter + 1)) * 100; // 보너스 보상도 식으로 
        long bonusGem = (currentChapter + 1) * 5000; // 보너스 보석도 식으로 계산 
        bool isNewRecord
            = Database.DBItem.chapters[currentChapter].bestStage != maxStage; // DBItem에 저장된 최고스테이지 기록을 가져와 신기록인지 판단한다.

        // Database 클래스와 DBItem에 데이터 저장 정보를 갱신 
        Database.DBItem.player.experience += (baseExp + bonusExp); 
        Database.DBItem.goods.gem += (player.GEM + bonusGem);
        Database.DBItem.chapters[currentChapter].bestStage = maxStage;

        if (currentChapter + 1 < Database.DBItem.chapters.Length)
            Database.DBItem.chapters[currentChapter + 1].isUnlock = true; // 스테이지를 클리어 했으므로 다음 스테이지를 언락한다.

        Database.Write(); // DBItem에 저장된 데이터를 파일에 저장 

        // 새 기록 여부, 클리어 여부, 챕터, 스테이지, 보상 정보 전달
        // (보상은 원하는 개수 만큼 추가 가능)
        uIRewardResult.OnRewardResult(isNewRecord, true, currentChapter, maxStage,
            new(RewardType, long)[]{
               (RewardType.GEM, player.GEM), (RewardType.EXP,baseExp),
               (RewardType.GEM, bonusGem), (RewardType.EXP, bonusExp) });
    }

    public void GameOver() // gameClear랑 비슷한 구조임 
    {
        SetTimeScale(0);

        long exp = (long)(currentStage * (5 + (currentChapter + 1) * 1.2f));
        bool isNewRecord
            = Database.DBItem.chapters[currentChapter].bestStage < currentStage;

        // Database 클래스의 DBItem에 데이터 저장
        Database.DBItem.player.experience += exp;
        Database.DBItem.goods.gem += player.GEM;
        if (isNewRecord)
            Database.DBItem.chapters[currentChapter].bestStage = currentStage;

        Database.Write(); // DBItem에 저장된 데이터를 파일에 저장

        // 새 기록 여부, 챕터, 스테이지, 보상 정보 전달 (보상은 원하는 개수만큼 추가 가능)
        uIRewardResult.OnRewardResult(isNewRecord, false, currentChapter, currentStage,
            new (RewardType, long)[]
            {(RewardType.GEM, player.GEM), (RewardType.EXP, exp)});
    }
}
