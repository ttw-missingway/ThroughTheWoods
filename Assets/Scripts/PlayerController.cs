using System;
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
        BoardManager _board;

        bool _awake;

        private void Start()
        {
            _tTool = new TargetingTool();
            _cTool = new CheckingTool();
            _combatManager = CombatManager.Current;
            _board = _combatManager.Board;
            _events = _combatManager.GetComponent<EventBroadcaster>();
            _events.PromptAction += OnPromptAction;
            _events.EndTurnPrompt += OnEndTurnPrompt;
            _events.StartOfAlliesTurn += OnStartOfTurn;
            _events.EndOfAlliesTurn += OnEndOfTurn;
        }

        private void OnEndOfTurn(object sender, EventArgs e)
        {
            _awake = false;
        }

        private void OnStartOfTurn(object sender, EventArgs e)
        {
            _awake = true;
            _combatManager.CheckForTurnOver();
        }

        private void OnEndTurnPrompt(object sender, EventArgs e)
        {
            if (!_awake) return;

            PromptEndOfTurn();
        }

        private void PromptEndOfTurn()
        {
            CombatWriter.Singleton.WriteEndTurnPrompt();
        }

        public void OnPromptAction(object o, EventArgs e)
        {
            if (!_awake) return;

            PromptNewAction();
        }

        private void PromptNewAction()
        {
            CombatWriter.Singleton.Write("Select An Actor!");

            CombatWriter.Singleton.WriteAvailableCombatants(_board.GetAvailableAllies());
        }

        public void ReceiveLink(LinkLibrary.LinkData link){
            // if (_combatManager.Turn != CombatTurn.Ally || !_combatManager.State == CombatState.Control) return;
            if (!_awake) return;

            if (link.Keyword == "endturn")
                _combatManager.EndTurn();

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
                _availableActors = _cTool.GetAvailableAllies(_board.Allies);

                var matchingActor = _availableActors.Where(a => a.GetComponent<Targetable>().Keyword == link.Keyword).FirstOrDefault();
                if (matchingActor != null)
                {
                    _selectedActor = matchingActor;
                    WriteAbilities();
                }
                else{
                    var failedActor = _board.Allies.Where(a => a.GetComponent<Targetable>().Keyword == link.Keyword).FirstOrDefault();
                    _cTool.IsAvailable(failedActor, writeReason: true);
                }
            }
        }

        private void WriteAbilities()
        {
            CombatWriter.Singleton.WriteAvailableAbilities(_selectedActor.Abilities);
        }

        private void SetActor(Combatant actor){
            _selectedActor = actor;
        }

        private void SetAbility(AbilityData ability){
            _selectedAbility = ability;

            if (ability.TargetingMode == TargetScope.Random)
                RandomTarget(ability);
            else if (ability.TargetingMode == TargetScope.Global)
                GlobalTargets(ability);
        }

        private void SetTarget(Targetable target){
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

        private void Execute(){
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

            _selectedActor.ActionProcessor.ReceiveAbility(ability);
            Clear();

            if (ability.ChannelTime > 0){
                CombatWriter.Singleton.Write("channeling attack!");
                _currentTurn = PlayerTurn.Waiting;
                _events.CallEndOfAction();
            }
        }

        private void Clear()
        {
            _selectedActor = null;
            _selectedAbility = null;
            _selectedTargets.Clear();
            _availableTargets.Clear();
        }
    }
}
