using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.UI
{
    public enum SelectionState
    {
        Ability,
        Target,
        Wait
    }

    public class UISelectionController : MonoBehaviour
    {
        private static SelectionState _selectionState = SelectionState.Ability;
        public static UISelectionController Current;
        [SerializeField] Texture2D targetCursor;
        [SerializeField] Texture2D actorSelectCursor;
        [SerializeField] Texture2D waitCursor;

        CursorMode cursorMode = CursorMode.Auto;
        Vector2 hotSpot = Vector2.zero;

        private void Awake()
        {
            Current = this;
        }

        private void Start()
        {
            ChangeSelectionState(SelectionState.Ability);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
                Cancel();
        }

        public void ChangeSelectionState(SelectionState state)
        {
            _selectionState = state;

            if (state == SelectionState.Ability)
                ResetUI();
            
            ChangeCursor(state);
        }

        public void Cancel()
        {
            ChangeSelectionState(SelectionState.Ability);
        }

        private void ResetUI()
        {
            var abilityLists = FindObjectsOfType<UIAbilityList>();

            foreach (var a in abilityLists)
            {
                a.CloseList();
            }
        }

        public void ChangeCursor(SelectionState state)
        {
            switch (state)
            {
                case SelectionState.Ability:
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                    break;
                case SelectionState.Target:
                    Cursor.SetCursor(targetCursor, hotSpot, cursorMode);
                    break;
                case SelectionState.Wait:
                    Cursor.SetCursor(waitCursor, hotSpot, cursorMode);
                    break;
                default:
                    break;
            }
        }

        public SelectionState SelectionState => _selectionState;
    }
}
