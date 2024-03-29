﻿using System;
using System.Collections.Generic;
using TTW.Systems;
using UnityEngine;

namespace TTW.Combat
{
    public class Targetable : MonoBehaviour
    {
        [SerializeField] public string Name;
        [SerializeField] TargetType _targetType;
        [SerializeField] string _keyword;
        [SerializeField] GameObject _targetIcon;
        
        public Health Health { get; set; }
        public Position Position { get; set; }
        public TargetType TargetType => _targetType;
        public string Keyword => _keyword;
        TargetingTool tTool;

        private void Awake()
        {
            Health = GetComponent<Health>();
            Position = GetComponent<Position>();
            tTool = new TargetingTool();
        }

        public void OpenTargetIcon(){
            _targetIcon.SetActive(true);
        }

        public void CloseTargetIcon(){
            _targetIcon.SetActive(false);
        }

        public void ReceiveAbility(Ability ability)
        {
            if (ability.AffectedTargets.Contains(this)) return;
            if (!tTool.TargetingConditionsCheck(ability.Sender, this, ability.AbilityData, true)) return;

            CombatManager.Current.EventBroadcaster.PrintDescriptor("         2b. " + Name + " has received ability " + ability.AbilityData.Name);

            if (!AccuracyCheck(ability)){
                // print("7. The ability missed!");
                return;
            }

            if (CheckAOOConditions(ability))
                RequestAttackOfOpportunity(ability.Sender.Targetable);

            //Broadcast AOO Trigger;

            if (Health.Stance == Stance.Countering)
                CounterAttack(ability.Sender);

            //target manipulation is handled in here
            if (ability.StatusEffect != StatusEffect.None)
                Health.CreateNewStatus(ability.StatusEffect, ability.StatusEffectDuration, canExpire: true);

            if (ability.Damaging){
                if (Health.Stance == Stance.Guarding && ability.MagicType == MagicType.None){
                    // print("7. ability blocked by guarding!");
                }
                else{
                    Health.TakeDamage();
                    // print("7. " + Name + " took damage!");
                }
                Health.BreakStance();
            }   

            if (ability.AttackVariant == AttackVariants.Deadly){
                Health.Death();
            }

            if (ability.AbilityType == AbilityType.Aid)
            {
                switch (ability.AidType)
                {
                    case AidType.Heal:
                        Health.Heal();
                        break;
                    case AidType.Revive:
                        Health.Revive();
                        break;
                    case AidType.Dispel:
                        Health.Dispel();
                        break;
                    default:
                        break;
                }
            }

            ability.AddAffectedTarget(this);

            if (ability.TargetingMode == TargetScope.Area){
                Position.AreaAttackToNeighbors(ability);
            }
        }

        private void CounterAttack(Combatant sender)
        {
            Targetable target = sender.Targetable;

            if (GetComponent<Combatant>() != null)
                GetComponent<Combatant>().CounterAttack(target);
        }

        private bool CheckAOOConditions(Ability ability){
            bool value = true;
            CombatDistance attackerPosition = ability.Sender.Position.Distance;

            if (!Health.PassesChainConditions(initialTarget: true))         value = false;
            if (attackerPosition == CombatDistance.Back)                    value = false;
            if (ability.AoO)                                                value = false;
            if (!ability.TargetTypes.Contains(TargetingClass.Foe))          value = false;

            return value;
        }

        private void RequestAttackOfOpportunity(Targetable target)
        {
            var aooRequest = new AoORequest(this, target);
            
            if (Health.Stance == Stance.Alert)
                aooRequest.SetAlert();

            foreach (Position p in Position.Neighbors){

                if (p == null) continue;

                if (p.GetComponent<Combatant>() != null && p.GetComponent<Targetable>() != null){
                    p.GetComponent<Combatant>().AoO.ReceiveAoORequest(aooRequest, p.GetComponent<Targetable>());
                }
            }
        }

        public void SetKeyword(string keyword){
            _keyword = keyword;
        }

        private bool AccuracyCheck(Ability ability)
        {
            var success = true;

            if (Health.Stance == Stance.Alert && !ability.NoMiss){
                if (DodgeRoll()){
                    CombatWriter.Singleton.Write("Damage Dodged!");
                    return false;
                }
            }

            if (ability.Range == RangeType.Ranged && !ability.NoMiss){
                float random = UnityEngine.Random.Range(0f, 100f);
                print("accuracy roll: " + random);
                if (random > ability.Accuracy){
                    success = false;
                    CombatWriter.Singleton.Write("Attack Missed!");
                }
            }

            return success;
        }

        private bool DodgeRoll()
        {
            var baseChance = 40f;
            var random = UnityEngine.Random.Range(0f, 100f);

            if (random < baseChance){
                return true;
            }

            return false;
        }
    }
}
