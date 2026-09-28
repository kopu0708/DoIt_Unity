using UnityEngine;
using TMPro;

public enum RewardType { GEM, EXP, ITEM }

public class UIRewardResult : MonoBehaviour
{
    [SerializeField]
    private GameController gameController;
    [SerializeField]
    private GameObject panelResult;
    [SerializeField]
    private GameObject textNewRecord;
    [SerializeField]
    private TextMeshProUGUI textTheme;
    [SerializeField]
    private TextMeshProUGUI textChapter;
    [SerializeField]
    private TextMeshProUGUI textStage;
    [SerializeField]
    private Transform rewardParent;
    [SerializeField]
    private UIRewardIcon[] rewards;

    public void OnRewardResult(bool isNewRecord, bool isClear, 
        int chapter, int stage, (RewardType, long)[] items)
    {
        panelResult.SetActive(true);
        textNewRecord.SetActive(isNewRecord);

        if (isClear == true) textTheme.text = "챕터 클리어";
        else textTheme.text = "게임 오버";

        textChapter.text = $"CHAPTER {chapter + 1:D2}";
        textStage.text = stage.ToString();

        UIRewardIcon item;
        for(int i = 0; i < items.Length; ++i)
        {
            item = Instantiate(rewards[(int)items[i].Item1], rewardParent);
            item.SetReward(items[i].Item2);
        }
    }

    public void ButtonEvent_ReturnLobby()
    {
        gameController.SetTimeScale(1);
        SceneLoader.Instance.LoadScene(SceneNames.Lobby);
    }
}
