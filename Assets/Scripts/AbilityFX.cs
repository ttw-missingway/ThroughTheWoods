using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat
{
    public class AbilityFX : MonoBehaviour
    {
        [SerializeField] float _animationTime = 3f;
        CombatManager _instance;
        ParticleSystem _psSystem;
        Camera _camera;
        List<Targetable> _targets;
        Ability _ability;
        public event EventHandler EndOfFX;
        public bool MarkedForDeletion = false;

        private void Start()
        {
            _instance = CombatManager.Current;
            _psSystem = GetComponentInChildren<ParticleSystem>();
        }
            
        public void SetAbility(Ability ability){
            _ability = ability;
            _targets = ability.CurrentTargets;
            CombatWriter.Singleton.ClearConsole();

            if (ability.AoO){
                CombatWriter.Singleton.Write("An Attack of Opportunity!");
            }
                
            CombatWriter.Singleton.Write("Performing " + _ability.AbilityData.Name);
        }

        public void Particles()
        {
            _psSystem.Play();
        }

        public void SendAbilityData()
        {
            print("         2b." + _ability.Sender.Targetable.Name + " is performing " + _ability.AbilityData.Name + " on " + _targets[0].Name);

            _ability.Sender.OnAbilityCommence(_ability);

            foreach (Targetable t in _targets)
            {
                t.ReceiveAbility(_ability);
            }
        }

        public void End()
        {
            MarkedForDeletion = true;
            EndOfFX?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
        }
    }
}
