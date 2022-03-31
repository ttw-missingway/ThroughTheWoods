using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTW.Systems;
using TTW.UI;
using UnityEngine;

namespace TTW.Combat
{
    public class Targetable : MonoBehaviour
    {
        [SerializeField] private TargetType _targetType;
        TargetType _originalTargetType;
        
        public Health Health { get; set; }
        public Position Position { get; set; }

        public TargetType TargetType => _targetType;

        private void Awake()
        {
            Health = GetComponent<Health>();
            Position = GetComponent<Position>();
            _originalTargetType = _targetType;
        }

        public void ReceiveAbility(Ability ability)
        {
            if (!TargetingConditionsCheck()) return;

            //target manipulation is handled in here
            if (ability.StatusEffect != StatusEffect.None)
                Health.CreateNewStatus(ability.StatusEffect, ability.StatusEffectDuration, canExpire: true);

            if (ability.AbilityType == AbilityType.Attack)
                Health.TakeDamage();

            if (ability.AbilityType == AbilityType.Aid)
            {
                switch (ability.AidType)
                {
                    case AidType.Heal:
                        Health.Heal();
                        break;
                    case AidType.Revive:
                        print("revive");
                        Health.Revive();
                        break;
                    case AidType.Dispel:
                        Health.Dispel();
                        break;
                    default:
                        break;
                }
            }
        }

        internal void ResetTargetType()
        {
            _targetType = _originalTargetType;
        }

        public void SelectTarget()
        {
            if (!TargetingConditionsCheck()) return;

            CombatInstance.Current.SetTarget(this);
            CombatInstance.Current.ExecuteAbility();
        }

        private bool TargetingConditionsCheck()
        {
            if (UISelectionController.Current.SelectionState != SelectionState.Target)
            {
                return false;
            }
            if (!CombatInstance.Current.Ability.TargetTypes.Contains(_targetType))
            {
                print("cannot be targeted by this attack!");
                return false;
            }
            if (CombatInstance.Current.Focus.gameObject == gameObject)
            {
                if (!CombatInstance.Current.Ability.TargetTypes.Contains(TargetType.Self))
                {
                    print("cannot target self!");
                    return false;
                }
            }
            else
            {
                if (Position.DistanceTo(CombatInstance.Current.Focus.GetComponent<Position>()) > CombatInstance.Current.Ability.RangeMax)
                {
                    print("target is too far away!");
                    return false;
                }
                if (Position.DistanceTo(CombatInstance.Current.Focus.GetComponent<Position>()) < CombatInstance.Current.Ability.RangeMin)
                {
                    print("target is too close!");
                    return false;
                }
            }

            return true;
        }

        public void SetTargetType(TargetType targetType)
        {
            _targetType = targetType;
        }
    }
}
