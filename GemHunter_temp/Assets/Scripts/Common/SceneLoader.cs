using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public enum SceneNames { Intro = 0, Lobby, Game }
public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; } // 싱글톤 패턴을 위해 스태틱으로 구현 
                                                             // "이 클래스의 인스턴스는 게임 전체에서 딱 하나만 존재해야 한다"를 보장하기 위해서 
                                                             // 어디서든 별도의 객체 생성없이 접근이 가능하게 만들어준다.

    [SerializeField]
    private GameObject loadingScreen; // 로딩 화면 
    [SerializeField]
    private Image loadingBackground; // 로딩 화면에 출력할 배경 이미지 
    [SerializeField]
    private Sprite[] loadingSprites; // 배경 이미지 목록
    [SerializeField]
    private Slider loadingProgress; // 로딩 진행도
    [SerializeField]
    private TextMeshProUGUI textProgress; // 로딩 진행도 텍스트

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject); // 혹시라도 실수로 여러개 씬에서 SceneLoader가 중복되면 이미 살아있는 쪽을 살리고 새로 생성된 것을 파괴해서 하나만 존재하도록 강제하기위한 코드 
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LoadScene(string name) // 이건 그냥 로딩할때 어떤 이미지를 불러올지 무작위로 정해주는 것 
    {
        int index = Random.Range(0, loadingSprites.Length); 
        loadingBackground.sprite = loadingSprites[index]; // 미리 지정해둔 스프라이트를 배열로 저장해둔거 꺼내기 
        loadingProgress.value = 0f;
        loadingScreen.SetActive(true); 

        StartCoroutine(LoadSceneAsync(name)); // 코루틴 호출 
    }

    public void LoadScene(SceneNames name)
    {
        LoadScene(name.ToString());
    }
    
    private IEnumerator LoadSceneAsync(string name)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name); 

        // 비동기 작업 (씬 불러오기)을 완료할 때까지 반복
        while(asyncOperation.isDone == false) // isDone은 비동기 작업이 완료인지, 진행중 인지 반환한다. (false는 작업 진행중, true 작업 완료)
        {
            // 비동기 작업 진행 상황(0.0 ~ 1.0)
            //loadingProgress.value = asyncOperation.progress; // 비동기 작업 진행 상황이다. 0.0f ~ 1.0f 사이 값으로, 작업이 끝나면 1,0을 반환한다.
                                                             // 0.0~1.0 값이지만, 실제로는 0.9에서 멈췄다가
                                                             // 로드 완료 시점에 한 번에 1.0이 되는 특성이 있음 
            float normalizedProgress = Mathf.Clamp01(asyncOperation.progress / 0.9f); // 그래서 0.9로 나눠서 0~1 범위로 재조정(정규화) 해준다.
            loadingProgress.value = normalizedProgress;
            textProgress.text = $"{Mathf.RoundToInt(normalizedProgress * 100)}%"; // 작업상황을 백분률로 변환해서 텍스트로 출력 

            yield return null; // 여기서 한 프레임 양보(정지)하고, 다음 프레임에 이 지점부터 다시 이어서 실행됨
                               // 이게 없으면 while문이 한 프레임 안에서 무한 반복되어 게임이 멈춤(시간이 흐를 기회 자체가 없어짐)
        }

        float changeDelay = 0.5f;
        yield return new WaitForSeconds(changeDelay);

        loadingScreen.SetActive(false);
    }
}
