using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TTW.UI
{
    public class UIAbilityList : MonoBehaviour
    {
        [SerializeField] Vector3 startingPoint;
        [SerializeField] Vector3 endingPoint;
        [SerializeField] float tweenTime;
        [SerializeField] bool listOpen = false;
        private bool _animating = false;

        public bool Animating => _animating;

        UISelectionController ui;

        private void Start()
        {
            ui = UISelectionController.Current;
        }

        public void OpenList()
        {
            if (ui.SelectionState != SelectionState.Ability) return;

            if (listOpen)
            {
                CloseList();
                return;
            }

            var allOtherAbilityLists = FindObjectsOfType<UIAbilityList>().Where(a => a != this);

            foreach (var a in allOtherAbilityLists)
                a.CloseList();

            LeanTween.move(GetComponent<RectTransform>(), endingPoint, tweenTime).setEaseInOutSine().setOnComplete(EndTween);

            listOpen = true;
            _animating = true;
        }

        private void EndTween()
        {
            _animating = false;
        }

        public void CloseList()
        {
            if (ui == null) return;
            if (ui.SelectionState != SelectionState.Ability) return;

            LeanTween.move(GetComponent<RectTransform>(), startingPoint, tweenTime).setEaseInOutSine().setOnComplete(EndTween);
            listOpen = false;
            _animating = true;
        }
    }
}
