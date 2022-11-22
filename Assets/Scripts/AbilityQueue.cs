using System;
using System.Collections;
using System.Collections.Generic;
using TTW.Combat;
using UnityEngine;
using System.Linq;

//Dependent on EventBroadcaster
public class AbilityQueue : MonoBehaviour
{
    Queue<Ability> Abilities = new Queue<Ability>();
    [SerializeField] int _abilitiesCount = 0;
    EventBroadcaster broadcaster;

    private void Awake(){
        broadcaster = GetComponent<EventBroadcaster>();
    }

    public int QueueCount => Abilities.Count;

    public void AddAbility(Ability ability){
        broadcaster.ReadyForNewActionChain(false);
        Abilities.Enqueue(ability);
        _abilitiesCount = Abilities.Count;

        if (ReadyToLaunch())
            LaunchAbility();
    }

    public void LaunchAbility(){
        if (Abilities.Count == 0) return;

        var launch = Abilities.Dequeue();
        _abilitiesCount = Abilities.Count;

        var fx = Instantiate(launch.AbilityData.abilityFX);
        if (_abilitiesCount == 0)
            broadcaster.ReadyForNewActionChain(true);
        fx.EndOfFX += OnFXEnd;
        fx.SetAbility(launch);
    }

    private bool ReadyToLaunch(){
        bool ready = false;
        var effects = FindObjectsOfType<AbilityFX>();
        var animationsInScene = effects.Where(a => a.MarkedForDeletion == false).ToList();

        if (animationsInScene.Count == 0){
            ready = true;
        }

        return ready;
    }

    private void OnFXEnd(object sender, EventArgs e)
    {
        LaunchAbility();
    }
}
