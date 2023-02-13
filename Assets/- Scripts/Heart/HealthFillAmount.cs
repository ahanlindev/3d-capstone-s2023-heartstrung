using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthFillAmount : MonoBehaviour
{
    // Drag health component from Dodger here in inspector
    [SerializeField] private Health _health;
    private Renderer rend;
    
    private void Awake() {
        rend = GetComponent<Renderer>();
        if (!_health) { Debug.Log("HealthFillAmount does not have a health component set!"); }
    }

    private void Start() {
        ChangeHealthLevel(_health.CurrentHealth, 0);
    }

    private void OnEnable() {
        _health.ChangeHealthEvent += ChangeHealthLevel;
    }

    private void OnDisable() {
        _health.ChangeHealthEvent -= ChangeHealthLevel;
    }

    private void ChangeHealthLevel(float newLevel, float change) {
        // whatever shader stuff needs to happen

        float percentage = newLevel / _health.MaxHealth;
        rend.material.SetFloat("_health_fill", percentage);
    }
}
