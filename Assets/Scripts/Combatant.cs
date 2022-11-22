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
        [SerializeField] List<Targetable> _channelTargets = new List<Targetable>();
        [SerializeField] GameObject _attackIcon;

        public ActorData Actor => _actor;
        Position _position;
        Targetable _targetable;
        public Position Position => _position;
        public Targetable Targetable => _targetable;
        public event EventHandler onExhaustUpdate;
        AbilityQueue _abilityQueue;
        LinkLibrary _linkLibrary;
        LinkLibrary.LinkClass _linkClass = LinkLibrary.LinkClass.Ability;
        EventBroadcaster _eventBroadcaster;
        Health _health;
        public Health Health => _health;
        Exhaust _exhaust;
        Channel _channel;
        TargetingTool _tTool;

        private void Awake()
        {
            _position = GetComponent<Position>();
            _health = GetComponent<Health>();
            _targetable = GetComponent<Targetable>();
            _abilityQueue = CombatManager.Current.GetComponent<AbilityQueue>();
            _linkLibrary = CombatManager.Current.LinkLibrary;
            _tTool = new TargetingTool();
            RegisterAbilities();
        }

        private void Start()
        {
            _exhaust = _health.Exhaust;
            _channel = _health.Channel;
            _eventBroadcaster = CombatManager.Current.EventBroadcaster;
            _eventBroadcaster.EndTurn += _OnTurnEnd;
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

        private void _OnTurnEnd(object sender, EventArgs e)
        {
            Health.Tap(false);
        }

        // public void PerformChanneledAbility()
        // {
        //     print(this + " is performing channeled ability " + _channeledAbility.AbilityData.name);
        //     var tTool = new TargetingTool();

        //     foreach (Targetable t in _channelTargets){
        //         if (!tTool.TargetingConditionsCheck(this, t, _channeledAbility.AbilityData, true)){
        //             _channelTargets.Remove(t);
        //         }
        //     }

        //     SendAbility(_channelTargets, _channeledAbility);
        //     _channeling = false;
        //     _channeledAbility = null;
        //     _channelTargets.Clear();
        // }

        public void SendAbility(Ability ability)
        {
            var desiredTargets = new List<Targetable>(ability.CurrentTargets);

            foreach(Targetable t in desiredTargets){
                print(t.Name);
            }

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
            Ability counter = new Ability(CombatManager.Current.CounterAttack, this);
            counter.CurrentTargets.Add(target);
            SendAbility(counter);
        }

        public void AooRequest(AoORequest request){
            if (request.AlreadyRequested(Targetable)) return;

            ProximityTool pTool = new ProximityTool(request.Requestee.Position);

            if (!pTool.IsChained(Position)) return;

            request.AddRequested(Targetable);
            
            float random;
            float baseChance = 10f;

            if (GetComponent<Health>().Stance == Stance.Alert){
                random = -1f;
            }
            else{
                random = UnityEngine.Random.Range(0f, 100f);
            }

            if (random < baseChance){
                ExecuteAoO(request.Target);
            }

            foreach (Position p in Position.Neighbors){
                if (p == null) continue;

                if (p.GetComponent<Combatant>() != null){
                    p.GetComponent<Combatant>().AooRequest(request);
                }
            }
        }

        public void ExecuteAoO(Targetable target){
            var aooData = CombatManager.Current.AttackOfOpportunity;
            Ability aoo = new Ability(aooData, this);
            aoo.AttackOfOpportunity();
            aoo.CurrentTargets.Add(target);

            SendAbility(aoo);
        }

        //Can this be moved elsewhere?
        public void ReceiveAbility(Ability ability)
        {
            if (ability.ExhaustTime > 0){
                _exhaust.SetCountDown(ability.ExhaustTime);
            }
                
            if (ability.ChannelTime > 0){
                _channel.StartChannel(ability.ChannelTime, ability);
                return;
            }

            ChangeStance(ability);

            // if (ability.Reposition){
            //     Reposition(targets[0], ability);
            // }

            SendAbility(ability);
        }

        // private void SetChannel(Ability ability, List<Targetable> targets)
        // {
        //     _channel = ability.ChannelTime;
        //     _channeledAbility = ability;
        //     foreach (Targetable t in targets)
        //     {
        //         _channelTargets.Add(t);
        //     }
        //     _channeling = true;
        //     _eventBroadcaster.Channeler.AddToChannelOrder(this);
        // }

        private void ChangeStance(Ability ability)
        {
            var health = GetComponent<Health>();

            if (ability.Stance == Stance.Wait)
            {
                if (ability.Stance == Stance.None)
                    health.ChangeStance(Stance.Alert);
            }
            else
            {
                health.BreakStance();
                if (ability.ChangeStance)
                {
                    health.ChangeStance(ability.Stance);
                }
            }
        }

        // private void Reposition(Targetable target, Ability ability)
        // {
        //     if (ability.Movement == Movement.Advance)
        //         Position.Advance(target.Position, ability.MovementDegree);

        //     if (ability.Movement == Movement.Retreat)
        //         Position.Retreat(target.Position, ability.MovementDegree);
        // }
    }
}