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

    private void Start(){
        // broadcaster.EndAction += OnEndAction;
    }

    // private void OnEndAction(object sender, EventArgs e)
    // {
    //     LaunchAbility();
    // }

    public int QueueCount => Abilities.Count;

    public void AddAbility(Ability ability){
        Abilities.Enqueue(ability);
        _abilitiesCount = Abilities.Count;

        if (ReadyToLaunch()) 
            LaunchAbility();
    }

    public void LaunchAbility(){
        if (Abilities.Count == 0){
            EndOfAction();
            return;
        }

        var launch = Abilities.Dequeue();
        _abilitiesCount = Abilities.Count;

        var fx = Instantiate(launch.AbilityData.abilityFX);
        fx.EndOfFX += OnFXEnd;
        fx.SetAbility(launch);
        broadcaster.CallStartAction();
    }

    private bool ReadyToLaunch(){
        bool ready = false;
        var effects = FindObjectsOfType<AbilityFX>();
        var animationsInScene = effects.Where(a => a.MarkedForDeletion == false).ToList();

        if (animationsInScene.Count == 0) 
            ready = true;

        return ready;
    }

    private void OnFXEnd(object sender, EventArgs e)
    {
        LaunchAbility();
    }

    private void EndOfAction(){
        broadcaster.CallEndOfAction();
    }
}
