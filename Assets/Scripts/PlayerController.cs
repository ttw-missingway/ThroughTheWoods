using System;
using System.Collections;
using System.Collections.Generic;
using TTW.Systems;
using UnityEngine;
using System.Linq;

namespace TTW.Combat{

    public enum PlayerTurn{
        SelectCombatant,
        SelectAbility,
        SelectSubAbility,
        SelectTarget,
        ReadyToExecute,
        ReadyToEnd,
        Waiting
    }

    public class PlayerController : MonoBehaviour
    {
        [SerializeField] CombatManager _combatManager;
        [SerializeField] PlayerTurn _currentTurn;
        [SerializeField] Combatant _selectedActor;
        [SerializeField] AbilityData _selectedAbility;
        [SerializeField] List<AbilityData> _availableAbilities;
        [SerializeField] List<Combatant> _availableActors;
        [SerializeField] List<Targetable> _availableTargets;
        [SerializeField] List<Targetable> _selectedTargets = new List<Targetable>();

        TargetingTool _tTool;
        CheckingTool _cTool;

        EventBroadcaster _events;

        private void Start()
        {
            _tTool = new TargetingTool();
            _cTool = new CheckingTool();
            _combatManager = CombatManager.Current;
            _events = _combatManager.GetComponent<EventBroadcaster>();
            _events.EndAction += StartOfAction;
        }

        public void StartOfAction(object o, EventArgs e)
        {
            if (_combatManager.Turn != CombatSide.Ally) return;

            CombatWriter.Singleton.Write("Select An Actor!");
            _availableActors = _cTool.GetAvailableActors(_combatManager.Allies);
            

            if (_availableActors.Count == 0){
                EndTurn();
                return;
            }

            CombatWriter.Singleton.WriteAvailableCombatants(_availableActors);
        }

        public void ReceiveLink(LinkLibrary.LinkData link){

            if (_combatManager.State != CombatState.Control) return;
            if (_combatManager.Turn != CombatSide.Ally) return;

            if (_selectedActor == null)
            {
                SelectNewActor(link);
                return;
            }
            else if (_selectedAbility == null)
            {
                SelectNewAbility(link);
                return;
            }
            else if (_selectedTargets.Count() == 0)
            {
                SelectNewTargets(link);
            }
        }

        private void SelectNewTargets(LinkLibrary.LinkData link)
        {
            if (link.LinkClass == LinkLibrary.LinkClass.Enemy || link.LinkClass == LinkLibrary.LinkClass.Ally || link.LinkClass == LinkLibrary.LinkClass.Object)
            {
                var availableTargets = _tTool.FilterTargetables(_selectedActor, _selectedAbility, false);
                var matchingTarget = availableTargets.Where(a => a.GetComponent<Targetable>().Keyword == link.Keyword).FirstOrDefault();
                if (matchingTarget != null)
                {
                    _selectedTargets.Add(matchingTarget);
                }
                else{
                    var failedTarget = _tTool.AllTargetables.Where(t => t.Keyword == link.Keyword).FirstOrDefault();
                    _tTool.TargetingConditionsCheck(_selectedActor, failedTarget, _selectedAbility, writeReason: true);
                    return;
                }

                CombatWriter.Singleton.ClearConsole();
                CombatWriter.Singleton.Write("Performing " + _selectedAbility.Name);
                Execute();
            }
        }

        private void SelectNewAbility(LinkLibrary.LinkData link)
        {
            if (link.LinkClass == LinkLibrary.LinkClass.Ability)
            {
                var matchingAbilities = _selectedActor.Abilities.Where(a => a.Keyword == link.Keyword);
                if (matchingAbilities.Count() > 0)
                {
                    _selectedAbility = matchingAbilities.FirstOrDefault();

                    if (_selectedAbility.TargetTypes.Count == 1 && _selectedAbility.TargetTypes[0] == TargetingClass.Self){
                        SetTargetSelf();
                        Execute();
                        return;
                    }

                    CombatWriter.Singleton.ClearConsole();
                    CombatWriter.Singleton.Write("Select Target");
                }
            }
        }

