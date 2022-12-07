using System;
using TTW.Systems;
using UnityEngine;

namespace TTW.Combat{
    public class ActionProcessor : MonoBehaviour
    {
        private Combatant _combatant;
        private Targetable _targetable;
        private Position _position;
        private Health _health;

        private void Awake(){
            _combatant = GetComponent<Combatant>();
            _targetable = GetComponent<Targetable>();
            _position = GetComponent<Position>();
            _health = GetComponent<Health>();
        }

        public void ReceiveAbility(Ability ability)
        {
            //Apply Channel
            if (ability.ChannelTime > 0){
                _combatant.Channel.StartChannel(ability.ChannelTime, ability);
                return;
            }

            CheckStance(ability);
            CheckPositioning(ability);

            _combatant.SendAbility(ability);
        }

        private void CheckPositioning(Ability ability)
        {
            if (ability.Movement == Movement.None){
                return;
            }
            else if (ability.Movement == Movement.Advance){
                _position.Advance();
            }
            else if (ability.Movement == Movement.Retreat){
                _position.Retreat();
            }
            else if (ability.Movement == Movement.Reposition){
                _position.Reposition(ability.CurrentTargets[0].Position);
            }
            else if (ability.Movement == Movement.Swap){
                _position.Swap(ability.CurrentTargets[0].Position);
            }
        }

        private void CheckStance(Ability ability)
        {
            if (ability.Stance == Stance.Wait)
            {
                if (ability.Stance == Stance.None)
                    _health.ChangeStance(Stance.Alert);
            }
            else
            {
                _health.BreakStance();
                if (ability.ChangeStance)
                {
                    _health.ChangeStance(ability.Stance);
                }
            }
        }
    }
}
