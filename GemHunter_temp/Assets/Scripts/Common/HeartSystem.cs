using UnityEngine;
using UnityEngine.Events;
using System;

public class HeartSystem : MonoBehaviour
{
    [SerializeField]
    private UnityEvent<int, int> onValueChangedHeart; // 하트 수가 바뀔 때마다 호출
    [SerializeField]
    private UnityEvent<string> onValueChangedTimer; // 충전 시간이 바뀔 때마다 호출

    private int maxHeart; // 최대 하트 개수
    private int currentHeart; // 현재 하트 개수 
    private float timer; // 다음 하트 회복까지 남은 시간(초)
    private float refillTime; // 리필 시간(초)

    public float Timer
    {
        private set
        {
            timer = value;
            onValueChangedTimer.Invoke((currentHeart < maxHeart) ?
                $"{TimeSpan.FromSeconds(timer):mm\\:ss}" : "FULL");
        }

        get => timer;
    }

    public int CurrentHeart
    {
        private set
        {
            currentHeart = value;
            onValueChangedHeart.Invoke(currentHeart, maxHeart);
        }

        get => currentHeart;
    }

    private void Awake()
    {
        maxHeart = Database.DBItem.goods.maxHeart;
        refillTime = Database.DBItem.goods.heartRefillTime;

        LoadData();
    }

    private void Update()
    {
        if(CurrentHeart < maxHeart)
        {
            Timer -= Time.deltaTime;

            // 충전 시간 도달 시
            if(Timer <= 0f)
            {
                CurrentHeart++;

                // 회복할 하트가 남았다면 타이머를 리필 타임으로 초기화 시켜줌
                Timer = refillTime;

                // 하트, 시간 정보 저장
                SaveData();
            }
        }
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause) SaveData();
    }
    private void OnApplicationFocus(bool focus)
    {
        if (focus) LoadData();
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    public bool UseHeart(int count)
    {
        if(CurrentHeart >= count)
        {
            CurrentHeart -= count;

            // 하트 개수가 최대일 때 처음 하트를 소모했으면
            // 충전에 필요한 시간(timer)을 refillTime으로 초기화
            if (CurrentHeart == maxHeart - count) Timer = refillTime;

            SaveData();
            return true;
        }

        return false;
    }

    private void SaveData()
    {
        Database.DBItem.goods.heart = CurrentHeart;
        Database.DBItem.goods.heartTimer = Timer;
        Database.DBItem.goods.heartLastTime = DateTime.UtcNow.ToBinary().ToString();
        Database.Write();
    }

    private void LoadData()
    {
        CurrentHeart = Database.DBItem.goods.heart;
        string lastTimeString = Database.DBItem.goods.heartLastTime;

        // 이전 실행에서 저장된 데이터가 있다면
        if (!string.IsNullOrEmpty(lastTimeString))
        {
            DateTime lastTime = DateTime.FromBinary(Convert.ToInt64(lastTimeString));
            // 이전 저장 시 남은 timer가 있으면 그 값을 사용, 없으면 refillTime 사용
            float savedTimer = Database.DBItem.goods.heartTimer;
            // 앱 종료 후 흐른 시간
            float elapesd = (float)(DateTime.UtcNow - lastTime).TotalSeconds;
            // 이전에 남은 시간을 합쳐 총 경과 시간 계산
            float totalElapsed = elapesd + (refillTime - savedTimer);

            // 경과 시간 동안 회복할 수 있는 하트 수 계산
            int heartTORecover = Mathf.FloorToInt((float)totalElapsed / refillTime);
            CurrentHeart = Mathf.Min(CurrentHeart + heartTORecover, maxHeart);

            // 현재 하트가 개수가 최대라면 timer에 refillTime 저장
            if (CurrentHeart >= maxHeart) Timer = refillTime;
            // 현재 하트수가 최대가 아니라면 남은 시간을 계산해서 저장
            else Timer = refillTime - (totalElapsed % refillTime);
        }
        // 처음 실행하거나 데이터가 없다면
        else Timer = refillTime;
    }
}
