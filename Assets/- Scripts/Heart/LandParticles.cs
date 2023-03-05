using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandParticles : MonoBehaviour
{
    [Tooltip("The heart component to listen to for landed events")]
    [SerializeField] private HeartStateMachine _heart;
    
    private List<ParticleSystem> _particleSystems;

    private void OnEnable() {
        _particleSystems = new List<ParticleSystem>(GetComponentsInChildren<ParticleSystem>());

        if (_heart) {
            _heart.LandedEvent += PlayParticles;
        } else {
            Debug.LogError("LandedParticles has no heart component set");
        }
    }

    private void PlayParticles() {
        _particleSystems.ForEach((ps) => ps.Play());
    }
}
