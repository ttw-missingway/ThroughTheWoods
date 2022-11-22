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
        // [SerializeField] Combatant _focus;
        // [SerializeField] AbilityData _ability;
        // [SerializeField] Targetable _target;
        // [SerializeField] List<Targetable> _possibleTargets = new List<Targetable>();
        // [SerializeField] List<Combatant> _availableActors = new List<Combatant>();
        [SerializeField] CombatSide _turn = CombatSide.Ally;
        // [SerializeField] int _queueCount;
        [SerializeField] CombatState _combatState = CombatState.Event;
        [SerializeField] CombatWriter _combatWriter;

        [SerializeField] public AbilityData AttackOfOpportunity;
        [SerializeField] public AbilityData CounterAttack;

        //API
        public List<Combatant> Allies;
        public List<Combatant> Enemies;
        public List<Targetable> Objects;
        public List<Targetable> Targetables;
        public List<Position> AllySidePositions = new List<Position>();
        public List<Position> EnemySidePositions = new List<Position>();
        // public Combatant Focus => _focus;
        // public AbilityData Ability => _ability;
        // public Targetable Target => _target;
        public CombatState State => _combatState;
        public CombatSide Turn => _turn;
        // public List<Targetable> PossibleTargets => _possibleTargets;
        // public List<Combatant> AvailableActors => _availableActors;

        //QUEUES
        // public Queue<Combatant> _readyCombatants = new Queue<Combatant>();
        // public Queue<Action> _readyActions = new Queue<Action>();

        //EVENTS
        // public event EventHandler OnTurnEnd;
        
        //TOOLS
        // private static CheckingTool _checkingTool;
        // private static TargetingTool _targetingTool;

        private void Awake()
        {
            Current = this;
        }

        private void Start(){
            EstablishPositions();
            BeginCombat();
        }

        public void BeginCombat(){
            EventBroadcaster.Current.StartEventPhase();
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

        public void EndTurn(){
            if (_turn == CombatSide.Ally){
                _turn = CombatSide.Enemy;
            }
            else if (_turn == CombatSide.Enemy){
                _turn = CombatSide.Ally;
            }
            ChangeState(CombatState.Event);
            EventBroadcaster.Current.CallEndTurn();
            EventBroadcaster.Current.StartEventPhase();
        }

        public void ChangeState(CombatState state){
            _combatState = state;
        }

        // private void Start()
        // {
        //      _checkingTool = new CheckingTool();
        //     _targetingTool = new TargetingTool(Targetables);
        //     EstablishPositions();
        //     FilterAvailableActors();
        //     _combatWriter.WriteAvailableActors(_availableActors);

        //     // OnTurnEnd?.Invoke(this, EventArgs.Empty);
        //     // Dequeue();
        //     // Reset();
        // }

        private void EstablishPositions()
        {
            var positions = FindObjectsOfType<Position>().ToList();

            foreach (var p in positions)
            {
                if (p.PlayingFieldSide == CombatSide.Ally){
                    AllySidePositions.Add(p);
                }
                else if (p.PlayingFieldSide == CombatSide.Enemy){
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

        // private void CollectCombatants()
        // {
        //     var targetables = FindObjectsOfType<Targetable>();
        //     var actors = FindObjectsOfType<Combatant>().Where(a => a.Targetable.TargetType == TargetType.Actor);
        //     var enemies = FindObjectsOfType<Combatant>().Where(e => e.Targetable.TargetType == TargetType.Enemy);
        //     var obstacles = FindObjectsOfType<Targetable>().Where(o => o.TargetType == TargetType.Obstacle);

        //     Actors = actors.ToList();
        //     Enemies = enemies.ToList();
        //     Objects = obstacles.ToList();
        //     Targetables = targetables.ToList();
        // }

        // private void ChangeState(CombatState newState){
        //     _combatState = newState;
        // }

        // public void QueueUp(Combatant combatant, Action action)
        // {
        //     _readyCombatants.Enqueue(combatant);
        //     _readyActions.Enqueue(action);

        //     _queueCount = _readyCombatants.Count();
        // }

        // private void Dequeue()
        // {
        //     if (_readyCombatants.Count == 0) return;
        //     if (_readyActions.Count == 0) return;

        //     var combatant = _readyCombatants.Dequeue();
        //     var action = _readyActions.Dequeue();

        //     print("dequeuing action, " + _readyCombatants.Count + ": number of ready combatants");
        //     print("dequeuing action, " + _readyActions.Count + ": number of ready actions");
        //     action();

        //     _queueCount = _readyCombatants.Count();
        // }

        // private void Reset()
        // {
        //     print("resetting");
        //     _combatWriter.ClearConsole();
        //     _focus = null;
        //     _ability = null;
        //     _target = null;
        //     _possibleTargets.Clear();
        // }

        // private void StartUserTurn()
        // {
        //     if (Turn == TargetType.Actor)
        //     {
        //         FilterAvailableActors();
        //         _combatWriter.WriteAvailableActors(_availableActors);
        //         ChangeCombatState(CombatState.AllySelect);
        //     }
        // }

        // public void ChangeCombatState(CombatState combatState){
        //     _combatState = combatState;
        // }

        // public void SetAbility(AbilityData ability)
        // {
        //     _combatWriter.ClearConsole();
        //     _ability = ability;
        //     _possibleTargets = _targetingTool.FilterTargetables(_focus.Targetable.TargetType, _ability);

        //     CombatWriter.Singleton.Write(_focus + "Is Planning To Use " + ability);
        //     CombatWriter.Singleton.Write("Available Targets Are: ");
        //     for (int i=0; i<_possibleTargets.Count(); i++){
        //         CombatWriter.Singleton.Write((i+1) + ": " + _possibleTargets[i]);
        //     }

        //     ChangeState(CombatState.TargetSelect);
        // }

        // public void SetFocus(Combatant focus)
        // {
        //     _combatWriter.ClearConsole();
        //     _focus = focus;
        //     CombatWriter.Singleton.Write(focus + " Is Selected");
        //     CombatWriter.Singleton.Write("Available Abilities Are: ");
        //     for (int i=0; i<_focus.Abilities.Count(); i++){
        //         CombatWriter.Singleton.Write((i+1) + ": " + _focus.Abilities[i]);
        //     }

        //     ChangeState(CombatState.AbilitySelect);
        // }

        // public void SetTarget(Targetable target)
        // {
        //     _combatWriter.ClearConsole();
        //     _target = target;
        //     CombatWriter.Singleton.Write(_focus + "Is Planning To Use " + _ability + " Against " + target );
        //     CombatWriter.Singleton.Write("Press Y to confirm or N to Cancel");
        //     ChangeState(CombatState.ConfirmSelection);
        // }

        // public void ExecuteAbility()
        // {
        //     _combatWriter.ClearConsole();
        //     CombatWriter.Singleton.Write(_focus.gameObject.name + " performs the ability " + _ability.Name + " on the target " + _target.gameObject.name + "!");

        //     if (_ability.Movement == Movement.Advance)
        //         _focus.Position.Advance(_target.Position, _ability.MovementDegree);

        //     if (_ability.Movement == Movement.Retreat)
        //         _focus.Position.Retreat(_target.Position, _ability.MovementDegree);

        //     _focus.PerformAbility();
        // }

        // private void ActionEnd()
        // {
        //     if (_checkingTool.CheckTurnOver(Turn, Actors, Enemies))
        //     {
        //         TurnEnd();
        //         return;
        //     }

        //     _focus.Tap(true);
        //     Reset();
        //     print("Dequeuing via action end");
        //     Dequeue();
        // }

        // private void UntapAll()
        // {
        //     print("untapping all");
        //     foreach (var a in Actors)
        //     {
        //         a.Tap(false);
        //     }
        //     foreach (var e in Enemies)
        //     {
        //         e.Tap(false);
        //     }
        // }

        // private void TurnEnd()
        // {
        //     CombatWriter.Singleton.Write("TURN END!");
        //     _turnsCount++;
        //     UntapAll();
        //     ChangeTurn();
        //     OnTurnEnd?.Invoke(this, EventArgs.Empty);
        //     print("Dequeuing via turn end");
        //     Dequeue();
        // }

        // private void ChangeTurn()
        // {
        //     if (Turn == TargetType.Actor)
        //     {
        //         Turn = TargetType.Enemy;
        //         ChangeCombatState(CombatState.EnemyTurn);
        //         print("Changing State From Actor to Enemy");
        //     }
        //     else if (Turn == TargetType.Enemy)
        //     {
        //         Turn = TargetType.Actor;
        //         ChangeCombatState(CombatState.AllySelect);
        //         print("Changing State From Enemy to Actor");
        //     }
        // }

        // public void AbilityAnimationEnd()
        // {
        //     if (_checkingTool.CheckVictory(Turn, Actors, Enemies)) return;

        //     ActionEnd();
        // }

        // private void FilterAvailableActors(){
        //     _availableActors.Clear();

        //     foreach (var a in Actors){
        //         if (!_checkingTool.IsAvailable(a)) continue;

        //         _availableActors.Add(a);
        //     }
        // }
    }
}