using UnityEngine;
using UnityEngine.UI;
public class UIHP : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private EntityBase entity;

    public void Setup(EntityBase entity)
    {
        this.entity = entity;
    }

    private void Update()
    {
        image.fillAmount = entity.Stats.currentHp / entity.Stats.MaxHp;
    }
}
