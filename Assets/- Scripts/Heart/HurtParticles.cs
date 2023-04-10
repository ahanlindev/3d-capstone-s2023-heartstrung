using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Component that plays all particle systems in children of this 
/// gameobject when the supplied Health component takes damage
/// </summary>
public class HurtParticles : MonoBehaviour {

    [Tooltip("The health component to listen to for hurt events")]
    [SerializeField] private Health _health;
    
    private List<ParticleSystem> _particleSystems;

    private void OnEnable() {
        _particleSystems = new List<ParticleSystem>(GetComponentsInChildren<ParticleSystem>());

        if (_health) {
            _health.ChangeHealthEvent += PlayParticles;
        } else {
            Debug.LogError("HurtParticles has no health component set");
        }
    }

    private void PlayParticles(float newHealth, float delta) {
        if (delta < 0) {
            _particleSystems.ForEach((ps) => ps.Play());
        }
    }
}