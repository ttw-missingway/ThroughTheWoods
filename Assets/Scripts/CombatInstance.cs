using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTW.Systems;
using TTW.UI;
using TTW.World;
using UnityEngine;

namespace TTW.Combat
{
    public class CombatInstance : MonoBehaviour
    {
        [SerializeField] int _actionsCount = 0;
        [SerializeField] Combatant _focus;
        [SerializeField] AbilityData _ability;
        [SerializeField] Targetable _target;
        public static CombatInstance Current;
        public Queue<Combatant> _readyCombatants = new Queue<Combatant>();
        public Queue<Action> _readyActions = new Queue<Action>();

        public Combatant Focus => _focus;
        public AbilityData Ability => _ability;
        public Targetable Target => _target;

        public event EventHandler OnActionEnd;

        private void Awake()
        {
            Current = this;
        }

        private void Start()
        {
            var positions = FindObjectsOfType<Position>().ToList();

            foreach (var p in positions)
            {
                p.CombatStart(positions);
            }
        }

        public void QueueUp(Combatant combatant, Action action)
        {
            _readyCombatants.Enqueue(combatant);
            _readyActions.Enqueue(action);
        }

        private void Dequeue()
        {
            if (_readyCombatants.Count == 0) return;
            if (_readyActions.Count == 0) return;

            var combatant = _readyCombatants.Dequeue();
            var action = _readyActions.Dequeue();

            action();
        }

        public void Reset()
        {
            _focus = null;
            _ability = null;
            _target = null;

            //will need to be changed to wait once wait is implemented
            UISelectionController.Current.ChangeSelectionState(SelectionState.Ability);
        }

        public void SetAbility(AbilityData ability)
        {
            _ability = ability;
        }

        public void SetFocus(Combatant focus)
        {
            _focus = focus;
        }

        public void SetTarget(Targetable target)
        {
            _target = target;
        }

        public void ExecuteAbility()
        {
            if (_ability.Movement == Movement.Advance)
                _focus.Position.Advance(_target.Position, _ability.MovementDegree);

            if (_ability.Movement == Movement.Retreat)
                _focus.Position.Retreat(_target.Position, _ability.MovementDegree);

            _focus.PerformAbility();

            print(_focus.gameObject.name + " performs the ability " + _ability.Name + " on the target " + _target.gameObject.name + "!");
        }

        private void ActionEnd()
        {
            _actionsCount++;
            OnActionEnd?.Invoke(this, EventArgs.Empty);
            Dequeue();
            Reset();
        }

        public void AbilityAnimationEnd()
        {
            ActionEnd();
        }
    }
}