        private void SelectNewActor(LinkLibrary.LinkData link)
        {
            if (link.LinkClass == LinkLibrary.LinkClass.Ally)
            {
                _availableActors = _cTool.GetAvailableActors(_combatManager.Allies);

                var matchingActor = _availableActors.Where(a => a.GetComponent<Targetable>().Keyword == link.Keyword).FirstOrDefault();
                if (matchingActor != null)
                {
                    _selectedActor = matchingActor;
                    WriteAbilities();
                }
                else{
                    var failedActor = _combatManager.Allies.Where(a => a.GetComponent<Targetable>().Keyword == link.Keyword).FirstOrDefault();
                    _cTool.IsAvailable(failedActor, writeReason: true);
                }
            }
        }

        private void WriteAbilities()
        {
            CombatWriter.Singleton.WriteAvailableAbilities(_selectedActor.Abilities);
        }

        public void SetActor(Combatant actor){
            _selectedActor = actor;
        }

        public void SetAbility(AbilityData ability){
            _selectedAbility = ability;

            if (ability.TargetingMode == TargetScope.Random)
                RandomTarget(ability);
            else if (ability.TargetingMode == TargetScope.Global)
                GlobalTargets(ability);
        }

        public void SetTarget(Targetable target){
            _selectedTargets.Add(target);
        }

        private void RandomTarget(AbilityData ability){
            var targetables = _tTool.FilterTargetables(_selectedActor, ability, false);
            var randomInt = UnityEngine.Random.Range(0, targetables.Count);
            _selectedTargets.Add(targetables[randomInt]);
        }

        private void GlobalTargets(AbilityData ability){
            var targetables = _tTool.FilterTargetables(_selectedActor, ability, false);
            foreach(Targetable t in targetables){
                _selectedTargets.Add(t); 
            }
        }

        internal void SetTargetSelf()
        {
            _selectedTargets.Add(_selectedActor.Targetable);
        }

        private void Update(){
            if (Input.GetKeyDown(KeyCode.Return)){
                EndTurn();
            }
        }

        // private void Update(){

        //     if (_combatManager.Turn != CombatTurn.Actor || _combatManager.State != CombatState.Control){
        //         _currentTurn = PlayerTurn.Waiting;
        //         return;
        //     }
        //     else if (_currentTurn == PlayerTurn.Waiting){
        //         _currentTurn = PlayerTurn.SelectCombatant;
        //         _availableActors = cTool.GetAvailableActors(_combatManager.Actors);
        //         CombatWriter.Singleton.WriteAvailableCombatants(_availableActors);
        //         if (_availableActors.Count ==0){
        //             CombatWriter.Singleton.Write("No Available Actors Left, Press Enter To End Turn");
        //         }
        //     }

        //     if (Input.GetKeyDown(KeyCode.B)){
        //         if (_currentTurn == PlayerTurn.SelectAbility){
        //             _currentTurn = PlayerTurn.SelectCombatant;
        //             CombatWriter.Singleton.WriteAvailableCombatants(_availableActors);
        //         }
        //         else if (_currentTurn == PlayerTurn.SelectTarget){
        //             _currentTurn = PlayerTurn.SelectAbility;
        //             CombatWriter.Singleton.WriteAvailableAbilities(_selectedActor.Abilities);
        //         }
        //         else if (_currentTurn == PlayerTurn.ReadyToExecute){
        //             _currentTurn = PlayerTurn.SelectTarget;
        //             if (_availableTargets != null)
        //                     CombatWriter.Singleton.WriteAvailableTargets(_availableTargets);
        //                 else    
        //                     CombatWriter.Singleton.Write("No available targets");
        //         }
        //     }

        //     if (Input.GetKeyDown(KeyCode.Return)){
        //         CombatWriter.Singleton.Write("Are You Sure You Want To End Your Turn? Y/N");
        //         _currentTurn = PlayerTurn.ReadyToEnd;
        //     }

        //     if (_currentTurn == PlayerTurn.ReadyToEnd){
        //         if (Input.GetKeyDown(KeyCode.Y)){
        //             CombatWriter.Singleton.Write("Enemy Turn!");
        //             EndTurn();
        //         }
        //         if (Input.GetKeyDown(KeyCode.N)){
        //             _currentTurn = PlayerTurn.Waiting;
        //         }
        //     }

