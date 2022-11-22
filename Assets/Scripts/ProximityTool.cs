using System.Collections;
using System.Collections.Generic;
using TTW.Combat;
using UnityEngine;
using System.Linq;
using TTW.Systems;

public class ProximityTool
{
    List<Position> FoundLinks = new List<Position>();
    Position _posA;

    public ProximityTool(Position posA){
        _posA = posA;
        BuildProximityChain(posA);
    }

    private void BuildProximityChain(Position posA){
        if (!ChainConditions(posA)) return;

        List<Position> AllPositions = MonoBehaviour.FindObjectsOfType<Position>()
                                        .Where(p => p.PlayingFieldSide == posA.PlayingFieldSide)
                                        .OrderBy(p => p.OrderNo)
                                        .ToList();

        var index = posA.OrderNo;

        for (var i = index; i < AllPositions.Count; i++){
            
            if (!ChainConditions(AllPositions[i])){
                break;
            }

            FoundLinks.Add(AllPositions[i]);
        }

        for (var i = index-1; i > 0; i--){
            if (!ChainConditions(AllPositions[i])){
                break;
            }

            FoundLinks.Add(AllPositions[i]);
        }
    }

    public bool IsChained(Position posB){
        if (FoundLinks.Contains(posB)) return true;

        return false;
    }

    private bool ChainConditions(Position p){
        if (p.GetComponent<Targetable>().TargetType == TargetType.Obstacle){
            return false;
        } 
        if (p.Distance != CombatDistance.Front){
            return false;
        }
        if (p.GetComponent<Health>().StatusExists(StatusEffect.Isolated)){
            return false;
        }
        if (p.GetComponent<Health>().StatusExists(StatusEffect.Trance)){
            return false;
        }
        if (p.GetComponent<Health>().StatusExists(StatusEffect.Down)){
            return false;
        }
        if (p.GetComponent<Combatant>().Exhausted){
            return false;
        }  
        if (p.GetComponent<Combatant>().Channeling){
            return false;
        }  

        return true;
    }
}
