using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat{
    public class AoO
    {
        public void ReceiveAoORequest(AoORequest request, Targetable requested){
            if (request.AlreadyRequested(requested)) return;

            var position = requested.Position;
            ProximityTool pTool = new ProximityTool(request.Requestee.Position);

            if (!pTool.IsChained(requested.Position)) return;

            request.AddRequested(requested);
            
            float random;
            float baseChance = 10f;

            if (request.Alert){
                random = -1f;
            }
            else{
                random = UnityEngine.Random.Range(0f, 100f);
            }

            if (random < baseChance){
                ExecuteAoO(request.Target, requested.GetComponent<Combatant>());
            }

            AoORequest(request, position);
        }
        private void ExecuteAoO(Targetable target, Combatant combatant){
            var aooData = CombatManager.Current.AttackOfOpportunity;
            Ability aoo = new Ability(aooData, combatant);
            aoo.AttackOfOpportunity();
            aoo.CurrentTargets.Add(target);

            combatant.SendAbility(aoo);
        }
        public void AoORequest(AoORequest request, Position position)
        {
            foreach (Position p in position.Neighbors){
                if (p == null) continue;

                if (p.GetComponent<Combatant>() != null){
                    var combatant = p.GetComponent<Combatant>();
                    combatant.AoO.ReceiveAoORequest(request, combatant.Targetable);
                }
            }
        }
    }
}