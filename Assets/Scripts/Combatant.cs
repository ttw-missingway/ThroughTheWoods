using System;
using System.Collections.Generic;
using TTW.Systems;
using UnityEngine;

namespace TTW.Combat
{
    public class Combatant : MonoBehaviour
    {
        [SerializeField] ActorData _actor;
        [SerializeField] public List<AbilityData> Abilities = new List<AbilityData>();
        [SerializeField] Ability _channeledAbility;
        [SerializeField] List<Targetable> _channelTargets = new();
        [SerializeField] GameObject _attackIcon;

        public ActorData Actor => _actor;
        Position _position;
        Targetable _targetable;
        public Position Position => _position;
        public Targetable Targetable => _targetable;
        public event EventHandler OnExhaustUpdate;
        AbilityQueue _abilityQueue;
        LinkLibrary _linkLibrary;
        LinkLibrary.LinkClass _linkClass = LinkLibrary.LinkClass.Ability;
        EventBroadcaster _eventBroadcaster;
        Health _health;
        ActionProcessor _actionProcessor;
        public ActionProcessor ActionProcessor => _actionProcessor;
        public Health Health => _health;
        Exhaust _exhaust;
        Channel _channel;
        public Channel Channel => _channel;
        TargetingTool _tTool;
        AoO _aoo;
        public AoO AoO => _aoo;

        private void Awake()
        {
            _position = GetComponent<Position>();
            _health = GetComponent<Health>();
            _targetable = GetComponent<Targetable>();
            _actionProcessor = GetComponent<ActionProcessor>();
            _abilityQueue = CombatManager.Current.GetComponent<AbilityQueue>();
            _linkLibrary = CombatManager.Current.LinkLibrary;
            _tTool = new TargetingTool();
            _aoo = new AoO();
            RegisterAbilities();
        }

        private void Start()
        {
            _exhaust = _health.Exhaust;
            _channel = _health.Channel;
            _eventBroadcaster = CombatManager.Current.EventBroadcaster;
            _eventBroadcaster.StartOfEventPhase += OnTurnEnd;
        }

        private void RegisterAbilities(){
            foreach(AbilityData a in Abilities){
                _linkLibrary.AddLink(a.Keyword, _linkClass, a);
            }
        }

        public void OpenAttackIcon(){
            _attackIcon.SetActive(true);
        }

        public void CloseAttackIcon(){
            _attackIcon.SetActive(false);
        }

        private void OnTurnEnd(object sender, EventArgs e)
        {
            Health.Tap(false);
        }

        public void SendAbility(Ability ability)
        {
            var desiredTargets = new List<Targetable>(ability.CurrentTargets);

            ability.ClearTargets();
            foreach(Targetable t in desiredTargets){
                if (_tTool.TargetingConditionsCheck(this, t, ability.AbilityData, true)){
                    ability.AddTarget(t);
                }
            }

            _abilityQueue.AddAbility(ability);
            Health.Tap(true);
        }

        internal void CounterAttack(Targetable target)
        {
            Ability counter = new(CombatManager.Current.CounterAttack, this);
            counter.CurrentTargets.Add(target);
            SendAbility(counter);
        }

        public void OnAbilityCommence(Ability ability){
            if (ability.ExhaustTime > 0)
                _exhaust.SetExhaust(ability.ExhaustTime);
        }
    }
}