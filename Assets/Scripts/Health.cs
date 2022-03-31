using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTW.Systems;
using TTW.UI;
using UnityEngine;

namespace TTW.Combat
{
    public class Health: MonoBehaviour
    {
        [SerializeField] private List<Status> _statusEffects = new List<Status>();
        public List<Status> StatusEffects => _statusEffects;
        [SerializeField] private RectTransform _uiStatusContainer;
        [SerializeField] private UIStatus _statusPrefab;

        CombatInstance _combInst;

        private void Start()
        {
            _combInst = CombatInstance.Current;
            _combInst.OnActionEnd += _combInst_OnActionEnd;
        }

        private void _combInst_OnActionEnd(object sender, EventArgs e)
        {
            List<Status> markedForRemoval = new List<Status>();

            foreach (var s in _statusEffects)
            {
                s.DurationDown();

                if (s.CanExpire && s.Duration <= 0)
                    markedForRemoval.Add(s);
            }

            foreach (var m in markedForRemoval)
            {
                RemoveStatusEffect(m);
            }
        }

        internal void Heal()
        {
            if (!StatusExists(StatusEffect.Wounded)) return;

            var woundedStat = _statusEffects.Where(s => s.StatusEffect == StatusEffect.Wounded).FirstOrDefault();
            RemoveStatusEffect(woundedStat);
        }

        internal void Revive()
        {
            if (!StatusExists(StatusEffect.Down)) return;

            ClearAllStatus();
            GetComponent<Targetable>().ResetTargetType();
        }

        private void RemoveStatusEffect(Status stat)
        {
            var matchingStatus = _uiStatusContainer.GetComponentsInChildren<UIStatus>().Where(s => s.Status == stat).FirstOrDefault();

            if (matchingStatus != null)
                Destroy(matchingStatus.gameObject);

            if (_statusEffects.Contains(stat))
                _statusEffects.Remove(stat);
        }

        internal void Dispel()
        {
            var clearableStatusList = _statusEffects.Where(s => DetermineDispel(s.StatusEffect) == true).ToList();

            foreach (var s in clearableStatusList)
            {
                RemoveStatusEffect(s);
            }
        }

        public bool StatusExists(StatusEffect status)
        {
            var relevantStatus = _statusEffects.Where(s => s.StatusEffect == status).ToList();

            return relevantStatus.Count() > 0;
        }

        public void TakeDamage()
        {
            if (StatusExists(StatusEffect.Wounded))
                Death();
            else
                CreateNewStatus(StatusEffect.Wounded, 0, canExpire: false);

            print(gameObject.name + " took damage!");
        }

        public void CreateNewStatus(StatusEffect status, int duration, bool canExpire)
        {
            Status newStatus = new Status(status, duration, canExpire);
            _statusEffects.Add(newStatus);
            var uiStatus = Instantiate(_statusPrefab, _uiStatusContainer);
            uiStatus.SetStatus(newStatus);
        }

        private static bool DetermineDispel(StatusEffect status)
        {
            bool dispel;

            switch (status)
            {
                default:
                case StatusEffect.None:
                case StatusEffect.Enraged:
                case StatusEffect.Wounded:
                case StatusEffect.Down:
                case StatusEffect.Trapped:
                case StatusEffect.Asleep:
                    dispel = false;
                    break;
                case StatusEffect.Stunned:
                case StatusEffect.Burned:
                case StatusEffect.Madness:
                case StatusEffect.Blind:
                case StatusEffect.Bubble:
                case StatusEffect.Genera:
                case StatusEffect.Mirror:
                case StatusEffect.Angel:
                case StatusEffect.Trance:
                case StatusEffect.Mute:
                case StatusEffect.Phase:
                case StatusEffect.Invulnerable:
                    dispel = true;
                    break;
            }

            return dispel;
        }

        private void Death()
        {
            ClearAllStatus();
            CreateNewStatus(StatusEffect.Down, 0, canExpire: false);
            GetComponent<Targetable>().SetTargetType(TargetType.Down);
        }

        private void ClearAllStatus()
        {
            _statusEffects.Clear();
            foreach (var c in _uiStatusContainer.GetComponentsInChildren<UIStatus>())
            {
                Destroy(c.gameObject);
            }
        }
    }

    [System.Serializable]
    public class Status
    {
        public StatusEffect StatusEffect { get; set; }
        public int Duration { get; set; }
        public bool CanExpire { get; set; }

        public Status(StatusEffect status, int duration, bool canExpire)
        {
            StatusEffect = status;
            Duration = duration;
            CanExpire = canExpire;
        }

        public void DurationDown()
        {
            if (!CanExpire) return;

            Duration--;
        }
    }
}
