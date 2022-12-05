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
            print("1.End of event phase");
            EndOfEventPhase?.Invoke(this, EventArgs.Empty);
        }

        public void StartEventPhase()
        {
            print("1.Start of Event Phase");
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
            print("2.Start of turn: Ally");
            StartOfAlliesTurn?.Invoke(this, EventArgs.Empty);
        }

        public void CallStartOfEnemiesTurn()
        {
            print("2.Start of turn: Enemy");
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
            print("     2A.Start of Action");
            StartOfAction?.Invoke(this, EventArgs.Empty);
        }

        public void CallActionPrompt(){
            PromptAction?.Invoke(this, EventArgs.Empty);
        }

        public void CallEndOfAction(){
            print("     2A.End of Action");
            EndOfAction?.Invoke(this, EventArgs.Empty);
        }

        public void CallEndOfEnemiesTurn(){
            print("2.End Of Enemies Turn");
            EndOfEnemiesTurn?.Invoke(this, EventArgs.Empty);
        }

        public void CallEndOfAlliesTurn(){
            print("2.End Of Allies Turn");
            EndOfAlliesTurn?.Invoke(this, EventArgs.Empty);
        }
    }
}