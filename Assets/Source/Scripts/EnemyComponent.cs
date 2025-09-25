using D2D.Gameplay;
using UnityEngine;

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

        currentHealth = maxHealth;
    }

    public void GetHit(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {

        }
    }
    private void Die()
    {
        

        Destroy(gameObject);
    }

    
}