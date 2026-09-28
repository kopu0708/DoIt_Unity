using UnityEngine;
using System.Collections;
using TMPro;

public class IntroSceneController : MonoBehaviour
{
    [SerializeField]
    private SceneNames nextScene;
    [SerializeField]
    private TextMeshProUGUI textPressAnyKey;

    private void Awake()    
    {
        Application.targetFrameRate = 60;

        Database.Read();  // 인트로 씬이 시작될 떄 매서드를 호출해 저장된 데이터를 불러온다.
    }
    private IEnumerator Start() // 인트로 씬은 타이틀 이미지와 텍스트를 출력하는데 
    {
        while (true)
        {
            yield return StartCoroutine(FadeEffect.Fade(textPressAnyKey, 1, 0)); // Fade() 메서드에 따라 1.0 에서 0.0으로

            yield return StartCoroutine(FadeEffect.Fade(textPressAnyKey, 0, 1)); // 0.0 에서 1.0 으로 같은 속도로 깜빡이게 한다.
        }
    }

    private void Update()
    {
        if (Utils.IsAnyInputDown())
        {
            SceneLoader.Instance.LoadScene(nextScene); // 아무 키가 입력이 감지되면 다음 씬으로 넘어간다.
        }
    }
}
