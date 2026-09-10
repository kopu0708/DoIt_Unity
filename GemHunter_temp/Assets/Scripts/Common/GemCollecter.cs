using UnityEngine;
using UnityEngine.Events;
public class GemCollecter : MonoBehaviour
{
    [SerializeField]
    private GameObject gemEffectPrefab;
    [SerializeField]
    private RectTransform uiElement; // 보석이 이동할 목표 위치 (Gem Icon UI)
    [SerializeField]
    private UnityEvent onGemCollectEvent; // 보석을 얻을 때 호출할 메서드 등록 
    private MemoryPool memoryPool; // 보석을 생성하고 관리할 메모리 풀

    private void Awake()
    {
        memoryPool = new MemoryPool(gemEffectPrefab);
    }

    public void SpawnGemEffect(Vector2 point, int count = 5)
    {
        for(int i = 0; i < count; ++i)
        {
            GameObject gem = memoryPool.ActivatePoolItem(point);
            gem.GetComponent<GemCollectEffect>().Setup(this, uiElement);
        }
    }

    public void OnGemCollect(GameObject gem)
    {
        onGemCollectEvent?.Invoke(); // 보석을 얻을 때 이벤트에 등록된 메소드를 호출한다.
        memoryPool.DeactivatePoolItem(gem); // 매개변수로 받은 gem 오브젝트 비활성화 
    }
}
