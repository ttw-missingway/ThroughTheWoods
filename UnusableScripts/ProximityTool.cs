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
        if (!ChainConditions(posA, initialPosition: true)) return;

        List<Position> AllPositions = MonoBehaviour.FindObjectsOfType<Position>()
                                        .Where(p => p.CombatSide == posA.CombatSide)
                                        .OrderBy(p => p.OrderNo)
                                        .ToList();

        var index = posA.OrderNo - 1;

        for (var i = index+1; i < AllPositions.Count; i++){
            
            if (!ChainConditions(AllPositions[i], initialPosition: false)){
                break;
            }

            FoundLinks.Add(AllPositions[i]);
        }

        for (var i = index-1; i > -1; i--){
            if (!ChainConditions(AllPositions[i], initialPosition: false)){
                break;
            }

            FoundLinks.Add(AllPositions[i]);
        }
    }

    public bool IsChained(Position posB){
        if (FoundLinks.Contains(posB)) return true;

        return false;
    }

    private bool ChainConditions(Position p, bool initialPosition){
        return p.Health.PassesChainConditions(initialPosition);
    }
}
