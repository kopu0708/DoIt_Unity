using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISkillIcon : MonoBehaviour
{
    [SerializeField]
    private Image skillIcon;
    [SerializeField]
    private TextMeshProUGUI skillLevel;

    public void Setup(Sprite defaultSprite)
    {
        skillIcon.sprite = defaultSprite;
        skillLevel.text = "-";
    }

    public void LevelUp(int currentLevel, Sprite activeSprite)
    {
        skillIcon.sprite = activeSprite;
        skillLevel.text = currentLevel.ToString();
    }
}
