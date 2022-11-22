using System.Collections;
using System.Collections.Generic;
using TTW.Systems;
using UnityEngine;

namespace TTW.Combat{
    public class CheckingTool
    {
        public bool IsAvailable(Combatant c, bool writeReason){
            return c.Health.PassesActionConditions(writeReason);
        }

        public bool CheckTurnOver(CombatSide turn, List<Combatant> combatants)
        {
            switch (turn)
            {
                default:
                case CombatSide.Ally:
                    foreach (var a in combatants)
                    {
                        if (IsAvailable(a, false))
                            return false;
                    }
                    return true;
                case CombatSide.Enemy:
                    foreach (var a in combatants)
                    {
                        if (IsAvailable(a, false))
                            return false;
                    }
                    return true;    
            }
        }

        public List<Combatant> GetAvailableActors(List<Combatant> actors){
            var availableActors = new List<Combatant>();
            foreach(Combatant a in actors){
                if (IsAvailable(a, false))
                    availableActors.Add(a);
            }

            return availableActors;
        }

        public List<Combatant> GetAvailableEnemies(List<Combatant> enemies){
            var availableEnemies = new List<Combatant>();
            foreach(Combatant e in enemies){
                if (IsAvailable(e, false))
                    availableEnemies.Add(e);
            }

            return availableEnemies;
        }

        public bool CheckVictory(CombatSide turn, List<Combatant> actors, List<Combatant> enemies)
        {
            if (turn == CombatSide.Ally){
                foreach(Combatant e in enemies){
                    if (!e.GetComponent<Health>().StatusExists(StatusEffect.Down)){
                        return false;
                    }
                }
                CombatWriter.Singleton.ClearConsole();
                CombatWriter.Singleton.Write("YOU WIN!");
                return true;
            }
            if (turn == CombatSide.Enemy){
                foreach(Combatant a in actors){
                    if (!a.GetComponent<Health>().StatusExists(StatusEffect.Down)){
                        return false;
                    }
                }
                CombatWriter.Singleton.ClearConsole();
                CombatWriter.Singleton.Write("GAME OVER...");
                return true;
            }

            return false;
        }
    }
}