using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIChapterIcon : MonoBehaviour
{
    [SerializeField]
    private GameObject lockedIcon;
    [SerializeField]
    private Image ImageChapter;
    [SerializeField]
    private TextMeshProUGUI textChapterName;
    [SerializeField]
    private TextMeshProUGUI textStage;

    public void Setup(int index, ChapterData chapterData)
    {
        lockedIcon.SetActive(!Database.DBItem.chapters[index].isUnlock);

        ImageChapter.color = chapterData.ChapterDataTable.colorChapter;
        textChapterName.text = $"#{index + 1:D2} {chapterData.ChapterDataTable.chapterName}";
        textStage.text = $"스테이지 {Database.DBItem.chapters[index].bestStage}/ {chapterData.StageDataTable.maxStage}";
    }
}
