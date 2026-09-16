using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public enum SceneNames { Intro = 0, Lobby, Game }
public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

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
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LoadScene(string name)
    {
        int index = Random.Range(0, loadingSprites.Length);
        loadingBackground.sprite = loadingSprites[index];
        loadingProgress.value = 0f;
        loadingScreen.SetActive(true);

        StartCoroutine(LoadSceneAsync(name));
    }

    public void LoadScene(SceneNames name)
    {
        LoadScene(name.ToString());
    }
    
    private IEnumerator LoadSceneAsync(string name)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name);

        // 비동기 작업 (씬 불러오기)을 완료할 때까지 반복
        while(asyncOperation.isDone == false)
        {
            // 비동기 작업 진행 상황(0.0 ~ 1.0)
            loadingProgress.value = asyncOperation.progress;
            textProgress.text = $"{Mathf.RoundToInt(asyncOperation.progress * 100)}%";

            yield return null;
        }

        float changeDelay = 0.5f;
        yield return new WaitForSeconds(changeDelay);

        loadingScreen.SetActive(false);
    }
}
