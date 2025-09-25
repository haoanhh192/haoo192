using D2D.Gameplay;
using UnityEngine;

using static D2D.Utilities.CommonGameplayFacade;

[RequireComponent(typeof(OnTriggerEnterComponent))]
public class EnemyComponent : MonoBehaviour
{
    public OnTriggerEnterComponent triggerEnterComponent;
    public Health health;

    [SerializeField] private float maxHealth = 10f;

    public float currentHealth;

    private void Awake()
    {
        triggerEnterComponent = GetComponent<OnTriggerEnterComponent>();
        health = GetComponent<Health>();

        health.Died += Die;

        currentHealth = maxHealth;
    }

    public void GetHit(float damage)
    {
        health.ApplyDamage(gameObject, damage + (_db.PowerIncreasePercent.Value * damage));
    }
    private void Die()
    {
        

        Destroy(gameObject);
    }

    
}