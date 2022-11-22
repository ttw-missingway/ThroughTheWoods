using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat {
    public class EventBroadcaster : MonoBehaviour
    {
        [SerializeField] CombatManager _manager;
        public event EventHandler EndTurn;
        public event EventHandler StartTurnAlly;
        public event EventHandler StartTurnEnemy;
        public event EventHandler StartTurn;
        public event EventHandler EndAction;
        private bool _endOfActionFlag = false;
        public bool EndOfActionFlag => _endOfActionFlag;

        private void EndEventPhase()
        {
            print("end of event phase");
            _manager.ChangeState(CombatState.Control);
        }

        public void ReadyForNewActionChain(bool inProgress){
            _endOfActionFlag = inProgress;
        }

        public void StartEventPhase()
        {
            print("Start of Event Phase");
            if (!CheckForEvents())
            {
                EndEventPhase();
                CallStartTurn();
            }
            else
            {
                //perform events
            }
        }

        private void CallStartTurn()
        {
            if (_manager.Turn == CombatSide.Ally)
            {
                print("start of turn: Ally");
                StartTurnAlly?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                print("start of turn: Enemy");
                StartTurnEnemy?.Invoke(this, EventArgs.Empty);
            }

            StartTurn?.Invoke(this, EventArgs.Empty);
        }

        private bool CheckForEvents()
        {
           //check for events, return true if

            return false;
        }

        public void EndOfAnimation(){
            // if (_manager.State == CombatState.Event){
            //     EndEventPhase();
            // }
            if (_manager.State == CombatState.Control){
                if (_manager.NoCombatantsRemaining()){
                    print("end of turn");
                    CallEndTurn();
                }
                else if (_endOfActionFlag){
                    print("end of action");
                    _manager.ChangeState(CombatState.Control);
                    CallEndOfAction();
                }
            }
        }

        public void CallEndOfAction(){
            EndAction?.Invoke(this, EventArgs.Empty);
        }

        public void CallEndTurn(){
            EndTurn?.Invoke(this, EventArgs.Empty);
        }
    }
}