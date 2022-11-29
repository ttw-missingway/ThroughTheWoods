using System;
using System.Collections.Generic;
using System.Linq;
using TTW.Systems;
using UnityEngine;

namespace TTW.Combat
{
    public class CombatManager : MonoBehaviour
    {
        //SINGLETON
        public static CombatManager Current;

        //FIELDS
        [SerializeField] int _turnsCount = 0;
        [SerializeField] CombatSide _turn = CombatSide.Ally;
        [SerializeField] CombatState _combatState = CombatState.Event;
        [SerializeField] CombatWriter _combatWriter;
        [SerializeField] public AbilityData AttackOfOpportunity;
        [SerializeField] public AbilityData CounterAttack;

        //Components
        LinkLibrary _linkLibrary;
        AbilityQueue _abilityQueue;
        EventBroadcaster _eventBroadcaster;
        
        //API
        public List<Combatant> Allies;
        public List<Combatant> Enemies;
        public List<Targetable> Objects;
        public List<Targetable> Targetables;
        public List<Position> AllySidePositions = new List<Position>();
        public List<Position> EnemySidePositions = new List<Position>();
        public CombatState State => _combatState;
        public CombatSide Turn => _turn;
        public EventBroadcaster EventBroadcaster => _eventBroadcaster;
        public AbilityQueue AbilityQueue => _abilityQueue;
        public LinkLibrary LinkLibrary => _linkLibrary;
        public List<Combatant> AvailableActors => _cTool.AvailableActors;
        public List<Combatant> AvailableEnemies => _cTool.AvailableEnemies;

        //Tools
        CheckingTool _cTool;

        private void Awake()
        {
            Current = this;
            _linkLibrary = GetComponent<LinkLibrary>();
            _abilityQueue = GetComponent<AbilityQueue>();
            _eventBroadcaster = GetComponent<EventBroadcaster>();
            _eventBroadcaster.StartOfAction += OnStartOfAction;
            _eventBroadcaster.EndOfAction += OnEndOfAction;
            _eventBroadcaster.EndOfEventPhase += OnEndOfEventPhase;
            _cTool = new CheckingTool(); 
        }

        private void Start(){
            EstablishPositions();
            _cTool.GetAvailableAllies(Allies);
            _cTool.GetAvailableEnemies(Enemies);
            BeginCombat();
        }

        private void OnStartOfAction(object sender, EventArgs e)
        {
            _combatState = CombatState.Animation;
        }

        private void OnEndOfEventPhase(object sender, EventArgs e)
        {
            if (Turn == CombatSide.Ally) 
                _eventBroadcaster.CallStartOfAlliesTurn();
            else
                _eventBroadcaster.CallStartOfEnemiesTurn();
        }

        private void OnEndOfAction(object sender, EventArgs e)
        {
            CheckForTurnOver();
        }

        public void CheckForTurnOver()
        {
            _combatState = CombatState.Control;

            if (CheckNoCombatantsRemaining()){
                PromptEndOfTurn();
            }
            else{
                PromptAction();
            }
        }

        private void PromptEndOfTurn()
        {
            _eventBroadcaster.CallEndTurnPrompt();
        }

        private void PromptAction(){
            _eventBroadcaster.CallActionPrompt();
        }

        public void BeginCombat(){
            _eventBroadcaster.StartEventPhase();
        }

        public void AddActor(Combatant actor){
            Allies.Add(actor);
        }

        public void AddEnemy(Combatant enemy){
            Enemies.Add(enemy);
        }

        public void AddObject(Targetable o){
            Objects.Add(o);
        }

        public bool CheckNoCombatantsRemaining(){
            bool results = false;

            if (_turn == CombatSide.Ally)
                results = _cTool.CheckTurnOver(_turn, Allies);
            else   
                results = _cTool.CheckTurnOver(_turn, Enemies);

            return results;
        }

        public void EndTurn(){
            if (_turn == CombatSide.Ally){
                _turn = CombatSide.Enemy;
                _eventBroadcaster.CallEndOfAlliesTurn();
            }
            else if (_turn == CombatSide.Enemy){
                _turn = CombatSide.Ally;
                _eventBroadcaster.CallEndOfEnemiesTurn();
            }
            ChangeState(CombatState.Event);
            
            _eventBroadcaster.StartEventPhase();
        }

        public void ChangeState(CombatState state){
            _combatState = state;
        }

        private void EstablishPositions()
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

        public void SwitchPositions(List<Position> list, int posA, int posB){
            Position tmp = list[posA];
            list[posA] = list[posB];
            list[posB] = tmp;
            list[posA].SetPositionOrder(list, posA);
            list[posB].SetPositionOrder(list, posB);
        }

        public void ReorderPositions(List<Position> list, int oldIndex, int newIndex){
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