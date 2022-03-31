using System.Collections;
using System.Collections.Generic;
using TTW.Systems;
using UnityEngine;
using TMPro;
using TTW.Combat;

namespace TTW.UI
{
    public class UIAbility : MonoBehaviour
    {
        [SerializeField] AbilityData ability;

        UISelectionController ui;
        UIAbilityList parent;
        private void Awake()
        {
            parent = GetComponentInParent<UIAbilityList>();
            GetComponentInChildren<TMP_Text>().text = ability.Name;
        }
        private void Start()
        {
            ui = UISelectionController.Current;
        }

        public void SelectAbility()
        {
            if (ui.SelectionState != SelectionState.Ability) return;
            if (parent.Animating) return;

            CombatInstance.Current.SetAbility(ability);

            ui.ChangeSelectionState(SelectionState.Target);
        }
    }
}