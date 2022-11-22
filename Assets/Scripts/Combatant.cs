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
        [SerializeField] int _exhaust = 0;
        [SerializeField] int _channel = 0;
        [SerializeField] bool _exhausted;
        [SerializeField] bool _channeling;
        [SerializeField] public bool Tapped = false;
        [SerializeField] Ability _channeledAbility;
        [SerializeField] List<Targetable> _channelTargets = new List<Targetable>();
        [SerializeField] GameObject _attackIcon;
        public Targetable Targetable { get; private set; }
        public bool Exhausted => _exhausted;
        public int ExhaustTime => _exhaust;
        public bool Channeling => _channeling;
        public int ChannelTime => _channel;
        public ActorData Actor => _actor;

        public Position Position { get; set; }

        public event EventHandler onExhaustUpdate;
        AbilityQueue _abilityQueue;
        LinkLibrary _linkLibrary;
        LinkLibrary.LinkClass _linkClass = LinkLibrary.LinkClass.Ability;

        private void Awake()
        {
            Position = GetComponent<Position>();
            Targetable = GetComponent<Targetable>();
            _abilityQueue = CombatManager.Current.GetComponent<AbilityQueue>();
            _linkLibrary = LinkLibrary.Current;
            RegisterAbilities();
        }

        private void Start()
        {
            EventBroadcaster.Current.EndTurn += _OnTurnEnd;
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

        private void Tap(bool tapped)
        {
            Tapped = tapped;
        }

        public void EmptyTurn(){
            Tap(true);
        }

        private void _OnTurnEnd(object sender, EventArgs e)
        {
            Tap(false);
        }

        public void PerformChanneledAbility()
        {
            print(this + " is performing channeled ability " + _channeledAbility.AbilityData.name);
            var tTool = new TargetingTool();

            foreach (Targetable t in _channelTargets){
                if (!tTool.TargetingConditionsCheck(this, t, _channeledAbility.AbilityData, true)){
                    _channelTargets.Remove(t);
                }
            }

            SendAbility(_channelTargets, _channeledAbility);
            _channeling = false;
            _channeledAbility = null;
            _channelTargets.Clear();
        }

        private void SendAbility(List<Targetable> targets, Ability ability)
        {
            ability.ClearTargets();
            foreach(Targetable t in targets){
                ability.AddTarget(t);
            }
            print("adding ability by " + Targetable.Name + " to queue");
            _abilityQueue.AddAbility(ability);
            Tap(true);
        }

        internal void CounterAttack(Targetable target)
        {
            Ability counter = new Ability(CombatManager.Current.CounterAttack, this);
            List<Targetable> targets = new List<Targetable>();
            targets.Add(target);
            SendAbility(targets, counter);
        }

        public void ReduceChannelTime(){
            _channel--;
            if (_channel <= 0){ _channel = 0;}
        }

        public void ReduceExhaustTime(){
            _exhaust--;
            if (_exhaust <= 0){
                _exhaust = 0;
                _exhausted = false;
            } 
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
            aoo.AttackOfOpprotunity();
            List<Targetable> targets = new List<Targetable>();

            targets.Add(target);

            SendAbility(targets, aoo);
        }

        public void ReceiveAbility(Ability ability, List<Targetable> targets)
        {
            if (ability.ExhaustTime > 0){
                SetExhaust(ability);
            }
                
            if (ability.ChannelTime > 0){
                SetChannel(ability, targets);
                return;
            }

            ChangeStance(ability);

            // if (ability.Reposition){
            //     Reposition(targets[0], ability);
            // }

            SendAbility(targets, ability);
        }

        private void SetChannel(Ability ability, List<Targetable> targets)
        {
            _channel = ability.ChannelTime;
            _channeledAbility = ability;
            foreach (Targetable t in targets)
            {
                _channelTargets.Add(t);
            }
            _channeling = true;
            EventBroadcaster.Current.Channeler.AddToChannelOrder(this);
        }

        private void SetExhaust(Ability ability)
        {
                _exhaust = ability.ExhaustTime;
                _exhausted = true;
        }

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