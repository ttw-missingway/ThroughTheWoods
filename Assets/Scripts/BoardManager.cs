using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TTW.Systems;
using UnityEngine;

namespace TTW.Combat{
    public class BoardManager : MonoBehaviour
    {
        public List<Combatant> Allies;
        public List<Combatant> Enemies;
        public List<Targetable> Traps;
        public List<Position> AllySidePositions = new();
        public List<Position> EnemySidePositions = new();
        CheckingTool _cTool;
        TTWMath _math;
            
        public List<Combatant> GetAvailableAllies(){
            return _cTool.GetAvailableAllies(Allies);
        }

        public List<Combatant> AvailableEnemies(){
            return _cTool.AvailableEnemies;
        }

        private void Start(){
            _cTool = new CheckingTool(); 
            _math = new TTWMath();
            _cTool.GetAvailableAllies(Allies);
            _cTool.GetAvailableEnemies(Enemies);
        } 

        public void EstablishPositions()
        {
            var positions = FindObjectsOfType<Position>().ToList();

            foreach (var p in positions)
            {
                if (p.CombatSide == CombatSide.Ally){
                    AllySidePositions.Add(p);
                }
                else if (p.CombatSide == CombatSide.Enemy){
                    EnemySidePositions.Add(p);
                }
            }

            for (int i = 0; i < AllySidePositions.Count; i++){
                AllySidePositions[i].SetPositionOrder(AllySidePositions, i);
            }

            for (int i = 0; i < EnemySidePositions.Count; i++){
                EnemySidePositions[i].SetPositionOrder(EnemySidePositions, i);
            }
        }


        public void AddActor(Combatant actor){
            Allies.Add(actor);
        }

        public void AddEnemy(Combatant enemy){
            Enemies.Add(enemy);
        }

        public void AddObject(Targetable o){
            Traps.Add(o);
        }

        public bool CheckNoCombatantsRemaining(CombatSide turn){
            bool results;

            if (turn == CombatSide.Ally)
                results = _cTool.CheckTurnOver(turn, Allies);
            else   
                results = _cTool.CheckTurnOver(turn, Enemies);

            return results;
        }



        public void SwapPositions(CombatSide side, int posA, int posB){
            if (side == CombatSide.Ally){
                _math.SwitchPositions(AllySidePositions, posA, posB);
                AllySidePositions[posA].SetPositionOrder(AllySidePositions, posA);
                AllySidePositions[posB].SetPositionOrder(AllySidePositions, posB);
                (AllySidePositions[posA].transform.position, AllySidePositions[posB].transform.position) = (AllySidePositions[posB].transform.position, AllySidePositions[posA].transform.position);
            }       
            else if (side == CombatSide.Enemy){
                _math.SwitchPositions(EnemySidePositions, posA, posB);
                EnemySidePositions[posA].SetPositionOrder(EnemySidePositions, posA);
                EnemySidePositions[posB].SetPositionOrder(EnemySidePositions, posB);
                (EnemySidePositions[posA].transform.position, EnemySidePositions[posB].transform.position) = (EnemySidePositions[posB].transform.position, EnemySidePositions[posA].transform.position);
            } 
        }

        private void ReorderPositions(List<Position> list, int oldIndex, int newIndex){
            Position item = list[oldIndex];
            list.RemoveAt(oldIndex);
            list.Insert(newIndex, item);

            for (var i = 0; i < list.Count(); i++){
                list[i].SetPositionOrder(list, i+1);
            }
        }

        public void DestroyAtPosition(List<Position> list, int index){
            list.RemoveAt(index);

            for (var i = 0; i < list.Count(); i++){
                list[i].SetPositionOrder(list, i+1);
            }
        }
    }
}
