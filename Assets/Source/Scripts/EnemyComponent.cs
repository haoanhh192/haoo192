using UnityEngine;

[RequireComponent(typeof(OnTriggerEnterComponent))]
public class EnemyComponent : MonoBehaviour
{
    public OnTriggerEnterComponent triggerEnterComponent;

    [SerializeField] private float maxHealth = 10f;

    public float currentHealth;

    private void Awake()
    {
        triggerEnterComponent = GetComponent<OnTriggerEnterComponent>();

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