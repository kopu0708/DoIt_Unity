using UnityEngine;
using UnityEngine.UI;
public class UIHP : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private EntityBase entity;

    private void Awake()  //플레이어의 체력 UI는 게임을 시작하기 전에 미리 생성했으므로 Awake() 메서드에서 처리
    {
        if (entity != null) 
            entity.Stats.CurrentHP.OnValueChanged += UpdateHP; // 현재 체력값에 변화가 있을 때마다 UpdateHP() 메서드를 호출 하도록 등록
                                                               
    }
    public void Setup(EntityBase entity) // 적 체력은 적이 생길 때 생성하므로 Setup()
    {
        this.entity = entity;  
        this.entity.Stats.CurrentHP.OnValueChanged += UpdateHP; 
    }

    private void UpdateHP(Stat stat, float prev, float current)
    {
        image.fillAmount = entity.Stats.CurrentHP.Value /
            entity.Stats.GetStat(StatType.HP).Value;
    }
}
