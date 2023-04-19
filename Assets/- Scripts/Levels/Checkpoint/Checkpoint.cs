using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject KittyRespawnLocation;
    public GameObject DodgerRespawnLocation;
    public GameObject Flag;
    public Material InactiveMaterial;
    public Material ActiveMaterial;
    private CheckpointManager manager;
    
    public bool activateSilently;
    public bool activated = false;
    public ParticleSystem sys1;
    public ParticleSystem sys2;
    
    void Start() {
        // Find the CheckPointManager in the scene
        CheckpointManager[] managers = FindObjectsOfType(typeof(CheckpointManager)) as CheckpointManager[];

        // Make sure there is only one CheckpointManager in this scene
        if(managers.Length != 1) {
            Debug.LogWarning("There are " + managers.Length + " checkpointManagers in this scene!");
            return;
        }
        manager = managers[0];
        Flag.GetComponent<MeshRenderer>().material = InactiveMaterial;
        sys1.Pause();
        sys2.Pause();
    }

    public void activate() {
        activated = true;
        manager.UpdateCheckpoint(this);
        Flag.GetComponent<MeshRenderer>().material = ActiveMaterial;
        
        if (activateSilently) { return; }
        
        // if able, play fanfare
        sys1.Play();
        sys2.Play();
        AudioManager.instance.PlaySoundEvent("Checkpoint");
    }
}
