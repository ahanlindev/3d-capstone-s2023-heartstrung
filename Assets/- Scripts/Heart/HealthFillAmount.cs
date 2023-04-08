using UnityEngine;

public class HealthFillAmount : MonoBehaviour
{
    // Drag health component from Dodger here in inspector
    [SerializeField] private Health _health;
    private Renderer _renderer;
    private static readonly int HealthFill = Shader.PropertyToID("_health_fill");

    private void Awake() {
        _renderer = GetComponent<Renderer>();
        if (!_health) { Debug.Log("HealthFillAmount does not have a health component set!"); }
    }

    private void Start() {
        ChangeHealthLevel(_health.currentHealth, 0);
    }

    private void OnEnable() {
        _health.ChangeHealthEvent += ChangeHealthLevel;
    }

    private void OnDisable() {
        _health.ChangeHealthEvent -= ChangeHealthLevel;
    }

    private void ChangeHealthLevel(float newLevel, float change) {
        // whatever shader stuff needs to happen

        float percentage = newLevel / _health.maxHealth;
        _renderer.material.SetFloat(HealthFill, percentage);
    }
}
