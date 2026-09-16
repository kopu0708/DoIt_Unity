using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField]
    protected GameObject projectilePrefab;
    [SerializeField]
    protected Transform projectileSpawnPoint;

    private float currentCooldownTime = 0f;
    private float maxCooldownTime = 0f;
    private bool isSkillAvailable = true;
    protected float damage;
    protected EntityBase owner;

    public void Setup(EntityBase owner)
    {
        this.owner = owner;
        damage = owner.Stats.GetStat(StatType.Damage).Value;
        maxCooldownTime = owner.Stats.GetStat(StatType.CooldownTime).Value;
    }

    private void Update()
    {
        if( isSkillAvailable == false && Time.time - currentCooldownTime >
            maxCooldownTime)
        {
            isSkillAvailable = true;
        }
    }

    public void TryAttack()
    {
        if(isSkillAvailable == true)
        {
            OnAttack();
            isSkillAvailable = false;
            currentCooldownTime = Time.time;
        }
    }

    public abstract void OnAttack();
}
