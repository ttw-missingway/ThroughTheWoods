using System.Collections;
using System.Collections.Generic;
using TMPro;
using TTW.Systems;
using TTW.UI;
using UnityEngine;

namespace TTW.Combat
{
    public class UIActor : MonoBehaviour
    {
        [SerializeField] Combatant _actor;
        [SerializeField] UIAbilityList _abilityList;
        [SerializeField] TMP_Text _exhaustText;

        private void Awake()
        {
            _actor = GetComponent<Combatant>();
            _actor.onExhaustUpdate += _actor_onExhaustUpdate;
        }

        private void Start()
        {
            WriteExhaustTime();
        }

        private void _actor_onExhaustUpdate(object sender, System.EventArgs e)
        {
            WriteExhaustTime();
        }

        private void WriteExhaustTime()
        {
            _exhaustText.text = "ex: " + _actor.ExhaustTime.ToString();
            _exhaustText.text += "\n ch: " + _actor.ChannelTime.ToString();
        }

        public void SelectActor()
        {
            if (UISelectionController.Current.SelectionState != SelectionState.Ability) return;
            if (_actor.Channeling || _actor.Exhausted) return;

            _abilityList.OpenList();
            // CombatManager.Current.SetFocus(_actor);
        }
    }
}