using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using TTW.Systems;
using TTW.UI;
using UnityEngine;
using TMPro;

namespace TTW.Combat
{
    public class Health: MonoBehaviour
    {
        [SerializeField] private List<Status> _statusEffects = new List<Status>();
        [SerializeField] private Stance _currentStance = Stance.None;
        [SerializeField] private Exhaust _exhaust;
        [SerializeField] private Channel _channel;
        [SerializeField] private bool _tapped;
        public List<Status> StatusEffects => _statusEffects;
        public Stance Stance => _currentStance;
        public Exhaust Exhaust => _exhaust;
        public Channel Channel => _channel;
        public bool Exhausted => _exhaust.Exhausted;
        public bool Channeling => _channel.Channeling;
        public bool Tapped => _tapped;

        // [SerializeField] private RectTransform _uiStatusContainer;
        [SerializeField] private UIStatus _statusPrefab;

        [SerializeField] TMP_Text _display;

        EventBroadcaster _eventBroadcast;
        Position _position;
        Targetable _targetable;
        Combatant _combatant;

        private void Awake()
        {
            _position = GetComponent<Position>();
            _targetable = GetComponent<Targetable>();
            _combatant = GetComponent<Combatant>();
            _exhaust = new Exhaust(_position.CombatSide);
            _channel = new Channel(_position.CombatSide, _combatant);
        }

        private void Start()
        {
            // _combInst = CombatManager.Current;
            // _combInst.OnTurnEnd += _combInst_OnActionEnd;
            _eventBroadcast = CombatManager.Current.EventBroadcaster;
            _eventBroadcast.EndTurn += _combInst_OnActionEnd;
            
            UpdateDisplay();
        }

        public void Tap(bool tap){
            _tapped = tap;
        }

        public bool PassesChainConditions(){
            if (_targetable.TargetType == TargetType.Obstacle){
                return false;
            } 
            if (_position.Distance != CombatDistance.Front){
                return false;
            }
            if (StatusExists(StatusEffect.Isolated)){
                return false;
            }
            if (StatusExists(StatusEffect.Trance)){
                return false;
            }
            if (StatusExists(StatusEffect.Down)){
                return false;
            }
            if (Exhausted){
                return false;
            }  
            if (Channeling){
                return false;
            }  

            return true;
        }

        public bool PassesActionConditions(bool writeReason){
            string reason = "";
            bool returnValue = true;

            if (StatusExists(StatusEffect.Down)){
                reason += "\n" + _targetable.Name + " is not available because they are down";
                returnValue = false;
            } 
            if (StatusExists(StatusEffect.Stunned)){
                reason += "\n" + _targetable.Name + " is not available because they are stunned";
                returnValue = false;
            } 
            if (StatusExists(StatusEffect.Asleep)){
                reason += "\n" + _targetable.Name + " is not available because they are asleep";
                returnValue = false;
            } 
            if (Tapped){
                reason += "\n" + _targetable.Name + " is not available because they are tapped";
                returnValue = false;
            } 
            if (Exhausted){
                reason += "\n" + _targetable.Name + " is not available because they are exhausted";
                returnValue = false;
            } 
            if (Channeling){
                reason += "\n" + _targetable.Name + " is not available because they are channeling";
                returnValue = false;
            } 

            if (writeReason)
                print(reason);
            
            return returnValue;
        }

        private void _combInst_OnActionEnd(object sender, EventArgs e)
        {
            List<Status> markedForRemoval = new List<Status>();

            foreach (var s in _statusEffects)
            {
                s.DurationDown();

                if (s.KillMe && s.Duration <= 0)
                    markedForRemoval.Add(s);
            }

            foreach (var m in markedForRemoval)
            {
                RemoveStatusEffect(m);
                UpdateDisplay();
            }
        }

        internal void Heal()
        {
            if (!StatusExists(StatusEffect.Wounded)) return;

            var woundedStat = _statusEffects.Where(s => s.StatusEffect == StatusEffect.Wounded).FirstOrDefault();
            RemoveStatusEffect(woundedStat);
            UpdateDisplay();
        }

        internal void Revive()
        {
            if (!StatusExists(StatusEffect.Down)) return;

            ClearAllStatus();
            UpdateDisplay();
        }

