using System;
using UnityEngine;

namespace TTW.Combat {
    public class EventBroadcaster : MonoBehaviour
    {
        [SerializeField] CombatManager _manager;
        public event EventHandler EndOfAlliesTurn;
        public event EventHandler EndOfEnemiesTurn;
        public event EventHandler StartOfAlliesTurn;
        public event EventHandler StartOfEnemiesTurn;
        public event EventHandler EndOfAction;
        public event EventHandler StartOfAction;
        public event EventHandler StartOfEventPhase;
        public event EventHandler EndOfEventPhase;
        public event EventHandler EndTurnPrompt;
        public event EventHandler PromptAction;

        private void EndEventPhase()
        {
            print("end of event phase");
            EndOfEventPhase?.Invoke(this, EventArgs.Empty);
        }

        public void StartEventPhase()
        {
            print("Start of Event Phase");
            StartOfEventPhase?.Invoke(this, EventArgs.Empty);

            if (!CheckForEvents())
            {
                EndEventPhase();
            }
            else
            {
                //perform events
            }
        }

        public void CallStartOfAlliesTurn()
        {
            print("start of turn: Ally");
            StartOfAlliesTurn?.Invoke(this, EventArgs.Empty);
        }

        public void CallStartOfEnemiesTurn()
        {
            print("start of turn: Enemy");
            StartOfEnemiesTurn?.Invoke(this, EventArgs.Empty);
        }

        public void CallEndTurnPrompt(){
            EndTurnPrompt?.Invoke(this, EventArgs.Empty);
        }

        private bool CheckForEvents()
        {
            return false;
        }

        public void CallStartAction(){
            print("Start of Action");
            StartOfAction?.Invoke(this, EventArgs.Empty);
        }

        public void CallActionPrompt(){
            print("Action Prompt");
            PromptAction?.Invoke(this, EventArgs.Empty);
        }

        public void CallEndOfAction(){
            print("End of Action");
            EndOfAction?.Invoke(this, EventArgs.Empty);
        }

        public void CallEndOfEnemiesTurn(){
            print("End Of Enemies Turn");
            EndOfEnemiesTurn?.Invoke(this, EventArgs.Empty);
        }

        public void CallEndOfAlliesTurn(){
            print("End Of Allies Turn");
            EndOfAlliesTurn?.Invoke(this, EventArgs.Empty);
        }
    }
}