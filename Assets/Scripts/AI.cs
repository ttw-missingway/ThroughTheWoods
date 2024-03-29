using System.Collections.Generic;
using TTW.Systems;
using UnityEngine;
using System.Linq;
using System;

namespace TTW.Combat
{
    public class AI : MonoBehaviour
    {
        //test comment
        CombatManager _combatManager;
        EventBroadcaster _eventBroadcaster;
        BoardManager _board;
        [SerializeField] PlayerTurn _currentTurn;
        [SerializeField] Combatant _selectedEnemy;
        [SerializeField] AbilityData _selectedAbility;
        [SerializeField] List<AbilityData> _availableAbilities;
        [SerializeField] List<Combatant> _availableEnemies;
        [SerializeField] List<Targetable> _availableTargets;
        [SerializeField] List<Targetable> _selectedTargets = new List<Targetable>();
        TargetingTool tTool;
        CheckingTool cTool;

        bool _awake = false;

        private void Start()
        {
            _combatManager = CombatManager.Current;
            _eventBroadcaster = _combatManager.EventBroadcaster;
            _board = _combatManager.Board;
            tTool = new TargetingTool();
            cTool = new CheckingTool();
            _availableEnemies = cTool.GetAvailableEnemies(_board.Enemies);
            _eventBroadcaster.PromptAction += _OnPromptAction;
            _eventBroadcaster.EndTurnPrompt += _OnEndTurnPrompt;
            _eventBroadcaster.StartOfEnemiesTurn += _OnTurnStart;
            _eventBroadcaster.EndOfEnemiesTurn += _OnEndTurn;
        }

        private void _OnEndTurnPrompt(object sender, EventArgs e)
        {
            if (!_awake) return;

            _combatManager.EndTurn();
        }

        private void _OnEndTurn(object sender, EventArgs e)
        {
            _awake = false;
            Clear();
        }

        private void _OnPromptAction(object sender, EventArgs e)
        {
            if (!_awake) return;

            PerformAction();
        }

        private void _OnTurnStart(object sender, EventArgs e)
        {
            _awake = true;
            _combatManager.CheckForTurnOver();
        }

        public void PerformAction(){
            if (!_awake) return;

            _availableEnemies = cTool.GetAvailableEnemies(_board.Enemies);
            TTWMath math = new TTWMath();
            var _shuffledList = math.Shuffle<Combatant>(_availableEnemies, _availableEnemies.Count);

            foreach (Combatant a in _shuffledList){
                var _availableAbilities = tTool.FilterAbilities(a, a.Abilities);
                if (_availableAbilities.Count == 0) continue;
                var randomSelection = UnityEngine.Random.Range(0, a.Abilities.Count());
                _selectedAbility = _availableAbilities[randomSelection];

                var _availableTargets = tTool.FilterTargetables(a, _selectedAbility, false);
                if (_availableTargets.Count == 0) continue;
                var randomTarget = UnityEngine.Random.Range(0, _availableTargets.Count());
                _selectedTargets.Add(_availableTargets[randomTarget]);

                var ability = new Ability(_selectedAbility, a);

                foreach (Targetable t in _selectedTargets){
                    ability.CurrentTargets.Add(t);
                }

                a.ActionProcessor.ReceiveAbility(ability);
                Clear();
                return;
            }
        }

        private void Clear()
        {
            _selectedEnemy = null;
            _selectedAbility = null;
            _selectedTargets.Clear();
            _availableTargets.Clear();
        }
    }
}