        //     if (_currentTurn == PlayerTurn.SelectCombatant){
        //         for (var i=0; i<keyCodes.Length; i++){
        //             if (Input.GetKeyDown(keyCodes[i])){
        //                 _selectedActor = _availableActors[i];
        //                 CombatWriter.Singleton.Write("Selected " + _selectedActor);
        //                 _currentTurn = PlayerTurn.SelectAbility;
        //                 _availableAbilities = tTool.FilterAbilities(_selectedActor, _selectedActor.Abilities);
        //                 if (_availableAbilities.Count > 0){
        //                     CombatWriter.Singleton.WriteAvailableAbilities(_availableAbilities);
        //                 }
        //                 else{
        //                     CombatWriter.Singleton.Write("No Available Abilities");
        //                 }

        //             }  
        //         }  
        //     }
        //     else if (_currentTurn == PlayerTurn.SelectAbility){
        //         for (var i=0; i<keyCodes.Length; i++){
        //             if (Input.GetKeyDown(keyCodes[i])){
        //                 _selectedAbility = _availableAbilities[i];
        //                 CombatWriter.Singleton.Write("Selected " + _selectedAbility);
        //                 _currentTurn = PlayerTurn.SelectTarget;
        //                 _availableTargets = tTool.FilterTargetables(_selectedActor, _selectedAbility);
        //                 if (_availableTargets.Count > 0)
        //                     CombatWriter.Singleton.WriteAvailableTargets(_availableTargets);
        //                 else    
        //                     CombatWriter.Singleton.Write("No available targets");
        //             }
        //         }
        //     }
        //     else if (_currentTurn == PlayerTurn.SelectTarget) {
        //         for (var i=0; i<keyCodes.Length; i++){
        //             if (Input.GetKeyDown(keyCodes[i])){
        //                 _selectedTargets.Add(_availableTargets[i]);
        //                 CombatWriter.Singleton.Write("Selected " + _availableTargets[i]);
        //                 _currentTurn = PlayerTurn.ReadyToExecute;
        //                 CombatWriter.Singleton.Write("Press Space To Perform Ability");
        //             }
        //         }
        //     }
        //     else if (_currentTurn == PlayerTurn.ReadyToExecute){
        //         if (Input.GetKeyDown(KeyCode.Space)){
        //             var ability = new Ability(_selectedAbility, _selectedActor);
        //             _selectedActor.ReceiveAbility(ability, _selectedTargets);
        //             Clear();

        //             if (ability.ChannelTime > 0){
        //                 CombatWriter.Singleton.Write("channeling attack!");
        //                 _currentTurn = PlayerTurn.Waiting;
        //             }
        //             else{
        //                 CombatWriter.Singleton.Write("performing attack!");
        //                 _combatManager.ChangeState(CombatState.Animation);
        //             }
        //         }
        //     }
        // }

        public void Execute(){
            var tTool = new TargetingTool();

            bool commenseExecute = true;

            foreach(Targetable t in _selectedTargets){
                if (!tTool.TargetingConditionsCheck(_selectedActor, t, _selectedAbility, true)){
                    commenseExecute = false;
                }
            }

            if (!commenseExecute) return;
    
            var ability = new Ability(_selectedAbility, _selectedActor);

            foreach (Targetable t in _selectedTargets){
                ability.CurrentTargets.Add(t);
            }

            _selectedActor.ReceiveAbility(ability);
            Clear();

            if (ability.ChannelTime > 0){
                CombatWriter.Singleton.Write("channeling attack!");
                _currentTurn = PlayerTurn.Waiting;
            }
            else{
                CombatWriter.Singleton.Write("performing attack!");
                _combatManager.ChangeState(CombatState.Animation);
            }
        }

        private void Clear()
        {
            _selectedActor = null;
            _selectedAbility = null;
            _selectedTargets.Clear();
            _availableTargets.Clear();
        }

        public void EndTurn(){
            _combatManager.EndTurn();
        }
    }
}
