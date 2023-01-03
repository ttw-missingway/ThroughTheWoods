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

        bool _abilitySet = false;

        private void Start()
        {
            _instance = CombatManager.Current;
            _psSystem = GetComponentInChildren<ParticleSystem>();
        }
            
        public void SetAbility(Ability ability){
            _abilitySet = true;
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
            if (!_abilitySet){
                print("ERROR: ABILITY NOT SET FOR FX");
                return;
            }

            _instance.EventBroadcaster.PrintDescriptor("         2b." + _ability.Sender.Targetable.Name + " is performing " + _ability.AbilityData.Name + " on " + _targets[0].Name);

            _ability.Sender.OnAbilityCommence(_ability);

            foreach (Targetable t in _targets)
            {
                t.ReceiveAbility(_ability);
            }
        }

        public void OpenAbilityIcons(){
            if (!_abilitySet){
                print("ERROR: ABILITY NOT SET FOR FX");
                return;
            }

            _targets[0].OpenTargetIcon();
            _ability.Sender.OpenAttackIcon();
        }

        public void CloseAbilityIcons(){
            if (!_abilitySet){
                print("ERROR: ABILITY NOT SET FOR FX");
                return;
            }

            _targets[0].CloseTargetIcon();
            _ability.Sender.CloseAttackIcon();
        }

        public void End()
        {
            MarkedForDeletion = true;
            EndOfFX?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
        }
    }
}
