using UnityEngine;

public class DummyWithHealth : MonoBehaviour
{
    private Health _health;

    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        _health.ChangeHealthEvent += OnChangeHealth;
    }

    private void OnDisable()
    {
        _health.ChangeHealthEvent -= OnChangeHealth;
    }

    private void OnChangeHealth(float newTotal, float delta)
    {
        if (newTotal <= 0)
        {
            Die();
        }
        else
        {
            TakeDamage();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void TakeDamage()
    {
        // play hurt animation or whatever else needs to happen
    }
}