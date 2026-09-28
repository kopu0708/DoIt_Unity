using UnityEngine;
using Newtonsoft.Json;
using System.IO;
 
public static class Database //세이브/로드 기능을 제공하는 유틸리티 성격이라, 인스턴스(객체)를 만들 필요가 없다.
// 그래서 static class로 정의해서, 인스턴스 생성 없이 Database.Write()처럼 
// 클래스 이름으로 바로 접근/호출할 수 있게 만들었다.
{
    // 플레이어 데이터를 저장하고 불러오는 파일 이름 
    public static readonly string DBFileName = "Database.dat"; // 파일 형식을 dat으로 쓴것은 JSON이라고 티 안나게 하기 위해서 이다. 수정하기 쉬우니깐
    public static readonly int maxChapter = 3;

    // 게임에서 사용하는 모든 데이터를 저장하는 클래스
    public static DatabaseItem DBItem { get; private set; } = new DatabaseItem(); // 실제로 저장/불러올 모든 데이터를 담을 그릇
                                                                                  // private set이라 "DBItem = 새그릇" 처럼 통째로 교체하는 건 외부에서 불가능
                                                                                  // (단, DatabaseItem이 class라서 DBItem.필드 = 값 처럼 내부 필드를 직접 바꾸는 건 막지 못함)
    public static bool IsRead { get; private set; } = false; // 세이브 데이터를 이미 불러오는 중인지 나타내는 플래그

    /// <summary>
    /// 파일에 데이터를 저장한다.
    /// </summary>
    public static void Write()
    {
        Logger.Log("Database::Write - Save data in Database");

        string path = Path.Combine(Application.persistentDataPath, DBFileName); // 이 게임이 영구 데이터를 안전하게 저장해도 되는 경로
                                                                                // Combine은 os 마다 구분자가 다르기에 이를 알아서 처리해줌 

        // JSON 직렬화
        string json = JsonConvert.SerializeObject(DBItem, Formatting.Indented);
        // 파일에 데이터 저장
        File.WriteAllText(path, json);
    }

    /// <summary>
    /// 파일에 데이터를 불러온다.
    /// </summary>
    public static void Read() 
    {
        Logger.Log("Database::Read - Load data in Database");

        IsRead = true;

        string path = Path.Combine(Application.persistentDataPath, DBFileName);

        // 파일이 있다면
        if (File.Exists(path))
        {
            Logger.Log("Database::Read - File Exist in Folder");

            // 파일에서 데이터 불러옴
            string json = File.ReadAllText(path);
            // JSON 역직렬화 
            DBItem = JsonConvert.DeserializeObject<DatabaseItem>(json); // JSON 문자열을 다시 DatabaseItem로 되돌린다.
                                                                        // 필드 이름과 JSON키 이름이 같다면 알아서 값을 채워준다.
            if (DBItem == null)
            {
                Reset();
            }
            else
            {
                if (DBItem.player == null) DBItem.player = new DBItem_Player();
                if (DBItem.player.level < 1) DBItem.player.level = 1;
                if (DBItem.goods == null) { DBItem.goods = new DBItem_Goods(); DBItem.goods.Reset(); }
                if (DBItem.chapters == null || DBItem.chapters.Length != Database.maxChapter)
                {
                    var oldChapters = DBItem.chapters;
                    DBItem.chapters = new DBItem_Chapter[Database.maxChapter];
                    for (int i = 0; i < DBItem.chapters.Length; ++i)
                    {
                        if (oldChapters != null && i < oldChapters.Length && oldChapters[i] != null)
                            DBItem.chapters[i] = oldChapters[i];
                        else
                        {
                            DBItem.chapters[i] = new DBItem_Chapter();
                            DBItem.chapters[i].Reset();
                        }
                    }
                    DBItem.chapters[0].isUnlock = true;
                }
            }
        }
        // 파일이 없다면
        else
        {
            Logger.Log("Database::Read - File Not Exist in Folder");
            Reset();
        }

        IsRead = false;
    }

    public static void Reset() // 첫시작 하거나 초기화를 원하면 실행 기본값으로 채우고 바로 저장 
    {
        // DBItem이 null이면 메모리 할당
        if (DBItem == null) DBItem = new DatabaseItem();
        // 전체 데이터 초기화
        DBItem.Reset();
        // 파일에 초기화된 데이터 저장
        Write();
    }
}


[System.Serializable]
public class DatabaseItem
{
    public DBItem_Player player;
    public DBItem_Goods goods;
    public DBItem_Chapter[] chapters;

    public DatabaseItem()
    {
        player = new DBItem_Player();
        goods = new DBItem_Goods();
        chapters = new DBItem_Chapter[Database.maxChapter];
        for(int i = 0; i < chapters.Length; ++i)
        {
            chapters[i] = new DBItem_Chapter();
        }
        Reset();
    }

    public void Reset()
    {
        player.Reset();
        goods.Reset();

        for(int i = 0; i < chapters.Length; ++i)
        {
            chapters[i].Reset();
        }
        // 첫 번째 챕터 풀림 여부를 true로 설정
        chapters[0].isUnlock = true;
    }
}

[System.Serializable]
public class DBItem_Player
{
    public int level = 1;
    public float experience;

    public void Reset()
    {
        level = 1;
        experience = 0f;
    }
}

[System.Serializable]
public class DBItem_Goods
{
    public int heart;
    public float heartTimer; // 하트 충전까지 남은 시간(초)
    public string heartLastTime; // 게임 종료 시간
    public float gem;

    public readonly int maxHeart = 50; // 최대 하트 개수
    public readonly float heartRefillTime = 20 * 60; // 하트 회복 시간(초): 20분

    public void Reset()
    {
        heart = maxHeart;
        heartTimer = 0f;
        heartLastTime = string.Empty;
        gem = 0;
    }
}

[System.Serializable]
public class DBItem_Chapter
{
    public bool isUnlock; // 챕터 잠금 해제 여부
    public int bestStage; // 현재 챕터에서 도달한 최고 스테이지

    public void Reset()
    {
        isUnlock = false;
        bestStage = 1;
    }
}