        public void UpdateDisplay(){
            _display.text = "";
            _display.text += GetComponent<Targetable>().Name;
            _display.text += "\n" + GetComponent<Position>().Distance;
            foreach(Status s in StatusEffects){
                _display.text += "\n" + s.StatusEffect;
            }
            _display.text += "\n" + _currentStance;
            _display.text += "\n" + "Exhaust: " + _exhaust.ExhaustTime;
            _display.text += "\n" + "Channel: " + _channel.ChannelTime;
        }

        private void RemoveStatusEffect(Status stat)
        {
            // var matchingStatus = _uiStatusContainer.GetComponentsInChildren<UIStatus>().Where(s => s.Status == stat).FirstOrDefault();

            // if (matchingStatus != null)
            //     Destroy(matchingStatus.gameObject);

            if (_statusEffects.Contains(stat)){
                _statusEffects.Remove(stat);
                
            }
                
        }

        internal void Dispel()
        {
            var clearableStatusList = _statusEffects.Where(s => DetermineDispel(s.StatusEffect) == true).ToList();

            foreach (var s in clearableStatusList)
            {
                RemoveStatusEffect(s);
            }

            UpdateDisplay();
        }

        public bool StatusExists(StatusEffect status)
        {
            var relevantStatus = _statusEffects.Where(s => s.StatusEffect == status).ToList();

            return relevantStatus.Count() > 0;
        }

        public void ChangeStance(Stance stance){
            _currentStance = stance;
            UpdateDisplay();
        }

        public void BreakStance(){
            _currentStance = Stance.None;
            UpdateDisplay();
        }

        public void TakeDamage()
        {
            if (StatusExists(StatusEffect.Wounded))
                Death();
            else
                CreateNewStatus(StatusEffect.Wounded, 0, canExpire: false);
        }

        private bool DodgeRoll()
        {
            var baseChance = 40f;
            var random = UnityEngine.Random.Range(0f, 100f);

            print(random);

            if (random < baseChance){
                return true;
            }

            return false;
        }

        public void CreateNewStatus(StatusEffect status, int duration, bool canExpire)
        {
            Status newStatus = new Status();
            newStatus.SetStatus(status, duration);
            _statusEffects.Add(newStatus);
            // var uiStatus = Instantiate(_statusPrefab, _uiStatusContainer);
            // uiStatus.SetStatus(newStatus);
            UpdateDisplay();
        }

        private static bool DetermineDispel(StatusEffect status)
        {
            bool dispel;

            switch (status)
            {
                default:
                case StatusEffect.None:
                // case StatusEffect.Enraged:
                case StatusEffect.Wounded:
                case StatusEffect.Down:
                // case StatusEffect.Trapped:
                // case StatusEffect.Asleep:
                // case StatusEffect.Guard:
                // case StatusEffect.Protect:
                // case StatusEffect.Alert:
                // case StatusEffect.Counter:
                // case StatusEffect.Sync:
                    dispel = false;
                    break;
                case StatusEffect.Stunned:
                // case StatusEffect.Burned:
                // case StatusEffect.Madness:
                // case StatusEffect.Blind:
                // case StatusEffect.Bubble:
                // case StatusEffect.Genera:
                // case StatusEffect.Mirror:
                // case StatusEffect.Angel:
                // case StatusEffect.Trance:
                // case StatusEffect.Mute:
                // case StatusEffect.Phase:
                // case StatusEffect.Invulnerable:
                // case StatusEffect.Blur:
                    dispel = true;
                    break;
            }

            return dispel;
        }

        public void Death()
        {
            ClearAllStatus();
            CreateNewStatus(StatusEffect.Down, 0, canExpire: false);
            UpdateDisplay();
        }

        private void ClearAllStatus()
        {
            _statusEffects.Clear();
            // foreach (var c in _uiStatusContainer.GetComponentsInChildren<UIStatus>())
            // {
            //     Destroy(c.gameObject);
            // }
            UpdateDisplay();
        }
    }

    [System.Serializable]
    public class Status
    {
        public StatusEffect StatusEffect;

        [ShowIf("@this.Duration > 0")]
        public int Duration;

        [ShowIf("KillMe")]
        public bool KillMe;

        public void SetStatus(StatusEffect status, int duration)
        {
            StatusEffect = status;
            Duration = duration;
        }

        public void DurationDown()
        {
            if (Duration == 0) return;

            Duration--;

            if (Duration == 0) KillMe = true;
        }
    }
}
