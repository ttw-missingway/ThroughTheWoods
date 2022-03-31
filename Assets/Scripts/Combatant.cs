using System;
using System.Collections;
using System.Collections.Generic;
using TTW.Systems;
using UnityEngine;

namespace TTW.Combat
{
    public class Combatant : MonoBehaviour
    {
        List<AbilityData> abilities = new List<AbilityData>();

        [SerializeField] int _exhaust = 0;
        [SerializeField] int _channel = 0;
        [SerializeField] bool _exhausted;
        [SerializeField] bool _channeling;
        [SerializeField] Ability _channeledAbility;
        [SerializeField] List<Targetable> _channelTargets = new List<Targetable>();
        CombatInstance _combatInst;
        public bool Exhausted => _exhausted;
        public int ExhaustTime => _exhaust;
        public bool Channeling => _channeling;
        public int ChannelTime => _channel;

        public Position Position { get; set; }

        public event EventHandler onExhaustUpdate;

        private void Awake()
        {
            Position = GetComponent<Position>();
        }

        private void Start()
        {
            _combatInst = CombatInstance.Current;
            _combatInst.OnActionEnd += _combatInst_OnActionEnd;
        }

        private void _combatInst_OnActionEnd(object sender, EventArgs e)
        {
            _exhaust--;
            _channel--;
            if (_exhaust < 0) _exhaust = 0;
            if (_channel < 0) _channel = 0;
            _exhausted = _exhaust > 0;
            _channeling = _channel > 0;
            onExhaustUpdate?.Invoke(this, EventArgs.Empty);

            _combatInst.QueueUp(this, PerformChanneledAbility);
        }

        private void PerformChanneledAbility()
        {
            if (_channeledAbility != null && _channel == 0)
            {
                foreach (var t in _channelTargets)
                {
                    SendAbility(t, _channeledAbility);
                }

                _channeledAbility = null;
                _channelTargets.Clear();
            }
        }

        public void PerformAbility()
        {
            var selectedAbility = CombatInstance.Current.Ability;
            var selectedTarget = CombatInstance.Current.Target;

            if (selectedAbility == null)
            {
                print("selected ability is null somehow!");
                return;
            }

            Ability ability = new Ability(selectedAbility, this);

            if (selectedAbility.ChannelTime > 0)
            {
                SetChannel(selectedAbility.ChannelTime, ability);
                _channelTargets.Add(selectedTarget);

                return;
            }

            var fx = Instantiate(selectedAbility.abilityFX);

            SendAbility(selectedTarget, ability);
        }

        private void SendAbility(Targetable selectedTarget, Ability ability)
        {
            selectedTarget.ReceiveAbility(ability);
            SetExhaust(ability.ExhaustTime);
        }

        private void SetExhaust(int exhaustTime)
        {
            _exhaust = exhaustTime + 1; //plus one because the end of turn will wipe 1 away regardless
            _exhausted = true;
            onExhaustUpdate?.Invoke(this, EventArgs.Empty);
        }

        private void SetChannel(int channelTime, Ability ability)
        {
            _channeledAbility = ability;
            _channel = channelTime;
            _channeling = true;
        }
    }
}