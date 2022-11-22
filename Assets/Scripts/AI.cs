using System.Collections.Generic;
using TTW.Systems;
using UnityEngine;
using System.Linq;
using System;

namespace TTW.Combat
{
    public class AI : MonoBehaviour
    {
        [SerializeField] CombatManager _combatManager;
        [SerializeField] PlayerTurn _currentTurn;
        [SerializeField] Combatant _selectedEnemy;
        [SerializeField] AbilityData _selectedAbility;
        [SerializeField] List<AbilityData> _availableAbilities;
        [SerializeField] List<Combatant> _availableEnemies;
        [SerializeField] List<Targetable> _availableTargets;
        [SerializeField] List<Targetable> _selectedTargets = new List<Targetable>();
        TargetingTool tTool;
        CheckingTool cTool;

        private void Start()
        {
            tTool = new TargetingTool();
            cTool = new CheckingTool();
            _availableEnemies = cTool.GetAvailableEnemies(_combatManager.Enemies);
            EventBroadcaster.Current.StartTurn += _OnTurnStart;
            EventBroadcaster.Current.EndAction += _OnActionEnd;
        }

        private void _OnActionEnd(object sender, EventArgs e)
        {
            if (_combatManager.Turn == CombatSide.Enemy && _combatManager.State == CombatState.Control){
                StartOfTurn();
            }
        }

        private void _OnTurnStart(object sender, EventArgs e)
        {
            if (_combatManager.Turn == CombatSide.Enemy && _combatManager.State == CombatState.Control){
                StartOfTurn();
            }
        }

        public void StartOfTurn(){
            _availableEnemies = cTool.GetAvailableEnemies(_combatManager.Enemies);
            var _shuffledList = Shuffle<Combatant>(_availableEnemies, _availableEnemies.Count);

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
                a.ReceiveAbility(ability, _selectedTargets);
                Clear();
                return;
            }

            EndTurn();
        }

        public IEnumerable<T> Shuffle<T>(IEnumerable<T> list, int size)
        {
            var r = new System.Random();
            var shuffledList = 
                list.
                    Select(x => new { Number = r.Next(), Item = x }).
                    OrderBy(x => x.Number).
                    Select(x => x.Item).
                    Take(size);

            return shuffledList.ToList();
        }

        private void EndTurn(){
            Clear();
            _combatManager.EndTurn();
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
