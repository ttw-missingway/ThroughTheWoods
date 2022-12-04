using System.Collections.Generic;
using TTW.Systems;
using UnityEngine;
using System.Linq;

namespace TTW.Combat{
    public class TargetingTool
    {
        List<Targetable> _allTargetables = new List<Targetable>();
        List<Targetable> _targetablesToReturn = new List<Targetable>();
        List<AbilityData> _abilitiesToReturn = new List<AbilityData>();

        public TargetingTool(){
            _allTargetables = GameObject.FindObjectsOfType<Targetable>().ToList();
        }

        public List<Targetable> AllTargetables => _allTargetables;

        public List<Targetable> FilterTargetables(Combatant sender, AbilityData ability, bool writeReason){
            _targetablesToReturn.Clear();

            foreach(Targetable t in _allTargetables){
                if (TargetingConditionsCheck(sender, t, ability, writeReason)){
                    _targetablesToReturn.Add(t);
                }
            }

            return _targetablesToReturn;
        }

        public List<AbilityData> FilterAbilities(Combatant sender, List<AbilityData> abilities){
            _abilitiesToReturn.Clear();

            foreach(AbilityData a in abilities){
                foreach (Targetable t in _allTargetables){
                    if (TargetingConditionsCheck(sender, t, a, writeReason: false)){
                        if (!_abilitiesToReturn.Contains(a)){
                            _abilitiesToReturn.Add(a);
                            break;
                        }
                    }
                }
            }
                
            return _abilitiesToReturn;
        }

        public bool TargetingConditionsCheck(Combatant sender, Targetable target, AbilityData ability, bool writeReason)
        {
            if (!TargetMatchCheck(sender, target, ability))
            {
                if (writeReason)
                    CombatWriter.Singleton.Write("ability could not target anything!");

                return false;
            }


            if (!RangeCheck(ability, target))
            {
                if (writeReason)
                    CombatWriter.Singleton.Write(target.Name + " is too far away!");

                return false;
            }
            
            return true;
        }

        private bool TargetMatchCheck(Combatant sender, Targetable target, AbilityData ability)
        {
            var abilityTypes = ability.TargetTypes;

            bool match = true;
            if (abilityTypes.Contains(TargetingClass.Down) || abilityTypes.Contains(TargetingClass.Self))
            {
                foreach (TargetingClass t in abilityTypes)
                {
                    if (!ValidTargetType(t, sender, target))
                    {
                        match = false;
                        break;
                    }
                }
            }
            else
            {
                match = false;
                foreach (TargetingClass t in abilityTypes)
                {
                    if (ValidTargetType(t, sender, target))
                    {
                        match = true;
                        break;
                    }
                }
            }

            return match;
        }

        private bool RangeCheck(AbilityData ability, Targetable target)
        {
            var distance = target.Position.Distance;
            var success = true;

            if (ability.RangeType == RangeType.Melee && distance == CombatDistance.Back){
                success = false;
            }

            return success;
        }

        private bool ValidTargetType(TargetingClass targetingClass, Combatant sender, Targetable receiver){
            bool value = false;
            var senderSide = sender.Position.CombatSide;
            var receiverSide = receiver.Position.CombatSide;

            bool isDown = receiver.Health.StatusExists(StatusEffect.Down);
            bool isObstacle = receiver.TargetType == TargetType.Obstacle;
            bool isSelf = sender.Targetable == receiver;


            switch (targetingClass){
                case TargetingClass.Foe:
                    if (senderSide == CombatSide.Ally && receiverSide == CombatSide.Enemy) value = true;
                    if (senderSide == CombatSide.Enemy && receiverSide == CombatSide.Ally) value = true;
                    if (isDown) value = false;
                    break;
                case TargetingClass.Ally:
                    if (senderSide == CombatSide.Ally && receiverSide == CombatSide.Ally) value = true;
                    if (senderSide == CombatSide.Enemy && receiverSide == CombatSide.Enemy) value = true;
                    if (isSelf) value = false;
                    if (isDown) value = false;
                    break;
                case TargetingClass.Down:
                    if (isDown) value = true;
                    break;
                case TargetingClass.Obstacle:
                    if (isObstacle) value = true;
                    break;
                case TargetingClass.Self:
                    if (isSelf) value = true;
                    break;
                default:
                    value = false;
                    break;
            }

            return value;
        }
    }
}