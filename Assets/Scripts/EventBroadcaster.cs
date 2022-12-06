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

        private bool _actionStarted = false;
        [SerializeField]
        private bool _displayTurnDescriptors = false;

        private void EndEventPhase()
        {

            PrintDescriptor("1.End of event phase");
            EndOfEventPhase?.Invoke(this, EventArgs.Empty);
        }

        public void PrintDescriptor(string text){
            if (!_displayTurnDescriptors) return;

            print(text);
        }

        public void StartEventPhase()
        {
            PrintDescriptor("1.Start of Event Phase");
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
            PrintDescriptor("2.Start of turn: Ally");
            StartOfAlliesTurn?.Invoke(this, EventArgs.Empty);
        }

        public void CallStartOfEnemiesTurn()
        {
            PrintDescriptor("2.Start of turn: Enemy");
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
            if (!_actionStarted){
                PrintDescriptor("     2A.Start of Action");
                _actionStarted = true;
                StartOfAction?.Invoke(this, EventArgs.Empty);
            }
            else{
                PrintDescriptor("     2A.Start of Sequenced-Action");
            }
        }

        public void CallActionPrompt(){
            PromptAction?.Invoke(this, EventArgs.Empty);
        }

        public void CallEndOfAction(){
            PrintDescriptor("     2A.End of Action");
            _actionStarted = false;
            EndOfAction?.Invoke(this, EventArgs.Empty);   
        }

        public void CallEndOfEnemiesTurn(){
            PrintDescriptor("2.End Of Enemies Turn");
            EndOfEnemiesTurn?.Invoke(this, EventArgs.Empty);
        }

        public void CallEndOfAlliesTurn(){
            PrintDescriptor("2.End Of Allies Turn");
            EndOfAlliesTurn?.Invoke(this, EventArgs.Empty);
        }
    }
}